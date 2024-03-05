using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Linq; // Use Newtonsoft.Json for parsing complex JSON

public class NFTMetadataParser : MonoBehaviour
{
    public NFTUnit unitSO;

    public void ParseAndPopulateUnit(string rawJson)
    {
        JObject jObject = JObject.Parse(rawJson);
        JArray data = (JArray)jObject["Ok"];

        foreach (JArray item in data)
        {
            string key = (string)item[0];
            JObject value = (JObject)item[1];

            switch (key)
            {
                case "basic_stats":
                    ParseBasicStats(value);
                    break;
                case "general":
                    ParseGeneral(value);
                    break;
                case "skills":
                    ParseSkills(value);
                    break;
                case "skins":
                    ParseSkins(value);
                    break;
            }
        }
    }

    void ParseBasicStats(JObject basicStats)
    {
        JArray statsArray = (JArray)basicStats["MetadataArray"];
        foreach (JArray stat in statsArray)
        {
            string statName = (string)stat[0];
            JObject statValue = (JObject)stat[1];

            switch (statName)
            {
                case "level":
                    unitSO.level = (int)statValue["Nat"];
                    break;
                case "health":
                    unitSO.health = (int)statValue["Int"];
                    break;
                case "damage":
                    unitSO.damage = (int)statValue["Int"];
                    break;
            }
        }
    }

    void ParseGeneral(JObject general)
    {
        JArray generalArray = (JArray)general["MetadataArray"];
        foreach (JArray item in generalArray)
        {
            string key = (string)item[0];
            JObject value = (JObject)item[1];

            switch (key)
            {
                case "name":
                    unitSO.unitName = (string)value["Text"];
                    break;
                case "description":
                    unitSO.description = (string)value["Text"];
                    break;
                case "class":
                    unitSO.unitClass = (string)value["Text"];
                    break;
                case "rarity":
                    unitSO.rarity = (int)value["Nat"];
                    break;
                case "faction":
                    unitSO.faction = (string)value["Text"];
                    break;
                // Add other fields as needed
            }
        }
    }

    void ParseSkills(JObject skills)
    {
        JArray skillsArray = (JArray)skills["MetadataArray"];
        foreach (JArray skill in skillsArray)
        {
            string skillName = (string)skill[0];
            JObject skillValue = (JObject)skill[1];
            unitSO.skills.Add(skillName, (int)skillValue["Nat"]);
        }
    }

    void ParseSkins(JObject skins)
    {
        JArray skinsArray = (JArray)skins["MetadataArray"];
        foreach (JArray skin in skinsArray)
        {
            JArray skinDetails = (JArray)skin[1]["MetadataArray"];
            NFTUnit.Skin newSkin = new NFTUnit.Skin();
            foreach (JArray detail in skinDetails)
            {
                string key = (string)detail[0];
                JObject value = (JObject)detail[1];
                switch (key)
                {
                    case "skin_id":
                        newSkin.skinId = (int)value["Nat"];
                        break;
                    case "skin_name":
                        newSkin.skinName = (string)value["Text"];
                        break;
                    case "skin_description":
                        newSkin.skinDescription = (string)value["Text"];
                        break;
                    case "skin_icon":
                        newSkin.skinIcon = (string)value["Text"];
                        break;
                    case "skin_rarity":
                        newSkin.skinRarity = (int)value["Nat"];
                        break;
                }
            }
            unitSO.skins.Add(newSkin);
        }
    }
}
