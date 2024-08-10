using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cosmicrafts.MainCanister.Models;

[CreateAssetMenu(fileName = "NewNFTData", menuName = "NFT Data", order = 1)]
public class NFTData : ScriptableObject
{
    [SerializeField]
    private SerializedCategory category;
    public SerializedCategory Category 
    { 
        get => category;
        set => category = value;
    }

    [SerializeField]
    private List<BasicStat> basicStats = new List<BasicStat>();
    public List<BasicStat> BasicStats 
    { 
        get => basicStats;
        set => basicStats = value;
    }

    [SerializeField]
    private List<GeneralInfo> general = new List<GeneralInfo>();
    public List<GeneralInfo> General 
    { 
        get => general;
        set => general = value;
    }

    [SerializeField]
    private List<Skill> skills = new List<Skill>();
    public List<Skill> Skills 
    { 
        get => skills;
        set => skills = value;
    }

    [SerializeField]
    private List<Skin> skins = new List<Skin>();
    public List<Skin> Skins 
    { 
        get => skins;
        set => skins = value;
    }

    [SerializeField]
    private string tokenId;
    public string TokenId 
    { 
        get => tokenId;
        set => tokenId = value;
    }

    [SerializeField]
    private string owner;
    public string Owner 
    { 
        get => owner;
        set => owner = value;
    }

    // Method to populate the NFTData from the metadata
    public void PopulateFromMetadata(TokenMetadata tokenMetadata)
    {
        this.TokenId = tokenMetadata.TokenId.ToString();
        this.Owner = tokenMetadata.Owner.Owner.ToString();

        // Handle Category Parsing from the root Metadata
        var category = tokenMetadata.Metadata.Category;
        if (category.Tag == CategoryTag.Unit)
        {
            // Handle Unit Category
            var unit = category.AsUnit();
            this.Category = new SerializedCategory(category.Tag.ToString(), unit.ToString());
        }
        else
        {
            // Handle other categories
            this.Category = new SerializedCategory(category.Tag.ToString());
        }

        this.BasicStats = tokenMetadata.Metadata.Basic.HasValue 
            ? tokenMetadata.Metadata.Basic.GetValueOrDefault().ConvertToBasicStats() 
            : new List<BasicStat>();

        this.General = new List<GeneralInfo> 
        {
            tokenMetadata.Metadata.General.ConvertToGeneralInfo(category)
        };

        this.Skills = tokenMetadata.Metadata.Skills.HasValue 
            ? tokenMetadata.Metadata.Skills.GetValueOrDefault().ConvertToSkills() 
            : new List<Skill>();

        this.Skins = tokenMetadata.Metadata.Skins.HasValue 
            ? tokenMetadata.Metadata.Skins.GetValueOrDefault().ConvertToSkin()
            : new List<Skin>();
    }


    public NFTData Clone()
    {
        var clone = CreateInstance<NFTData>();
        clone.Category = this.Category;
        clone.BasicStats = BasicStats.Select(stat => stat.Clone()).ToList();
        clone.General = General.Select(info => info.Clone()).ToList();
        clone.Skills = Skills.Select(skill => skill.Clone()).ToList();
        clone.Skins = Skins.Select(skin => skin.Clone()).ToList();
        clone.TokenId = TokenId;
        clone.Owner = Owner;

        return clone;
    }
}

[System.Serializable]
public class SerializedCategory
{
    [SerializeField]
    private string tagName;
    public string TagName => tagName;

    [SerializeField]
    private string value;
    public string Value => value;

    // Constructor for non-Unit categories
    public SerializedCategory(string tagName)
    {
        this.tagName = tagName;
        this.value = "None";
    }

    // Constructor for Unit categories
    public SerializedCategory(string tagName, string unitType)
    {
        this.tagName = tagName;
        this.value = unitType;
    }
}

[System.Serializable]
public class BasicStat
{
    [SerializeField]
    private string statName;
    public string StatName 
    { 
        get => statName; 
        set => statName = value; 
    }

    [SerializeField]
    private int statValue;
    public int StatValue 
    { 
        get => statValue; 
        set => statValue = value; 
    }

    public BasicStat Clone() => new BasicStat { StatName = this.StatName, StatValue = this.StatValue };
}

[System.Serializable]
public class GeneralInfo
{
    [SerializeField]
    private int unitId;
    public int UnitId 
    { 
        get => unitId; 
        set => unitId = value; 
    }

    [SerializeField]
    private string className;
    public string Class 
    { 
        get => className; 
        set => className = value; 
    }

    [SerializeField]
    private int rarity;
    public int Rarity 
    { 
        get => rarity; 
        set => rarity = value; 
    }

    [SerializeField]
    private string faction;
    public string Faction 
    { 
        get => faction; 
        set => faction = value; 
    }

    [SerializeField]
    private string name;
    public string Name 
    { 
        get => name; 
        set => name = value; 
    }

    [SerializeField]
    private string description;
    public string Description 
    { 
        get => description; 
        set => description = value; 
    }

    [SerializeField]
    private int icon;
    public int Icon 
    { 
        get => icon; 
        set => icon = value; 
    }

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
        };
    }
}


[System.Serializable]
public class Skill
{
    [SerializeField]
    private string skillName;
    public string SkillName 
    { 
        get => skillName; 
        set => skillName = value; 
    }

    [SerializeField]
    private int skillValue;
    public int SkillValue 
    { 
        get => skillValue; 
        set => skillValue = value; 
    }

    public Skill Clone()
    {
        return new Skill { SkillName = this.SkillName, SkillValue = this.SkillValue };
    }
}

[System.Serializable]
public class Skin
{
    [SerializeField]
    private int skinId;
    public int SkinId 
    { 
        get => skinId; 
        set => skinId = value; 
    }

    [SerializeField]
    private string skinName;
    public string SkinName 
    { 
        get => skinName; 
        set => skinName = value; 
    }

    [SerializeField]
    private string skinDescription;
    public string SkinDescription 
    { 
        get => skinDescription; 
        set => skinDescription = value; 
    }

    [SerializeField]
    private string skinIcon;
    public string SkinIcon 
    { 
        get => skinIcon; 
        set => skinIcon = value; 
    }

    [SerializeField]
    private int skinRarity;
    public int SkinRarity 
    { 
        get => skinRarity; 
        set => skinRarity = value; 
    }

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
