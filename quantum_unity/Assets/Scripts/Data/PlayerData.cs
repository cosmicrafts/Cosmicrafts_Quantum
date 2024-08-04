using System;
using System.Collections.Generic;
using CanisterPK.CanisterLogin.Models;
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
        public int CharacterNFTId = 0;
        public List<int> DeckNFTsId = new List<int>();
        public List<string> DeckNFTsKeyIds = new List<string>();
        public string Email { get; set; }
        public DateTime LastConnection { get; set; }
        public DateTime Registered { get; set; }
        public Config config = new Config();
        public string LastMapSelected = "0";
        public UnboundedUInt actualNumberRoom = 0;
        public bool IsLoggedIn { get; set; } = false;

        // Newly added properties to match the Player class
        public string Description { get; set; }
        public double Elo { get; set; }
        public List<FriendDetails> Friends { get; set; }
        public long RegistrationDate { get; set; }
        public event Action<int> OnAvatarIdChanged;
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
