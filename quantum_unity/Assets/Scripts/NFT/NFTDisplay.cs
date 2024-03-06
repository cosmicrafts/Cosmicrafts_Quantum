using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

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

    private NFTData nftData;

    void Start()
    {
        if (nftData != null) 
        {
            DisplayNFT();
        }
    }

    public void SetNFTData(NFTData nftData)
    {
        this.nftData = nftData;
        DisplayNFT(); // Update the UI as soon as new data is set
    }

    public void DisplayNFT()
    {
        if (nftData == null)
        {
            Debug.LogWarning("NFTData not assigned.");
            return;
        }

        // Populates UI with general information if available
        var generalInfo = nftData.General.FirstOrDefault();
        if (generalInfo != null)
        {
            unitNameText.text = generalInfo.Name;
            descriptionText.text = generalInfo.Description;
            factionText.text = generalInfo.Faction;
            unitClassText.text = generalInfo.Class;
            rarityText.text = generalInfo.Rarity.ToString();
            skinsText.text = generalInfo.SkinsText; // Update according to your data structure

            // Assuming method GetIconSpriteById exists and correctly returns a Sprite based on an ID
            iconImage.sprite = GetIconSpriteById(generalInfo.Icon);
        }
        else
        {
            Debug.LogWarning("General info is missing.");
        }

        // Populates UI with basic stats
        foreach (var stat in nftData.BasicStats)
        {
            switch (stat.StatName.ToLower())
            {
                case "level":
                    levelText.text = $"Level: {stat.StatValue}";
                    break;
                case "health":
                    healthText.text = $"Health: {stat.StatValue}";
                    break;
                case "damage":
                    damageText.text = $"Damage: {stat.StatValue}";
                    break;
                default:
                    Debug.LogWarning($"Unknown stat: {stat.StatName}");
                    break;
            }
        }

        // Populates skills and skins text
        skillsText.text = string.Join(", ", nftData.Skills.Select(s => $"{s.SkillName}: {s.SkillValue}"));
        skinsText.text = string.Join("\n", nftData.Skins.Select(s => $"{s.SkinName} - {s.SkinDescription}"));
    }

    private Sprite GetIconSpriteById(int iconId)
    {
        // Implement the method to fetch and return the appropriate sprite for an iconId
        // Placeholder implementation:
        Debug.LogWarning("GetIconSpriteById method is not implemented.");
        return null;
    }

}
