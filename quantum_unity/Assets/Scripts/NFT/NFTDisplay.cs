using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

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
    public Image iconImage; // Handle this based on your project needs

    public void DisplayNFT(string unitName, string description, string unitClass, int rarity, string faction, int level, int health, int damage, Dictionary<string, int> skills, List<string> skins)
    {
        UpdateText(unitNameText, unitName);
        UpdateText(descriptionText, description);
        UpdateText(factionText, faction);
        UpdateText(unitClassText, unitClass);
        UpdateText(rarityText, rarity.ToString());
        UpdateText(levelText, level.ToString());
        UpdateText(healthText, health.ToString());
        UpdateText(damageText, damage.ToString());

        // Handle skills display
        if (skills != null && skills.Count > 0)
        {
            skillsText.text = string.Join(", ", skills.Select(s => $"{s.Key}: {s.Value}"));
        }
        else
        {
            skillsText.gameObject.SetActive(false); // Hide if no skills
        }

        // Handle skins display
        if (skins != null && skins.Count > 0)
        {
            skinsText.text = string.Join("\n", skins);
        }
        else
        {
            skinsText.gameObject.SetActive(false); // Hide if no skins
        }

        // Handle icon display based on your project's needs
        // iconImage.sprite = GetIconSprite(...);
    }

    void UpdateText(TMP_Text textComponent, string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            textComponent.gameObject.SetActive(false); // Hide text if data is missing
        }
        else
        {
            textComponent.gameObject.SetActive(true);
            textComponent.text = text;
        }
    }
}
