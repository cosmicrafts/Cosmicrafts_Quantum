using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System.Collections.Generic;
using CanisterPK.testnft.Models;
using EdjCase.ICP.Candid.Models;
using Candid;
using Newtonsoft.Json;
using System.Linq;
using System;
using UnityEngine.UI;


public class NFTManager : MonoBehaviour
{
    public static NFTManager Instance { get; private set; }
   public TMP_Text nftListText;
   public GameObject nftPrefab;
   public Transform nftDisplayContainer;
   public NFTDisplay nftDisplay;
   public static int cloneCounter = 0;
   public RectTransform nftDisplayContainerRectTransform;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }
   async void Start()
   {
       nftPrefab.SetActive(false);
       await FetchOwnedNFTs();
   }

   public void UpdateNFTDisplay(NFTData nftData)
    {
        nftDisplay.SetNFTData(nftData);
        RefreshUILayout();
    }

   public void RefreshUILayout()
    {
        if (nftDisplayContainerRectTransform != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(nftDisplayContainerRectTransform);
        }
    }

   async Task FetchOwnedNFTs() {
    string userPrincipalId = GlobalGameData.Instance.GetUserData().WalletId;
    var account = new Account(Principal.FromText(userPrincipalId), null);
    var tokens = await GetOwnedNFTs(account);

    if (tokens != null && tokens.Count > 0) {
        nftListText.text = $"Owned NFTs: {tokens.Count}";
        foreach (var tokenId in tokens) {
            await FetchAndSetNFTMetadata(tokenId, nftDisplay); // Assume nftDisplay is your UI component to show NFT data
        }
    }
}


public void InstantiateNFTPrefab(NFTData data) {
    GameObject instance = Instantiate(nftPrefab, nftDisplayContainer);
    instance.SetActive(true); // Activate the instance here if necessary
    
    NFTItem nftItem = instance.GetComponent<NFTItem>();
    if (nftItem != null) {
        nftItem.SetNFTData(data); // This is the correct method to use based on your provided code
    } else {
        Debug.LogError("NFTItem component not found on instantiated prefab.");
    }
}


// Inside NFTManager.cs
async Task FetchAndSetNFTMetadata(UnboundedUInt tokenId, NFTDisplay displayComponent)
{
    var metadataResult = await CandidApiManager.Instance.testnft.Icrc7Metadata(tokenId);
    if (metadataResult.Tag == MetadataResultTag.Ok && metadataResult.Value is Dictionary<string, Metadata> metadataDictionary)
    {
        NFTData nftData = NFTMetadataParser.Parse(metadataDictionary);

        // Instantiate prefab and set its NFTData
        var instance = Instantiate(nftPrefab, nftDisplayContainer);
        var nftItem = instance.GetComponent<NFTItem>();
        if (nftItem != null) {
            nftItem.SetNFTData(nftData); // Ensure this method correctly assigns data to the prefab
            instance.SetActive(true);
        }
        
        // Assuming you want to update the display as soon as the prefab is instantiated and data is set
        displayComponent.SetNFTData(nftData);
        RefreshUILayout();
    }
    else
    {
        Debug.LogError($"Failed to fetch metadata for NFT with Token ID: {tokenId}");
    }
}


   async Task<List<UnboundedUInt>> GetOwnedNFTs(Account account)
   {
       var nftListResult = await CandidApiManager.Instance.testnft.Icrc7TokensOf(account);
       return nftListResult.Tag == TokensOfResultTag.Ok ? nftListResult.AsOk() : new List<UnboundedUInt>();
   }

async Task FetchAndDisplayNFTMetadata(UnboundedUInt tokenId)
{
    var metadataResult = await CandidApiManager.Instance.testnft.Icrc7Metadata(tokenId);
    if (metadataResult.Tag == MetadataResultTag.Ok && metadataResult.Value is Dictionary<string, Metadata> metadataDictionary)
    {
        // Parse metadata and log the result
        NFTData nftData = NFTMetadataParser.Parse(metadataDictionary);
        string nftInfo = GetNFTDataInfo(nftData);
        Debug.Log($"Parsed NFTData for Token ID {tokenId}: {nftInfo}");

        InstantiateAndDisplayNFT(nftData); // Dynamically instantiate NFT display and update UI
    }
}

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

    private void InstantiateAndDisplayNFT(NFTData nftData)
    {
        if (nftDisplay == null)
        {
            Debug.LogError("NFTDisplay component not found.");
            return;
        }

        // Assuming nftDisplay is already correctly referenced
        nftDisplay.SetNFTData(nftData);
    }

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

}