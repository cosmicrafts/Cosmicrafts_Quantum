using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UnitSO", menuName = "NFT Collection/Unit")]
public class UnitSO : ScriptableObject
{
    public string nftID;
    public string unitName;
    public string description;
    public string faction;
    public string unitClass;
    public int rarity;
    public int icon;

    // Stats
    public int level;
    public int health;
    public int damage;

    // Skills
    public Dictionary<string, int> skills = new Dictionary<string, int>();

    // Skins
    [System.Serializable]
    public struct Skin
    {
        public int skinId;
        public string skinName;
        public string skinDescription;
        public string skinIcon;
        public int skinRarity;
    }
    public List<string> skins = new List<string>();
}
