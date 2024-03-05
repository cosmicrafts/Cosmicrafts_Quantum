using UnityEngine;
using System;
using TMPro;
using Candid;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using CanisterPK.testnft.Models;
using EdjCase.ICP.Candid.Models;
using Newtonsoft.Json.Linq;

public class NFTMetadataParser : MonoBehaviour
{
    public NFTDisplay nftDisplay;

    public void ParseAndPopulateUnit(string rawJson, UnboundedUInt tokenId)
    {
        try
        {
            JObject jObject = JObject.Parse(rawJson);
            JArray data = (JArray)jObject["Ok"];

            // Create a new UnitSO instance here or get it from somewhere
            UnitSO newUnitSO = ScriptableObject.CreateInstance<UnitSO>();

            foreach (JArray item in data)
            {
                string key = (string)item[0];
                JObject value = (JObject)item[1];

                switch (key)
                {
                    case "basic_stats":
                        ParseBasicStats(value, ref newUnitSO);
                        break;
                    case "general":
                        ParseGeneral(value, ref newUnitSO);
                        break;
                    case "skills":
                        ParseSkills(value, ref newUnitSO);
                        break;
                    case "skins":
                        ParseSkins(value, ref newUnitSO);
                        break;
                }
            }

            nftDisplay.DisplayNFT(newUnitSO);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error parsing NFT metadata: {ex.Message}");
        }
    }

    void ParseBasicStats(JObject basicStats, UnitSO unitSO)
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

    void ParseGeneral(JObject general, UnitSO unitSO)
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

    void ParseSkills(JObject skills,UnitSO unitSO)
    {
        JArray skillsArray = (JArray)skills["MetadataArray"];
        foreach (JArray skill in skillsArray)
        {
            string skillName = (string)skill[0];
            JObject skillValue = (JObject)skill[1];
            unitSO.skills.Add(skillName, (int)skillValue["Nat"]);
        }
    }

    void ParseSkins(JObject skins, ref UnitSO unitSO)
    {
        JArray skinsArray = (JArray)skins["MetadataArray"];
        unitSO.skins.Clear(); // Clear existing skins to avoid duplications

        foreach (JArray skin in skinsArray)
        {
            // Extract and store each skin's data as a raw JSON string
            string skinDataAsString = skin.ToString();
            unitSO.skins.Add(skinDataAsString);
        }
    }


    private void ParseBasicStats(JObject basicStats, ref UnitSO unitSO) { /* Implementation */ }
    private void ParseGeneral(JObject general, ref UnitSO unitSO) { /* Implementation */ }
    private void ParseSkills(JObject skills, ref UnitSO unitSO) { /* Implementation */ }
}
