using System;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;
using CanisterPK.testnft.Models;
using UnityEngine;

public static class NFTMetadataParser
{
    public static NFTData Parse(Dictionary<string, Metadata> metadataDictionary)
    {
        var nftData = new NFTData();
        if(metadataDictionary.TryGetValue("tokenId", out Metadata tokenIdMetadata))
    {
        nftData.TokenId = tokenIdMetadata.AsText();
        metadataDictionary.Remove("tokenId"); // Remove it if you prefer not to treat it as general metadata
    }

        foreach (var entry in metadataDictionary)
        {
            switch (entry.Key)
            {
                case "basic_stats":
                    ParseBasicStats(entry.Value.Value as Dictionary<string, Metadata>, nftData);
                    break;
                case "general":
                    ParseGeneralInfo(entry.Value.Value as Dictionary<string, Metadata>, nftData);
                    break;
                case "skills":
                    ParseSkills(entry.Value.Value as Dictionary<string, Metadata>, nftData);
                    break;
                // Skins logic removed
            }
        }
        return nftData;
    }

    private static void ParseBasicStats(Dictionary<string, Metadata> dict, NFTData nftData)
    {
        foreach (var item in dict)
        {
            int value = item.Value.Tag == MetadataTag.Nat ? (int)item.Value.AsNat() : (int)item.Value.AsInt();
            nftData.BasicStats.Add(new BasicStat { StatName = item.Key, StatValue = value });
        }
    }

    private static void ParseGeneralInfo(Dictionary<string, Metadata> dict, NFTData nftData)
    {
        var generalInfo = new GeneralInfo();
        foreach (var item in dict)
        {
            switch (item.Key)
            {
                case "unit_id":
                    generalInfo.UnitId = (int)item.Value.AsNat();
                    Debug.Log((int)item.Value.AsNat() );
                    break;
                case "class":
                    generalInfo.Class = item.Value.AsText();
                    break;
                case "rarity":
                    generalInfo.Rarity = (int)item.Value.AsNat();
                    break;
                case "faction":
                    generalInfo.Faction = item.Value.AsText();
                    break;
                case "name":
                    generalInfo.Name = item.Value.AsText();
                    break;
                case "description":
                    generalInfo.Description = item.Value.AsText();
                    break;
                case "icon":
                    generalInfo.Icon = (int)item.Value.AsNat();
                    break;
                // SkinsText or related to skins parsing not included
            }
        }
        nftData.General.Add(generalInfo);
    }

    private static void ParseSkills(Dictionary<string, Metadata> dict, NFTData nftData)
    {
        foreach (var item in dict)
        {
            nftData.Skills.Add(new Skill { SkillName = item.Key, SkillValue = (int)item.Value.AsInt() });
        }
    }

    // ParseSkins method or related logic removed
}
