using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System.Collections.Generic;
using CanisterPK.testnft.Models;
using EdjCase.ICP.Candid.Models;
using Candid;
using Newtonsoft.Json;
using System;


public class NFTManager : MonoBehaviour
{
   public TMP_Text nftListText;
   public GameObject nftPrefab;
   public Transform nftDisplayContainer;
   public NFTMetadataParser metadataParser;
   public NFTDisplay nftDisplay;

   async void Start()
   {
       nftPrefab.SetActive(false);
       await FetchOwnedNFTs();
   }

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
               await FetchAndDisplayNFTMetadata(tokenId);
           }
       }

       nftListText.text = $"Owned NFTs: {tokens.Count}";
       foreach (var tokenId in tokens)
       {
           var instance = Instantiate(nftPrefab, nftDisplayContainer);
           instance.SetActive(true);
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
    if (metadataResult.Tag == MetadataResultTag.Ok)
        {
            // Directly using Dictionary<string, Metadata> instead of MetadataArray
            if (metadataResult.Value is Dictionary<string, Metadata> metadataDictionary)
            {
                foreach (var entry in metadataDictionary)
                {
                string key = entry.Key;
                Metadata metadata = entry.Value;
                Debug.Log($"Key: {key}, Metadata Tag: {metadata.Tag}");
                // Process the metadata, handling nested structures
                ProcessMetadata(metadata);
                }
            }
        }
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