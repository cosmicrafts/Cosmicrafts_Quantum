using System;
using System.Collections.Generic;

namespace Cosmicrafts.Data
{
    [Serializable]
    public class PlayerData
    {
        public string PrincipalId = "pid";
        public string Username = "Anon";
        public int Level { get; set; }
        public int AvatarID = 1;
        public int CharacterNFTId = 0;
        public List<int> DeckNFTsId = new List<int>();
        public List<string> DeckNFTsKeyIds = new List<string>();
        public string Email { get; set; }
        public DateTime LastConnection { get; set; }
        public DateTime Registered { get; set; }
        public Config config = new Config();
        public string LastMapSelected = "0";
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
