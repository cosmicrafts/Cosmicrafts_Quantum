using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using Cosmicrafts.Managers;
using Cosmicrafts.MainCanister.Models;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.Data;
using System.Threading.Tasks;
using System.Numerics;
using Candid;
using System.Collections;

namespace Cosmicrafts
{
    public class ChestManager : MonoBehaviour
    {
        private Dictionary<int, ChestSO> chestDictionary;

        public static ChestManager Instance { get; private set; }
        public TMP_Text ownedChestsText;
        public GameObject chestPrefab;
        public Transform chestDisplayContainer;
        public Toggle updateChestsToggle;
        public NotificationManager notificationManager;
        private List<string> currentTokenIds = new List<string>();

        void Start()
        {
            InitializeChestDictionary();
            chestPrefab.SetActive(false);
            StartCoroutine(DelayedFetch());
            if (updateChestsToggle != null)
            {
                updateChestsToggle.onValueChanged.AddListener((value) =>
                {
                    if (value) UpdateOwnedChests();
                });
            }
        }

        private IEnumerator DelayedFetch()
        {
            yield return new WaitForSeconds(0.5f); // Adjust the delay as necessary
            FetchOwnedChests();
        }

        private void InitializeChestDictionary()
        {
            chestDictionary = new Dictionary<int, ChestSO>();
            ChestSO[] chests = Resources.LoadAll<ChestSO>("Chests");

            if (chests.Length == 0)
            {
                Debug.LogError("[Chest Manager] No ChestSO files found in Resources/Chests folder.");
            }

            foreach (var chest in chests)
            {
                if (chest != null)
                {
                    if (!chestDictionary.ContainsKey(chest.rarity))
                    {
                        chestDictionary[chest.rarity] = chest;
                    }
                    else
                    {
                        Debug.LogError($"[Chest Manager] Duplicate ChestSO entry found for rarity {chest.rarity}. Check for duplicate entries.");
                    }
                }
                else
                {
                    Debug.LogError("[Chest Manager] Found a null ChestSO entry during initialization. Ensure all chest SOs are valid.");
                }
            }
        }

        private void FetchOwnedChests()
        {
            // Get the list of Chest NFTs from the NFTManager
            var chestNFTs = NFTManager.Instance.AllNFTDatas.Where(nft => nft.Category.TagName == "Chest").ToList();

            if (chestNFTs.Count == 0)
            {
                Debug.LogWarning("[Chest Manager] No chests found for the player.");
                ownedChestsText.text = "0";
                return;
            }

            ownedChestsText.text = $"{chestNFTs.Count}";

            foreach (var nft in chestNFTs)
            {
                int rarity = nft.General.First().Rarity;
                if (chestDictionary.TryGetValue(rarity, out ChestSO chestSO))
                {
                    InstantiateChestPrefab(nft, chestSO);
                    currentTokenIds.Add(nft.TokenId);
                }
                else
                {
                    Debug.LogError($"[Chest Manager] No ChestSO found for rarity {rarity}. Ensure ChestSOs are correctly set up in Resources/Chests.");
                }
            }
        }

        private void InstantiateChestPrefab(NFTData nftData, ChestSO chestSO)
        {
            GameObject instance = Instantiate(chestPrefab, chestDisplayContainer);
            ChestInstance chestInstance = instance.GetComponent<ChestInstance>();
            if (chestInstance != null)
            {
                // Convert the TokenId to UnboundedUInt using the correct constructor or method
                var tokenId = UnboundedUInt.FromBigInteger(BigInteger.Parse(nftData.TokenId));
                chestInstance.Setup(chestSO, tokenId); // Pass the tokenId instead of the entire NFTData
            }
            instance.SetActive(true);
        }

        public void UpdateOwnedChests()
        {
            // Get the latest list of Chest NFTs from the NFTManager
            var newChestNFTs = NFTManager.Instance.AllNFTDatas.Where(nft => nft.Category.TagName == "Chest").ToList();

            int newChestCount = newChestNFTs.Select(nft => nft.TokenId).Except(currentTokenIds).Count();

            if (newChestCount > 0)
            {
                ownedChestsText.text = $"{newChestNFTs.Count}";
                foreach (var nft in newChestNFTs)
                {
                    if (!currentTokenIds.Contains(nft.TokenId))
                    {
                        int rarity = nft.General.First().Rarity;
                        if (chestDictionary.TryGetValue(rarity, out ChestSO chestSO))
                        {
                            InstantiateChestPrefab(nft, chestSO);
                        }
                        currentTokenIds.Add(nft.TokenId);
                    }
                }

                string notificationText = $"[Chest Manager] Received {newChestCount} new chest{(newChestCount > 1 ? "s" : "")}!";
                notificationManager.ShowNotification(notificationText);
            }
        }

        public async Task TransferChest(ChestSO chestSO, UnboundedUInt tokenId, string recipientPrincipalText)
        {
            if (GameDataManager.Instance == null)
            {
                Debug.LogError("[Chest Manager] GameDataManager instance is null.");
                return;
            }

            var userData = GameDataManager.Instance.playerData;
            if (userData == null)
            {
                Debug.LogError("[Chest Manager] Failed to load player data.");
                return;
            }

            Debug.Log($"[Chest Manager] Attempting to transfer chest: {chestSO.chestName} with Token ID: {tokenId} to {recipientPrincipalText}");
            var recipientPrincipal = Principal.FromText(recipientPrincipalText);

            var transferArgs = new TransferArgs
            {
                From = new OptionalValue<Account>(new Account(Principal.FromText(userData.PrincipalId), null)),
                To = new Account(recipientPrincipal, null),
                TokenIds = new List<UnboundedUInt> { tokenId }
            };

            var transferReceipt = await CandidApiManager.Instance.MainCanister.Icrc7Transfer(transferArgs);

            if (transferReceipt.Tag == TransferReceiptTag.Ok)
            {
                Debug.Log("[Chest Manager] Chest transferred successfully.");
                RemoveChestAndRefreshCount(tokenId);
            }
            else if (transferReceipt.Tag == TransferReceiptTag.Err)
            {
                // Assuming Transfererror1 is the correct one for NFTs
                Transfererror1 errorInfo = transferReceipt.AsErr(); 
                Debug.LogError($"[Chest Manager] Failed to transfer chest: {errorInfo.Tag}");
            }
        }

        public void RemoveChestAndRefreshCount(UnboundedUInt tokenId)
        {
            ChestInstance[] chestInstances = chestDisplayContainer.GetComponentsInChildren<ChestInstance>();
            foreach (ChestInstance chestInstance in chestInstances)
            {
                if (chestInstance.tokenId == tokenId)
                {
                    Destroy(chestInstance.gameObject);
                    if (currentTokenIds.Remove(tokenId.ToString()))
                    {
                        ownedChestsText.text = $"{currentTokenIds.Count}";
                    }
                    break;
                }
            }
        }
    }
}
