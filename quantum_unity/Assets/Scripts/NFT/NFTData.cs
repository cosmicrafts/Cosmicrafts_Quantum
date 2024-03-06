using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNFTData", menuName = "NFT Data", order = 1)]
public class NFTData : ScriptableObject
{
    public List<BasicStat> BasicStats { get; set; } = new List<BasicStat>();
    public List<GeneralInfo> General { get; set; } = new List<GeneralInfo>();
    public List<Skill> Skills { get; set; } = new List<Skill>();
    public List<Skin> Skins { get; set; } = new List<Skin>();
}

public class BasicStat
{
    public string StatName { get; set; }
    public int StatValue { get; set; }
}

public class GeneralInfo
{
    public string Class { get; set; }
    public int Rarity { get; set; }
    public string Faction { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Icon { get; set; }
    public string SkinsText { get; set; }
}

public class Skill
{
    public string SkillName { get; set; }
    public int SkillValue { get; set; }
}

public class Skin
{
    public int SkinId { get; set; }
    public string SkinName { get; set; }
    public string SkinDescription { get; set; }
    public string SkinIcon { get; set; }
    public int SkinRarity { get; set; }
}
