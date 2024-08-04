using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;
using Candid;
using CanisterPK.testnft.Models;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.Managers;

namespace Cosmicrafts.Data
{
    public class NFTManager : MonoBehaviour
    {
        public static NFTManager Instance { get; private set; }
        public NFTCollection nftCollection;

        public List<NFTData> AllNFTDatas = new List<NFTData>();

        public GameplaySettingsAsset m_GameplaySettings;

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
                DontDestroyOnLoad(gameObject); // Ensure the manager persists across scenes
            }
        }

        async void Start()
        {
            await FetchOwnedNFTs();
        }

        // Fetch user's owned NFTs
        async Task FetchOwnedNFTs()
        {
            if (GameDataManager.Instance == null)
            {
                Debug.LogError("[NFTManager] GameDataManager instance is null.");
                return;
            }

            var userData = GameDataManager.Instance.playerData;
            if (userData == null)
            {
                Debug.LogError("Failed to load player data.");
                return;
            }

            var account = new Account(Principal.FromText(userData.PrincipalId), null);
            var tokens = await GetOwnedNFTs(account);

            if (tokens != null && tokens.Count > 0)
            {
                foreach (var tokenId in tokens)
                {
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

                var existingData = AllNFTDatas.FirstOrDefault(nd => nd.TokenId == tokenId.ToString());
                if (existingData != null)
                {
                    var index = AllNFTDatas.IndexOf(existingData);
                    AllNFTDatas[index] = nftData.Clone();
                }
                else
                {
                    AllNFTDatas.Add(nftData.Clone());
                }

                OnMetadataUpdated?.Invoke(tokenId.ToString());
            }
            nftCollection.AllNFTDatas = AllNFTDatas;
            nftCollection.RefreshCollection();
        }

        // Fetch user's owned NFT tokens
        async Task<List<UnboundedUInt>> GetOwnedNFTs(Account account)
        {
            var nftListResult = await CandidApiManager.Instance.testnft.Icrc7TokensOf(account);
            return nftListResult.Tag == TokensOfResultTag.Ok ? nftListResult.AsOk() : new List<UnboundedUInt>();
        }

        public async Task<TransferReceipt> TransferNFT(List<UnboundedUInt> tokenIds, Principal recipientPrincipal)
        {
            if (GameDataManager.Instance == null)
            {
                Debug.LogError("[NFTManager] GameDataManager instance is null.");
                return null;
            }

            var userData = GameDataManager.Instance.playerData;
            if (userData == null)
            {
                Debug.LogError("Failed to load player data.");
                return null;
            }

            var senderAccount = new Account(Principal.FromText(userData.PrincipalId), new OptionalValue<List<byte>>());
            var recipientAccount = new Account(recipientPrincipal, new OptionalValue<List<byte>>());

            var transferArgs = new TransferArgs
            {
                CreatedAtTime = new OptionalValue<ulong>(),
                From = new OptionalValue<Account>(senderAccount),
                IsAtomic = new OptionalValue<bool>(true),
                Memo = new OptionalValue<List<byte>>(System.Text.Encoding.UTF8.GetBytes("Transfer Memo").ToList()),
                SpenderSubaccount = new OptionalValue<List<byte>>(),
                To = recipientAccount,
                TokenIds = tokenIds
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
        }

        public NFTData GetNFTDataById(string tokenId)
        {
            return AllNFTDatas.FirstOrDefault(nftData => nftData.TokenId == tokenId);
        }

        public void RemoveNFT(string tokenId)
        {
            AllNFTDatas.RemoveAll(nft => nft.TokenId == tokenId);
            OnNFTTransferred?.Invoke(tokenId);
        }
    }
}
