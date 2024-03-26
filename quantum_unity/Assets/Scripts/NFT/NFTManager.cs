using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using Candid;
using CanisterPK.testnft.Models;
using EdjCase.ICP.Candid.Models;

public class NFTManager : MonoBehaviour
{
    public static NFTManager Instance { get; private set; }
    public NFTCollection nftCollection;
    
    public List<NFTData> AllNFTDatas = null;
    
    public GameplaySettingsAsset  m_GameplaySettings;

    public delegate void MetadataUpdated(string tokenId);
    public static event MetadataUpdated OnMetadataUpdated;
    public delegate void NFTTransferred(string tokenId);
    public static event NFTTransferred OnNFTTransferred;
    
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
        await FetchOwnedNFTs();
    }

    
    // Fetch user's owned NFTs
    async Task FetchOwnedNFTs()
    {
        string userPrincipalId = GlobalGameData.Instance.GetUserData().WalletId;
        var account = new Account(Principal.FromText(userPrincipalId), null);
        var tokens = await GetOwnedNFTs(account);

        if (tokens != null && tokens.Count > 0)
        {
            foreach (var tokenId in tokens)
            {
               // Debug.Log("[NFTManager] Getting Token: "+ tokenId);
                await FetchAndSetNFTMetadata(tokenId);
            }

            nftCollection.AllNFTDatas = AllNFTDatas;
            nftCollection.RefreshCollection();
        }
    }
    
    // Fetch metadata for a specific NFT token and update UI
    async Task FetchAndSetNFTMetadata(UnboundedUInt tokenId)
    {
        var metadataResult = await CandidApiManager.Instance.testnft.Icrc7Metadata(tokenId);
        if (metadataResult.Tag == MetadataResultTag.Ok && metadataResult.Value is Dictionary<string, Metadata> metadataDictionary)
        {
            Debug.Log($"Successfully fetched metadata for Token ID: {tokenId}");
            NFTData nftData = NFTMetadataParser.Parse(metadataDictionary);
            nftData.TokenId = tokenId.ToString();

            // Check if NFTData with this tokenId already exists
            var existingData = AllNFTDatas.FirstOrDefault(nd => nd.TokenId == tokenId.ToString());
            if (existingData != null)
            {
                // Update existing NFTData
                var index = AllNFTDatas.IndexOf(existingData);
                AllNFTDatas[index] = nftData.Clone();
            }
            else
            {
                // Add new NFTData
                AllNFTDatas.Add(nftData.Clone());
            }

            OnMetadataUpdated?.Invoke(tokenId.ToString());
        }
    }

    // Fetch user's owned NFT tokens
    async Task<List<UnboundedUInt>> GetOwnedNFTs(Account account)
    {
        var nftListResult = await CandidApiManager.Instance.testnft.Icrc7TokensOf(account);
        return nftListResult.Tag == TokensOfResultTag.Ok ? nftListResult.AsOk() : new List<UnboundedUInt>();
    }
    
    public async Task<TransferReceipt> TransferNFT(List<UnboundedUInt> tokenIds, Principal recipientPrincipal) 
    {
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

    public async Task UpdateNFTMetadata(string tokenId)
    {
        Debug.Log($"Starting metadata update for Token ID: {tokenId}");
        UnboundedUInt tokenID = UnboundedUInt.FromBigInteger(BigInteger.Parse(tokenId));
        await FetchAndSetNFTMetadata(tokenID);
        OnMetadataUpdated?.Invoke(tokenId);
        Debug.Log($"Finished metadata update for Token ID: {tokenId}");
        // Trigger any UI refresh mechanisms here if necessary.
    }

    public NFTData GetNFTDataById(string tokenId)
    {
        // Search for the NFTData with the matching tokenId
        return AllNFTDatas.FirstOrDefault(nftData => nftData.TokenId == tokenId);
    }

    public void RemoveNFT(string tokenId)
    {
        AllNFTDatas.RemoveAll(nft => nft.TokenId == tokenId);
        OnNFTTransferred?.Invoke(tokenId);
    }

}
