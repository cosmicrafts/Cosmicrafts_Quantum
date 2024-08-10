using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;
using Candid;
using Cosmicrafts.MainCanister.Models;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.Managers;
using System;
using Cosmicrafts.MainCanister;

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
            await GetPlayerDeckAsync();
        }

        public async Task FetchOwnedNFTs()
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



        public async Task<bool> GetPlayerDeckAsync()
        {
            if (GameDataManager.Instance == null)
            {
                Debug.LogError("[NFTManager] GameDataManager instance is null.");
                return false;
            }

            var userData = GameDataManager.Instance.playerData;
            if (userData == null)
            {
                Debug.LogError("Failed to load player data.");
                return false;
            }

            var principal = Principal.FromText(userData.PrincipalId);

            try
            {
                var deckResponse = await CandidApiManager.Instance.MainCanister.GetPlayerDeck(principal);

                // Check if the response has a value (which is a list of TokenId)
                if (deckResponse.HasValue)
                {
                    var deckList = deckResponse.GetValueOrDefault();

                    if (deckList.Count == 0)
                    {
                        Debug.LogWarning("Player deck is empty.");
                        userData.DeckNFTsKeyIds.Clear();  // Clear any existing deck in player data
                    }
                    else
                    {
                        Debug.Log("Player deck retrieved successfully.");

                        // Convert the list of TokenIds (UnboundedUInt) to strings and store in playerData.DeckNFTsKeyIds
                        userData.DeckNFTsKeyIds = deckList
                            .Select(tokenId => tokenId.ToString())
                            .ToList();
                    }
                }
                else
                {
                    Debug.LogWarning("Player deck is null.");
                    userData.DeckNFTsKeyIds.Clear();  // Clear any existing deck in player data
                }

                // Save the updated player data locally
                GameDataManager.Instance.SavePlayerData();

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error retrieving player deck: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> SaveDeckToBlockchain()
        {
            var playerData = GameDataManager.Instance.playerData;
            if (playerData == null)
            {
                Debug.LogError("[NFTManager] Player data is null.");
                return false;
            }

            try
            {
                // Convert the list of string IDs (DeckNFTsKeyIds) to UnboundedUInt
                var deckTokenIds = playerData.DeckNFTsKeyIds
                    .Select(key => UnboundedUInt.FromBigInteger(BigInteger.Parse(key)))
                    .ToList();

                // Create an instance of StoreCurrentDeckArg0 and add the deck data
                var storeDeckArg = new MainCanisterApiClient.StoreCurrentDeckArg0();
                storeDeckArg.AddRange(deckTokenIds);

                // Call the API to store the deck on the blockchain
                var isStored = await CandidApiManager.Instance.MainCanister.StoreCurrentDeck(storeDeckArg);

                if (isStored)
                {
                    Debug.Log("Deck saved to blockchain successfully.");
                }
                else
                {
                    Debug.LogWarning("Failed to save the deck to blockchain.");
                }

                return isStored;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error storing current deck: {ex.Message}");
                return false;
            }
        }


        public void OnSaveDeckButtonClicked()
        {
            // Call the async method but don't wait for it to finish
            SaveDeckToBlockchain().ContinueWith(task => 
            {
                if (task.Exception != null)
                {
                    Debug.LogError("Error occurred while saving deck to blockchain: " + task.Exception);
                }
            });
        }

        public async Task<List<NFTData>> GetChestsAsync()
{
    if (GameDataManager.Instance == null)
    {
        Debug.LogError("[NFTManager] GameDataManager instance is null.");
        return null;
    }

    var userData = GameDataManager.Instance.playerData;
    if (userData == null)
    {
        Debug.LogError("[NFTManager] Failed to load player data.");
        return null;
    }

    var principal = Principal.FromText(userData.PrincipalId);

    try
    {
        var chestsResponse = await CandidApiManager.Instance.MainCanister.GetChests(principal);

        if (chestsResponse == null || chestsResponse.Count == 0)
        {
            Debug.LogWarning("[NFTManager] No chests found for the player.");
            return new List<NFTData>();
        }

        var nftDataList = new List<NFTData>();

        foreach (var chestElement in chestsResponse)
        {
            var nftData = ScriptableObject.CreateInstance<NFTData>(); // Correct usage of CreateInstance
            nftData.PopulateFromMetadata(chestElement.F1); // Use PopulateFromMetadata method

            nftDataList.Add(nftData);
        }

        return nftDataList;
    }
    catch (Exception ex)
    {
        Debug.LogError($"[NFTManager] Error retrieving chests: {ex.Message}");
        return null;
    }
}



    }
}
