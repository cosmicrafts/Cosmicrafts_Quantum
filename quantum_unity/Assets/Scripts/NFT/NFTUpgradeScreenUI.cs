using UnityEngine;
using TMPro;

public class NFTUpgradeScreenUI : MonoBehaviour
{
    public TMP_Text levelInfoText;
    public TMP_Text hpInfoText;
    public TMP_Text damageInfoText;
    public Animator upgradeAnimator; // Reference to an Animator component for animations

    // Method to activate the upgrade screen and display information
    public void ActivateUpgradeScreen(int currentLevel, int updatedLevel, int currentHP, int hpDiff, int currentDamage, int damageDiff)
    {
        gameObject.SetActive(true); // Activate the panel

        // Update text fields
        levelInfoText.text = $"Level: {currentLevel} -> {updatedLevel}";
        hpInfoText.text = $"HP: {currentHP} + {hpDiff}";
        damageInfoText.text = $"Damage: {currentDamage} + {damageDiff}";

        // Trigger animation
        upgradeAnimator.SetTrigger("UpgradeSuccess"); // Assuming there's a trigger set up in your Animator
    }
}
