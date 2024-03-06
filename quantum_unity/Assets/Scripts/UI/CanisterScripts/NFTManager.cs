using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Candid;
using CanisterPK.testnft.Models;
using EdjCase.ICP.Candid.Models;

public class NFTManager : MonoBehaviour
{
    public static NFTManager Instance { get; private set; }

    // UI elements
    public TMP_Text nftListText;
    public GameObject nftPrefab;
    public Transform nftDisplayContainer;
    public NFTDisplay nftDisplay;
    public RectTransform nftDisplayContainerRectTransform;
    public static int cloneCounter = 0;

    private void Awake()
    {
        // Ensure only one instance of NFTManager exists
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
        // Deactivate prefab at start
        nftPrefab.SetActive(false);
        await FetchOwnedNFTs();
    }

    // Update NFT display with new data
    public void UpdateNFTDisplay(NFTData nftData)
    {
        nftDisplay.SetNFTData(nftData);
        RefreshUILayout();
    }

    // Refresh UI layout
    public void RefreshUILayout()
    {
        if (nftDisplayContainerRectTransform != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(nftDisplayContainerRectTransform);
        }
    }

    // Fetch user's owned NFTs
    async Task FetchOwnedNFTs()
    {
        string userPrincipalId = GlobalGameData.Instance.GetUserData().WalletId;
        var account = new Account(Principal.FromText(userPrincipalId), null);
        var tokens = await GetOwnedNFTs(account);

        if (tokens != null && tokens.Count > 0)
        {
            nftListText.text = $"Owned NFTs: {tokens.Count}";
            foreach (var tokenId in tokens)
            {
                await FetchAndSetNFTMetadata(tokenId, nftDisplay);
            }
        }
    }

    // Helper method to instantiate NFT prefab with data
    public void InstantiateNFTPrefab(NFTData data)
    {
        GameObject instance = Instantiate(nftPrefab, nftDisplayContainer);
        instance.SetActive(true);
        
        NFTItem nftItem = instance.GetComponent<NFTItem>();
        if (nftItem != null)
        {
            nftItem.SetNFTData(data);
        }
        else
        {
            Debug.LogError("NFTItem component not found on instantiated prefab.");
        }
    }

    // Fetch metadata for a specific NFT token and update UI
    async Task FetchAndSetNFTMetadata(UnboundedUInt tokenId, NFTDisplay displayComponent)
    {
        var metadataResult = await CandidApiManager.Instance.testnft.Icrc7Metadata(tokenId);
        if (metadataResult.Tag == MetadataResultTag.Ok && metadataResult.Value is Dictionary<string, Metadata> metadataDictionary)
        {
            NFTData nftData = NFTMetadataParser.Parse(metadataDictionary);

            // Instantiate prefab and set its NFTData
            var instance = Instantiate(nftPrefab, nftDisplayContainer);
            var nftItem = instance.GetComponent<NFTItem>();
            if (nftItem != null)
            {
                nftItem.SetNFTData(nftData);
                instance.SetActive(true);
            }

            // Update the display with fetched data
            displayComponent.SetNFTData(nftData);
            RefreshUILayout();
        }
        else
        {
            Debug.LogError($"Failed to fetch metadata for NFT with Token ID: {tokenId}");
        }
    }

    // Fetch user's owned NFT tokens
    async Task<List<UnboundedUInt>> GetOwnedNFTs(Account account)
    {
        var nftListResult = await CandidApiManager.Instance.testnft.Icrc7TokensOf(account);
        return nftListResult.Tag == TokensOfResultTag.Ok ? nftListResult.AsOk() : new List<UnboundedUInt>();
    }

    // Fetch metadata for an NFT token and display it
    async Task FetchAndDisplayNFTMetadata(UnboundedUInt tokenId)
    {
        var metadataResult = await CandidApiManager.Instance.testnft.Icrc7Metadata(tokenId);
        if (metadataResult.Tag == MetadataResultTag.Ok && metadataResult.Value is Dictionary<string, Metadata> metadataDictionary)
        {
            NFTData nftData = NFTMetadataParser.Parse(metadataDictionary);
            string nftInfo = GetNFTDataInfo(nftData);
            Debug.Log($"Parsed NFTData for Token ID {tokenId}: {nftInfo}");

            InstantiateAndDisplayNFT(nftData);
        }
    }

