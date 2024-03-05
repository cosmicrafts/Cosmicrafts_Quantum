using UnityEngine;
using System;
using TMPro;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;

public class NFTMetadataParser : MonoBehaviour
{
    public NFTDisplay nftDisplay;

    public void ParseAndDisplayNFT(string rawJson, UnboundedUInt tokenId)
    {
        try
        {
            if (nftDisplay == null)
            {
                Debug.LogError("NFTDisplay reference is null.");
                return;
            }

            JObject jObject = JObject.Parse(rawJson);

            // Initialize with default values in case of missing data
            string unitName = "";
            string description = "";
            string unitClass = "";
            int rarity = 0;
            string faction = "";
            int level = 0;
            int health = 0;
            int damage = 0;
            Dictionary<string, int> skills = new Dictionary<string, int>();
            List<string> skins = new List<string>();

            // Directly parse top-level sections
            ParseBasicStats(jObject["basic_stats"] as JObject, ref level, ref health, ref damage);
            ParseGeneral(jObject["general"] as JObject, ref unitName, ref description, ref unitClass, ref rarity, ref faction);
            ParseSkills(jObject["skills"] as JObject, ref skills);
            ParseSkins(jObject["skins"] as JObject, ref skins);

            nftDisplay.DisplayNFT(unitName, description, unitClass, rarity, faction, level, health, damage, skills, skins);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error parsing NFT metadata: {ex.Message}");
        }
    }

    void ParseBasicStats(JObject basicStats, ref int level, ref int health, ref int damage)
    {
        if (basicStats != null && basicStats["Value"] is JObject value)
        {
            level = (int)(value["level"]?["Nat"] ?? 0);
            health = (int)(value["health"]?["Int"] ?? 0);
            damage = (int)(value["damage"]?["Int"] ?? 0);
        }
    }

    void ParseGeneral(JObject general, ref string unitName, ref string description, ref string unitClass, ref int rarity, ref string faction)
    {
        if (general != null && general["Value"] is JObject value)
        {
            unitName = value["name"]?["Text"]?.ToString() ?? "";
            description = value["description"]?["Text"]?.ToString() ?? "";
            unitClass = value["class"]?["Value"]?.ToString() ?? "";
            rarity = (int)(value["rarity"]?["Nat"] ?? 0);
            faction = value["faction"]?["Value"]?.ToString() ?? "";
        }
    }

    void ParseSkills(JObject skills, ref Dictionary<string, int> skillsDict)
    {
        if (skills != null && skills["Value"] is JObject value)
        {
            // Note: This assumes skills are directly nested within "Value"
            foreach (var skillProperty in value.Properties())
            {
                string skillName = skillProperty.Name;
                int skillValue = (int)(skillProperty.Value?["Nat"] ?? 0);
                skillsDict[skillName] = skillValue;
            }
        }
    }

    void ParseSkins(JObject skins, ref List<string> skinsList)
    {
        skinsList.Clear(); // Clear existing skins 

        // I'm assuming you still need logic to parse skins data.
        // If the skin structure has changed, please provide the updated format. 
    }
}
