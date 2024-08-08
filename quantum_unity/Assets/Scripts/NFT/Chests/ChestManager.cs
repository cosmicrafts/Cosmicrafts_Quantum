using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Candid;
using Cosmicrafts.MainCanister.Models;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.Data;
using Cosmicrafts.Managers;

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
        private List<UnboundedUInt> currentTokenIds = new List<UnboundedUInt>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        async void Start()
        {
            InitializeChestDictionary();
            chestPrefab.SetActive(false);
            await FetchOwnedChests();
            if (updateChestsToggle != null)
            {
                updateChestsToggle.onValueChanged.AddListener((value) =>
                {
                    if (value) UpdateOwnedChests();
                });
            }
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

        public void RemoveChestAndRefreshCount(UnboundedUInt tokenId)
        {
            ChestInstance[] chestInstances = chestDisplayContainer.GetComponentsInChildren<ChestInstance>();
            foreach (ChestInstance chestInstance in chestInstances)
            {
                if (chestInstance.tokenId == tokenId)
                {
                    Destroy(chestInstance.gameObject);
                    if (currentTokenIds.Remove(tokenId))
                    {
                        ownedChestsText.text = $"{currentTokenIds.Count}";
                    }
                    break;
                }
            }
        }

        private async Task FetchOwnedChests()
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

            var account = new Account(Principal.FromText(userData.PrincipalId), null);
            var tokenIds = await GetOwnedChestTokens(account);

            if (tokenIds != null && tokenIds.Count > 0)
            {
                ownedChestsText.text = $"{tokenIds.Count}";
                foreach (var tokenId in tokenIds)
                {
                    await FetchAndSetChestData(tokenId);
                }
            }
            currentTokenIds = tokenIds;
        }

        private async Task FetchAndSetChestData(UnboundedUInt tokenId)
        {
            var metadataResult = await CandidApiManager.Instance.MainCanister.Icrc7Metadata(tokenId);
            if (metadataResult.Tag == MetadataResultTag.Ok)
            {
                if (metadataResult.Value is Dictionary<string, Metadata> metadataDictionary &&
                    metadataDictionary.TryGetValue("general", out Metadata generalMetadata))
                {
                    GeneralMetadata general = generalMetadata.General; // Directly access the General property

                    int rarity = general.Rarity.HasValue ? (int)general.Rarity.GetValueOrDefault() : 0;
                    if (chestDictionary.TryGetValue(rarity, out ChestSO chestSO))
                    {
                        InstantiateChestPrefab(tokenId, chestSO);
                    }
                    else
                    {
                        Debug.LogError($"[Chest Manager] No ChestSO found for rarity {rarity}. Ensure ChestSOs are correctly set up in Resources/Chests.");
                    }
                }
                else
                {
                    Debug.LogError("[Chest Manager] Metadata does not contain 'general' key or casting failed.");
                }
            }
            else
            {
                Debug.LogError($"[Chest Manager] Failed to fetch metadata for token ID {tokenId}. Metadata result tag: {metadataResult.Tag}");
            }
        }


        private void InstantiateChestPrefab(UnboundedUInt tokenId, ChestSO chestSO)
        {
            GameObject instance = Instantiate(chestPrefab, chestDisplayContainer);
            ChestInstance chestInstance = instance.GetComponent<ChestInstance>();
            if (chestInstance != null)
            {
                chestInstance.Setup(chestSO, tokenId);
            }
            instance.SetActive(true);
        }

        private async Task<List<UnboundedUInt>> GetOwnedChestTokens(Account account)
        {
            var tokenListResult = await CandidApiManager.Instance.MainCanister.Icrc7TokensOf(account);
            if (tokenListResult.Tag == TokensOfResultTag.Ok)
            {
                return tokenListResult.AsOk();
            }
            else
            {
                Debug.LogError("[Chest Manager] Failed to fetch owned chest tokens.");
                return new List<UnboundedUInt>();
            }
        }

        public async void UpdateOwnedChests()
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

            var account = new Account(Principal.FromText(userData.PrincipalId), null);
            var newTokenIds = await GetOwnedChestTokens(account);

            int newChestCount = newTokenIds.Except(currentTokenIds).Count();

            if (newChestCount > 0)
            {
                ownedChestsText.text = $"{newTokenIds.Count}";
                foreach (var tokenId in newTokenIds.Except(currentTokenIds))
                {
                    await FetchAndSetChestData(tokenId);
                }

                currentTokenIds.AddRange(newTokenIds.Except(currentTokenIds));

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

            var transferArgs = new Cosmicrafts.MainCanister.Models.TransferArgs
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

    }
}