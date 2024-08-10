using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;
using Candid;
using Cosmicrafts.MainCanister.Models;
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
            }
        }

        async void Start()
        {
            await FetchOwnedNFTs();
        }

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

            var principal = Principal.FromText(userData.PrincipalId);
            var nftsResponse = await CandidApiManager.Instance.MainCanister.GetNFTs(principal);

            if (nftsResponse != null && nftsResponse.Count > 0)
            {
                foreach (var nftElement in nftsResponse)
                {
                    ProcessAndCategorizeNFT(nftElement.F1); // F1 contains the TokenMetadata
                }

                nftCollection.AllNFTDatas = AllNFTDatas;
                nftCollection.RefreshCollection();
            }
        }

    private void ProcessAndCategorizeNFT(TokenMetadata tokenMetadata)
    {
        Metadata metadata = tokenMetadata.Metadata;

        // Assign category correctly
        SerializedCategory category;
        if (metadata.Category.Tag == CategoryTag.Unit)
        {
            // Assuming Unit is an enum and not a class with a Tag property
            Unit unit = metadata.Category.AsUnit();
            category = new SerializedCategory(metadata.Category.Tag.ToString(), unit.ToString());
        }
        else
        {
            category = new SerializedCategory(metadata.Category.Tag.ToString());
        }

        NFTData nftData = new NFTData
        {
            TokenId = tokenMetadata.TokenId.ToString(),
            BasicStats = metadata.Basic.HasValue ? metadata.Basic.GetValueOrDefault().ConvertToBasicStats() : new List<BasicStat>(),
            General = new List<GeneralInfo> { metadata.General.ConvertToGeneralInfo(metadata.Category) },
            Skills = metadata.Skills.HasValue ? metadata.Skills.GetValueOrDefault().ConvertToSkills() : new List<Skill>(),
            Skins = metadata.Skins.HasValue ? metadata.Skins.GetValueOrDefault().ConvertToSkin() : new List<Skin>(),
            Category = category // Assign the parsed category
        };

        AllNFTDatas.Add(nftData.Clone());

        // Categorize and store in player data based on NFT type
        CategorizeNFTInPlayerData(nftData);
    }



        private void CategorizeNFTInPlayerData(NFTData nftData)
        {
            var category = nftData.Category.TagName;

            // implementation pending

            OnMetadataUpdated?.Invoke(nftData.TokenId);
        }

        private List<BasicStat> ConvertBasicMetadata(BasicMetadata basic)
        {
            return new List<BasicStat>
            {
                new BasicStat { StatName = "Damage", StatValue = (int)basic.Damage },
                new BasicStat { StatName = "Health", StatValue = (int)basic.Health },
                new BasicStat { StatName = "Level", StatValue = (int)basic.Level }
            };
        }

        private GeneralInfo ConvertGeneralMetadata(GeneralMetadata general, Category category)
        {
            int iconValue;
            if (!int.TryParse(general.Image, out iconValue))
            {
                Debug.LogWarning($"Failed to parse Icon value: {general.Image}. Defaulting to 0.");
                iconValue = 0; // Default value in case of parsing failure
            }

            return new GeneralInfo
            {
                UnitId = (int)general.Id,
                Class = general.Name,
                Rarity = general.Rarity.HasValue ? (int)general.Rarity.GetValueOrDefault() : 0,
                Faction = general.Faction.HasValue ? general.Faction.GetValueOrDefault().ToString() : null,
                Name = general.Name,
                Description = general.Description,
                Icon = iconValue,
            };
        }

        private List<Skill> ConvertSkillsMetadata(SkillMetadata skills)
        {
            var skillList = new List<Skill>();

            return skillList;
        }

        private List<Skin> ConvertSkinMetadata(SkinMetadata skin)
        {
            // Assuming you now need to derive or use default values for Skin properties
            return new List<Skin>
            {
                new Skin
                {
                    SkinId = (int)skin, // Using the enum value as the SkinId
                    SkinName = "Default Skin Name", // Placeholder name, replace as needed
                    SkinDescription = "Default Skin Description", // Placeholder description, replace as needed
                    SkinIcon = "default_icon.png", // Placeholder icon, replace as needed
                    SkinRarity = 0 // Default rarity, adjust as necessary
                }
            };
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

            return await CandidApiManager.Instance.MainCanister.Icrc7Transfer(transferArgs);
        }

        public async Task UpdateNFTMetadata(string tokenId)
        {
            Debug.Log($"Starting metadata update for Token ID: {tokenId}");
            UnboundedUInt tokenID = UnboundedUInt.FromBigInteger(BigInteger.Parse(tokenId));
            //await FetchAndSetNFTMetadata(tokenID); // Ensure metadata is fetched and UI event is triggered
            OnMetadataUpdated?.Invoke(tokenId);
            Debug.Log($"Finished metadata update for Token ID: {tokenId}");
        }
    }
}
