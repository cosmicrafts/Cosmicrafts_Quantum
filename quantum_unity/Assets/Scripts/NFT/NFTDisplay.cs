using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;

public class NFTDisplay : MonoBehaviour
{
    public TMP_Text unitNameText;
    public TMP_Text descriptionText;
    public TMP_Text factionText;
    public TMP_Text unitClassText;
    public TMP_Text rarityText;
    public TMP_Text levelText;
    public TMP_Text healthText;
    public TMP_Text damageText;
    public TMP_Text skillsText;
    public TMP_Text skinsText;
    public Image iconImage;
    public TMP_Text tokenIdText;
    private string tokenId;
    private NFTData nftData;

    public string TokenId => tokenId;

    public void SetNFTData(NFTData nftData)
    {
        this.nftData = nftData;
        tokenId = nftData.TokenId;
        iconImage.sprite = GetIconSpriteById(nftData.General.FirstOrDefault()?.Icon ?? 0);
        DisplayNFT();
        tokenIdText.text = "Token ID: " + tokenId;
    }

    private void DisplayNFT()
    {
        if (nftData == null)
        {
            Debug.LogWarning("NFTData not assigned.");
            return;
        }

        var general = nftData.General.FirstOrDefault();
        if (general != null)
        {
            unitNameText.text = general.Name;
            descriptionText.text = general.Description;
            factionText.text = general.Faction;
            unitClassText.text = general.Class;
            rarityText.text = general.Rarity.ToString();
            skinsText.text = general.SkinsText;
            iconImage.sprite = GetIconSpriteById(general.Icon);
        }

        levelText.text = GetValueFromStats("level");
        healthText.text = GetValueFromStats("health");
        damageText.text = GetValueFromStats("damage");
        skillsText.text = string.Join(", ", nftData.Skills.Select(s => $"{s.SkillName}: {s.SkillValue}"));
        skinsText.text = string.Join("\n", nftData.Skins.Select(s => $"{s.SkinName} - {s.SkinDescription}"));
    }

    private string GetValueFromStats(string statName)
    {
        var stat = nftData.BasicStats.FirstOrDefault(s => s.StatName.ToLower() == statName.ToLower());
        return stat != null ? $"{stat.StatName}: {stat.StatValue}" : $"{statName}: Not Available";
    }

    private Sprite GetIconSpriteById(int iconId)
    {
        // Your logic to convert iconId to Sprite
        return iconImage.sprite;
    }
}
