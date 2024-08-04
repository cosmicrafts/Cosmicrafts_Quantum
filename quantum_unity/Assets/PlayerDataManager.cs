using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Cosmicrafts.Data
{
    public class PlayerDataManager : MonoBehaviour
    {
        public static PlayerDataManager Instance { get; private set; }
        public PlayerData playerData;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Persist this GameObject across scenes

                // Initialize playerData if not set in the inspector
                if (playerData == null)
                {
                    playerData = new PlayerData
                    {
                        PrincipalId = "pid",
                        Username = "Anon",
                        Level = 1,
                        AvatarID = 1,
                        CharacterNFTId = 0,
                        DeckNFTsId = new List<int>(),
                        DeckNFTsKeyIds = new List<string>(),
                        Email = "",
                        LastConnection = DateTime.Now,
                        Registered = DateTime.Now,
                        config = new Config(),
                        LastMapSelected = "0"
                    };
                }
            }
            else
            {
                Destroy(gameObject); // Destroy duplicates
            }
        }

        // Method to update player data
        public void UpdatePlayerData(string username, int level, int avatarID)
        {
            playerData.Username = username;
            playerData.Level = level;
            playerData.AvatarID = avatarID;
            playerData.LastConnection = DateTime.Now;
        }

        // Method to add NFT IDs to the player's deck
        public void AddNFTToDeck(int nftID, string nftKeyId)
        {
            if (!playerData.DeckNFTsId.Contains(nftID))
            {
                playerData.DeckNFTsId.Add(nftID);
                playerData.DeckNFTsKeyIds.Add(nftKeyId);
            }
        }

        // Method to clear the player's deck
        public void ClearDeck()
        {
            playerData.DeckNFTsId.Clear();
            playerData.DeckNFTsKeyIds.Clear();
        }

        // Method to set the player's current match type
        public void SetCurrentMatchType(TypeMatch matchType)
        {
            playerData.config.CurrentMatch = matchType;
        }

        // Method to set the player's selected map
        public void SetLastMapSelected(string mapID)
        {
            playerData.LastMapSelected = mapID;
        }

        // Method to retrieve player's information
        public PlayerData GetPlayerData()
        {
            return playerData;
        }
    }
}