    // Get information about NFT data
    private string GetNFTDataInfo(NFTData nftData)
    {
        // Construct a string containing all the information about the NFTData
        string info = $"BasicStats:\n";
        foreach (var stat in nftData.BasicStats)
        {
            info += $"  {stat.StatName}: {stat.StatValue}\n";
        }
        info += $"General:\n";
        foreach (var generalInfo in nftData.General)
        {
            info += $"  Class: {generalInfo.Class}, Rarity: {generalInfo.Rarity}, Faction: {generalInfo.Faction}, Name: {generalInfo.Name}, Description: {generalInfo.Description}, Icon: {generalInfo.Icon}, SkinsText: {generalInfo.SkinsText}\n";
        }
        info += $"Skills:\n";
        foreach (var skill in nftData.Skills)
        {
            info += $"  {skill.SkillName}: {skill.SkillValue}\n";
        }
        info += $"Skins:\n";
        foreach (var skin in nftData.Skins)
        {
            info += $"  SkinId: {skin.SkinId}, SkinName: {skin.SkinName}, SkinDescription: {skin.SkinDescription}, SkinIcon: {skin.SkinIcon}, SkinRarity: {skin.SkinRarity}\n";
        }
        return info;
    }

    // Instantiate NFT and update display
    private void InstantiateAndDisplayNFT(NFTData nftData)
    {
        if (nftDisplay == null)
        {
            Debug.LogError("NFTDisplay component not found.");
            return;
        }
        nftDisplay.SetNFTData(nftData);
    }

    // Process metadata
    void ProcessMetadata(Metadata metadata, string indent = "")
    {
        switch (metadata.Tag)
        {
            case MetadataTag.Blob:
                List<byte> blob = metadata.AsBlob();
                Debug.Log($"{indent}Blob: {BitConverter.ToString(blob.ToArray())}");
                break;
            case MetadataTag.Int:
                UnboundedInt intValue = metadata.AsInt();
                Debug.Log($"{indent}Int: {intValue}");
                break;
            case MetadataTag.MetadataArray:
                var metadataArray = metadata.AsMetadataArray();
                foreach (var arrayEntry in metadataArray)
                {
                    // Log key and directly process value without indicating it's nested
                    ProcessMetadata(arrayEntry.Value, $"{arrayEntry.Key}: ");
                }
                break;
            case MetadataTag.Nat:
                UnboundedUInt natValue = metadata.AsNat();
                Debug.Log($"{indent}Nat: {natValue}");
                break;
            case MetadataTag.Text:
                string text = metadata.AsText();
                Debug.Log($"{indent}Text: {text}");
                break;
            default:
                Debug.LogError($"{indent}Unknown metadata tag.");
                break;
        }
    }

    public async Task<TransferReceipt> TransferNFT(List<UnboundedUInt> tokenIds, Principal recipientPrincipal) {
    var senderPrincipalText = GlobalGameData.Instance.GetUserData().WalletId; // Use the correct property
    var senderAccount = new Account(Principal.FromText(senderPrincipalText), new OptionalValue<List<byte>>()); // Adapt if necessary
    var recipientAccount = new Account(recipientPrincipal, new OptionalValue<List<byte>>()); // Adapt based on actual Account constructor
    
    var transferArgs = new TransferArgs {
        CreatedAtTime = new OptionalValue<ulong>(), // Set this if needed
        From = new OptionalValue<Account>(senderAccount),
        IsAtomic = new OptionalValue<bool>(true), // Set based on your requirement
        Memo = new OptionalValue<List<byte>>(System.Text.Encoding.UTF8.GetBytes("Transfer Memo").ToList()), // Ensure System.Linq is used
        SpenderSubaccount = new OptionalValue<List<byte>>(), // Assuming no specific subaccount
        To = recipientAccount,
        TokenIds = tokenIds // Correctly using List<UnboundedUInt>
    };

    return await CandidApiManager.Instance.testnft.Icrc7Transfer(transferArgs);
}



}
