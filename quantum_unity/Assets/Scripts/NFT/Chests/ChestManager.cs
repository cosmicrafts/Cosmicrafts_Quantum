using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Candid;
using CanisterPK.chests.Models;
using EdjCase.ICP.Candid.Models;
using System.Numerics;
using Cosmicrafts.Data;
using Cosmicrafts.Managers;

public class ChestManager : MonoBehaviour
{
    [SerializeField] private ChestSO[] chestSOsByRarity;
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
            Debug.LogError("[ChestManager] GameDataManager instance is null.");
            return;
        }

        var userData = GameDataManager.Instance.playerData;
        if (userData == null)
        {
            Debug.LogError("Failed to load player data.");
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
        var metadataResult = await CandidApiManager.Instance.chests.Icrc7Metadata(tokenId);
        if (metadataResult.Tag == MetadataResultTag.Ok)
        {
            var metadataDictionary = metadataResult.Value as Dictionary<string, Metadata>;
            if (metadataDictionary != null && metadataDictionary.TryGetValue("rarity", out Metadata metadata))
            {
                int rarity = (int)metadata.AsNat();
                if (rarity >= 1 && rarity <= chestSOsByRarity.Length)
                {
                    ChestSO chestSO = chestSOsByRarity[rarity - 1];
                    InstantiateChestPrefab(tokenId, chestSO);
                }
                else
                {
                    Debug.LogError($"No ChestSO found for rarity {rarity}.");
                }
            }
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
        var tokenListResult = await CandidApiManager.Instance.chests.Icrc7TokensOf(account);
        return tokenListResult.Tag == TokensOfResultTag.Ok ? tokenListResult.AsOk() : new List<UnboundedUInt>();
    }

    public async void UpdateOwnedChests()
    {
        if (GameDataManager.Instance == null)
        {
            Debug.LogError("[ChestManager] GameDataManager instance is null.");
            return;
        }

        var userData = GameDataManager.Instance.playerData;
        if (userData == null)
        {
            Debug.LogError("Failed to load player data.");
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

            string notificationText = $"Received {newChestCount} new chest{(newChestCount > 1 ? "s" : "")}!";
            notificationManager.ShowNotification(notificationText);
        }
    }

    public async Task TransferChest(ChestSO chestSO, UnboundedUInt tokenId, string recipientPrincipalText)
    {
        if (GameDataManager.Instance == null)
        {
            Debug.LogError("[ChestManager] GameDataManager instance is null.");
            return;
        }

        var userData = GameDataManager.Instance.playerData;
        if (userData == null)
        {
            Debug.LogError("Failed to load player data.");
            return;
        }

        Debug.Log($"Attempting to transfer chest: {chestSO.chestName} with Token ID: {tokenId} to {recipientPrincipalText}");
        var recipientPrincipal = Principal.FromText(recipientPrincipalText);

        var transferArgs = new CanisterPK.chests.Models.TransferArgs
        {
            From = new OptionalValue<Account>(new Account(Principal.FromText(userData.PrincipalId), null)),
            To = new Account(recipientPrincipal, null),
            TokenIds = new List<UnboundedUInt> { tokenId }
        };

        var transferReceipt = await CandidApiManager.Instance.chests.Icrc7Transfer(transferArgs);

        if (transferReceipt.Tag == TransferReceiptTag.Ok)
        {
            Debug.Log("Chest transferred successfully.");
            RemoveChestAndRefreshCount(tokenId);
        }
        else if (transferReceipt.Tag == TransferReceiptTag.Err)
        {
            TransferError errorInfo = transferReceipt.AsErr();
            Debug.LogError($"Failed to transfer chest: {errorInfo.Tag}");
        }
    }
}
