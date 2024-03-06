using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic; 

public class NFTDisplay : MonoBehaviour {
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
    public Image iconImage; // Ensure you handle icon updates correctly with a method to map IDs to Sprites

    private NFTData nftData; // Add this line to declare the nftData field

    // Updated to take full NFTData and automatically refresh all UI components
    public void SetNFTData(NFTData nftData) {
        this.nftData = nftData;
        iconImage.sprite = GetIconSpriteById(nftData.General.FirstOrDefault()?.Icon ?? 0);
        DisplayNFT();
    }

    private void DisplayNFT() {
        if (nftData == null) {
            Debug.LogWarning("NFTData not assigned.");
            return;
        }

        GeneralInfo general = nftData.General.FirstOrDefault();
        if (general != null) {
            unitNameText.text = general.Name;
            descriptionText.text = general.Description;
            factionText.text = general.Faction;
            unitClassText.text = general.Class;
            rarityText.text = general.Rarity.ToString();
            skinsText.text = general.SkinsText;

            // Implement your logic to convert icon ID to Sprite and assign it
            iconImage.sprite = GetIconSpriteById(general.Icon);
        }

        levelText.text = GetValueFromStats(nftData.BasicStats, "level");
        healthText.text = GetValueFromStats(nftData.BasicStats, "health");
        damageText.text = GetValueFromStats(nftData.BasicStats, "damage");
        skillsText.text = string.Join(", ", nftData.Skills.Select(s => $"{s.SkillName}: {s.SkillValue}"));
        skinsText.text = string.Join("\n", nftData.Skins.Select(s => $"{s.SkinName} - {s.SkinDescription}"));
    }

    // Helper method to fetch stat value by name
    private string GetValueFromStats(List<BasicStat> stats, string statName) {
        var stat = stats.FirstOrDefault(s => s.StatName.ToLower() == statName.ToLower());
        return stat != null ? $"{stat.StatName}: {stat.StatValue}" : $"{statName}: Not Available";
    }

    private Sprite GetIconSpriteById(int iconId) {
    return iconImage.sprite;
}

}
