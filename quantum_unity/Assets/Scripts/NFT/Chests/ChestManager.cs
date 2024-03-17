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

public class ChestManager : MonoBehaviour
{
    public static ChestManager Instance { get; private set; } 

    public TMP_Text ownedChestsText; // Replace 'nftListText' with something chest-related
    public GameObject chestPrefab; 
    public ChestDisplay chestDisplay;
    public Transform chestDisplayContainer; // Renamed from 'nftDisplayContainer' 

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
        chestPrefab.SetActive(false); // Deactivate prefab at start 
        await FetchOwnedChests();
    }

    private async Task FetchOwnedChests()
    {
        string userPrincipalId = GlobalGameData.Instance.GetUserData().WalletId;
        var account = new Account(Principal.FromText(userPrincipalId), null);
        var tokenIds = await GetOwnedChestTokens(account);

        if (tokenIds != null && tokenIds.Count > 0)
        {
            ownedChestsText.text = $"Owned Chests: {tokenIds.Count}"; 
            foreach (var tokenId in tokenIds)
            {
                await FetchAndSetChestData(tokenId);
            }
        }
    }

    private async Task FetchAndSetChestData(UnboundedUInt tokenId)
    {
        var metadataResult = await CandidApiManager.Instance.chests.Icrc7Metadata(tokenId);
        if (metadataResult.Tag == MetadataResultTag.Ok && metadataResult.Value is Dictionary<string, Metadata> metadataDictionary)
        {
            ChestData chestData = ChestMetadataParser.Parse(metadataDictionary);
            chestDisplay.SetChestData(chestData); 
            InstantiateChestPrefab(tokenId, chestData.Rarity); 
        }
    }

    private void InstantiateChestPrefab(UnboundedUInt tokenId, int rarity)
    {
        GameObject instance = Instantiate(chestPrefab, chestDisplayContainer);
        instance.SetActive(true);
    }

    private async Task<List<UnboundedUInt>> GetOwnedChestTokens(Account account) // Renamed for clarity
    {
        var tokenListResult = await CandidApiManager.Instance.chests.Icrc7TokensOf(account);
        return tokenListResult.Tag == TokensOfResultTag.Ok ? tokenListResult.AsOk() : new List<UnboundedUInt>();
    }
}
