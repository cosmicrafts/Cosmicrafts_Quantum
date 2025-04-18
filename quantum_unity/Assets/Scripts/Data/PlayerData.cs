using System;
using System.Collections.Generic;
using Cosmicrafts.MainCanister.Models;
using EdjCase.ICP.Candid.Models;

namespace Cosmicrafts.Data
{
    [Serializable]
    public class PlayerData
    {
        public string PrincipalId = "pid";
        public string Username = "Anon";
        public int Level { get; set; }
                public int AvatarID
        {
            get => avatarID;
            set
            {
                if (avatarID != value)
                {
                    avatarID = value;
                    OnAvatarIdChanged?.Invoke(value);
                }
            }
        }
        private int avatarID = 1;
        public string CharacterNFTId { get; set; }
        public List<string> DeckNFTsKeyIds = new List<string>();
        public string Email { get; set; }
        public DateTime LastConnection { get; set; }
        public DateTime Registered { get; set; }
        public Config config = new Config();
        public string LastMapSelected = "0";
        public UnboundedUInt actualNumberRoom = 0;
        public bool IsLoggedIn { get; set; } = false;

        public string Description { get; set; }
        public double Elo { get; set; }
        public List<FriendDetails> Friends { get; set; }
        public long RegistrationDate { get; set; }

        // New properties for storing categorized NFTs
        public List<NFTData> Characters { get; set; } = new List<NFTData>();
        public List<NFTData> Avatars { get; set; } = new List<NFTData>();
        public List<NFTData> Chests { get; set; } = new List<NFTData>();
        public List<NFTData> Trophies { get; set; } = new List<NFTData>();
        public List<NFTData> Units { get; set; } = new List<NFTData>();

        public event Action<int> OnAvatarIdChanged;

        // In-memory storage for saved keys
        private Dictionary<string, List<string>> savedKeys = new Dictionary<string, List<string>>();


        // Method to check if a key exists
        public bool HasKey(string key)
        {
            return savedKeys.ContainsKey(key);
        }

        // Method to get the value associated with a key
        public List<string> GetKeyList(string key)
        {
            if (savedKeys.TryGetValue(key, out var list))
            {
                return list;
            }
            return new List<string>();
        }

        // Method to set the value for a key
        public void SetKeyList(string key, List<string> value)
        {
            if (savedKeys.ContainsKey(key))
            {
                savedKeys[key] = value;
            }
            else
            {
                savedKeys.Add(key, value);
            }
        }
    }

        [Serializable]
    public class Config
    {
        public int Language = 0;
        public TypeMatch CurrentMatch = TypeMatch.multi;
        public int ModeSelected = 9;
        
    }

    public enum TypeMatch { bots, multi }
}
