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

public class ChestManager : MonoBehaviour
{
    [SerializeField] private ChestSO[] chestSOsByRarity; 
    public static ChestManager Instance { get; private set; } 
    public TMP_Text ownedChestsText;
    public GameObject chestPrefab;
    public Transform chestDisplayContainer;
    public Toggle updateChestsToggle;
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
            updateChestsToggle.onValueChanged.AddListener((value) => {
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
        string userPrincipalId = GlobalGameData.Instance.GetUserData().WalletId;
        var account = new Account(Principal.FromText(userPrincipalId), null);
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
                // Ensure the rarity is an int and within the bounds of your ChestSO array
                int rarity = (int)metadata.AsNat();
                if (rarity >= 1 && rarity <= chestSOsByRarity.Length)
                {
                    ChestSO chestSO = chestSOsByRarity[rarity - 1]; // Adjust for zero-based indexing
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

    private async Task<List<UnboundedUInt>> GetOwnedChestTokens(Account account) // Renamed for clarity
    {
        var tokenListResult = await CandidApiManager.Instance.chests.Icrc7TokensOf(account);
        return tokenListResult.Tag == TokensOfResultTag.Ok ? tokenListResult.AsOk() : new List<UnboundedUInt>();
    }

    public async void UpdateOwnedChests()
    {
        string userPrincipalId = GlobalGameData.Instance.GetUserData().WalletId;
        var account = new Account(Principal.FromText(userPrincipalId), null);
        var newTokenIds = await GetOwnedChestTokens(account);

        if (newTokenIds.Except(currentTokenIds).Any()) // Check for new chests
        {
            ownedChestsText.text = $"{newTokenIds.Count}";
            foreach (var tokenId in newTokenIds.Except(currentTokenIds))
            {
                await FetchAndSetChestData(tokenId);
            }
            currentTokenIds = newTokenIds; // Update the current list
        }
    }

    public async Task TransferChest(ChestSO chestSO, UnboundedUInt tokenId, string recipientPrincipalText)
    {
    Debug.Log($"Attempting to transfer chest: {chestSO.chestName} with Token ID: {tokenId} to {recipientPrincipalText}");
    var recipientPrincipal = Principal.FromText(recipientPrincipalText);
    // Prepare the transfer arguments
    var transferArgs = new CanisterPK.chests.Models.TransferArgs
    {
        // Assuming you have a sender account setup similar to NFTManager
        From = new OptionalValue<Account>(new Account(Principal.FromText(GlobalGameData.Instance.GetUserData().WalletId), null)),
        To = new Account(recipientPrincipal, null),
        TokenIds = new List<UnboundedUInt> { tokenId }
    };
    // Perform the transfer
    var transferReceipt = await CandidApiManager.Instance.chests.Icrc7Transfer(transferArgs);
    
    // Check the Tag to determine if the transfer was successful
    if (transferReceipt.Tag == TransferReceiptTag.Ok)
    {
        Debug.Log("Chest transferred successfully.");
        // Update UI and internal state
        RemoveChestAndRefreshCount(tokenId);
    }
    else if (transferReceipt.Tag == TransferReceiptTag.Err)
    {
        // Extract error information if available
        TransferError errorInfo = transferReceipt.AsErr();
        // Handle different error types (you may add more detailed handling based on error types)
        Debug.LogError($"Failed to transfer chest: {errorInfo.Tag}");
    }
    }
}
