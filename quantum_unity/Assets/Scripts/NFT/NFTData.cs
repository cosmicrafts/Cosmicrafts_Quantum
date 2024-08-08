using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "NewNFTData", menuName = "NFT Data", order = 1)]
public class NFTData : ScriptableObject
{
    public List<BasicStat> BasicStats { get; set; } = new List<BasicStat>();
    public List<GeneralInfo> General { get; set; } = new List<GeneralInfo>();
    public List<Skill> Skills { get; set; } = new List<Skill>();
    public List<Skin> Skins { get; set; } = new List<Skin>();
    public string TokenId { get; set; }

    public NFTData Clone()
    {
        var clone = ScriptableObject.CreateInstance<NFTData>();
        clone.BasicStats = BasicStats.Select(stat => stat.Clone()).ToList();
        clone.General = General.Select(info => info.Clone()).ToList();
        clone.Skills = Skills.Select(skill => skill.Clone()).ToList();
        clone.Skins = Skins.Select(skin => skin.Clone()).ToList();
        clone.TokenId = TokenId;

        return clone;
    }
}

public class BasicStat
{
    public string StatName { get; set; }
    public int StatValue { get; set; }

    public BasicStat Clone() => new BasicStat { StatName = this.StatName, StatValue = this.StatValue };
}

public class GeneralInfo
{
    public int UnitId { get; set; }
    public string Class { get; set; }
    public int Rarity { get; set; }
    public string Faction { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Icon { get; set; }
    public string SkinsText { get; set; }

    public GeneralInfo Clone()
    {
        return new GeneralInfo
        {
            UnitId = this.UnitId,
            Class = this.Class,
            Rarity = this.Rarity,
            Faction = this.Faction,
            Name = this.Name,
            Description = this.Description,
            Icon = this.Icon,
            SkinsText = this.SkinsText
        };
    }
}

public class Skill
{
    public string SkillName { get; set; }
    public int SkillValue { get; set; }

    public Skill Clone()
    {
        return new Skill { SkillName = this.SkillName, SkillValue = this.SkillValue };
    }
}

public class Skin
{
    public int SkinId { get; set; }
    public string SkinName { get; set; }
    public string SkinDescription { get; set; }
    public string SkinIcon { get; set; }
    public int SkinRarity { get; set; }

    public Skin Clone()
    {
        return new Skin 
        { 
            SkinId = this.SkinId, 
            SkinName = this.SkinName, 
            SkinDescription = this.SkinDescription, 
            SkinIcon = this.SkinIcon, 
            SkinRarity = this.SkinRarity 
        };
    }
}
