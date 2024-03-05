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

    public void DisplayNFT(UnitSO unitSO)
    {
        UpdateText(unitNameText, unitSO.unitName);
        UpdateText(descriptionText, unitSO.description);
        UpdateText(factionText, unitSO.faction);
        UpdateText(unitClassText, unitSO.unitClass);
        UpdateText(rarityText, unitSO.rarity.ToString());
        UpdateText(levelText, unitSO.level.ToString());
        UpdateText(healthText, unitSO.health.ToString());
        UpdateText(damageText, unitSO.damage.ToString());

        // Handle skills display
        if (unitSO.skills != null && unitSO.skills.Count > 0)
        {
            skillsText.text = string.Join(", ", unitSO.skills.Select(s => $"{s.Key}: {s.Value}"));
        }
        else
        {
            skillsText.gameObject.SetActive(false); // Hide if no skills
        }
        // Handle skins display
         if (unitSO.skins != null && unitSO.skins.Count > 0)
        {
            skinsText.text = string.Join("\n", unitSO.skins);
        }
        else
        {
            skinsText.gameObject.SetActive(false); // Hide if no skins
        }
        // Handle icon display
        // iconImage.sprite = GetIconSprite(unitSO.icon); // Implement this method based on your project's needs.

        

    }

    void UpdateText(TMP_Text textComponent, string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            textComponent.gameObject.SetActive(false); // Hide text if data is missing
        }
        else
        {
            textComponent.text = text;
        }
    }
}
