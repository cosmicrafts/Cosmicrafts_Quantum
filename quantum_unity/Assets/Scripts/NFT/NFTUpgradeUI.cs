using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EdjCase.ICP.Candid.Models;
using System.Numerics;
using CanisterPK.CanisterLogin;
using Candid;
using System.Linq;
using System;

public class NFTUpgradeUI : MonoBehaviour
{
    public Button upgradeButton;
    public TMP_Text notificationText;
    public NFTCard nftCard;


    public TMP_Text levelInfoText;
    public TMP_Text hpInfoText;
    public TMP_Text damageInfoText;

    // Existing code...

    private void DisplayUpgradeInfo(int currentLevel, int updatedLevel, int currentHP, int hpDiff, int currentDamage, int damageDiff)
    {
        levelInfoText.text = $"Level: {currentLevel} -> {updatedLevel}";
        hpInfoText.text = $"HP: {currentHP} + {hpDiff}";
        damageInfoText.text = $"Damage: {currentDamage} + {damageDiff}";
    }

    private void Start()
    {
        upgradeButton.onClick.AddListener(OnUpgradeButtonPressed);
    }

    private async void OnUpgradeButtonPressed()
{
    string tokenIdToUpgrade = nftCard.TokenId;
    if (string.IsNullOrEmpty(tokenIdToUpgrade))
    {
        Debug.LogError("Token ID is empty.");
        notificationText.text = "Token ID is empty.";
        return;
    }

    int currentLevel = ExtractNumber(nftCard.GetValueFromStats("Level"));
    int currentHP = ExtractNumber(nftCard.GetValueFromStats("Health"));
    int currentDamage = ExtractNumber(nftCard.GetValueFromStats("Damage"));
    

    // Log the original token ID string
    Debug.Log($"Token ID to Upgrade: {tokenIdToUpgrade}");

    UnboundedUInt tokenID = UnboundedUInt.FromBigInteger(BigInteger.Parse(tokenIdToUpgrade));

    // Log the UnboundedUInt representation of the token ID
    Debug.Log($"Token ID as UnboundedUInt: {tokenID}");

    try
    {
        // Access the CanisterLoginApiClient through CandidApiManager instance
        var apiClient = CandidApiManager.Instance.CanisterLogin;
        
        // Check if apiClient is null
        if (apiClient == null)
        {
            Debug.LogError("CanisterLoginApiClient is not initialized.");
            notificationText.text = "Error: API client not initialized.";
            return;
        }

        Debug.Log($"Triggering NFT upgrade for Token ID: {tokenIdToUpgrade}");

        (bool success, string message) = await apiClient.UpgradeNFT(tokenID);
        if (success)
        {
            Debug.Log("NFT upgrade successful!");
            notificationText.text = "Upgrade successful: " + message;
            await NFTManager.Instance.UpdateNFTMetadata(tokenIdToUpgrade);

            NFTData updatedData = NFTManager.Instance.GetNFTDataById(tokenIdToUpgrade);
            nftCard.SetNFTData(updatedData); // Make sure to update the NFTCard with the new data

            int updatedLevel = ExtractNumber(nftCard.GetValueFromStats("Level"));
            int updatedHP = ExtractNumber(nftCard.GetValueFromStats("Health"));
            int updatedDamage = ExtractNumber(nftCard.GetValueFromStats("Damage"));

            // Calculate differences
            DisplayUpgradeInfo(currentLevel, updatedLevel, currentHP, updatedHP - currentHP, currentDamage, updatedDamage - currentDamage);
        }
        else
        {
            Debug.LogError("NFT upgrade failed: " + message);
            notificationText.text = "Upgrade failed: " + message;
        }
    }
    catch (System.Exception ex)
    {
        Debug.LogError($"Exception during NFT upgrade: {ex.Message}");
        notificationText.text = $"Exception during upgrade: {ex.Message}";
    }
}

    private int ExtractNumber(string input, bool allowNotAvailable = false)
    {
        var match = System.Text.RegularExpressions.Regex.Match(input, @"\d+");
        if (match.Success)
        {
            return int.Parse(match.Value);
        }
        else if (allowNotAvailable && input.Contains("Not Available", StringComparison.OrdinalIgnoreCase))
        {
            return -1; // Or any other default value you prefer
        }
        else
        {
            Debug.LogError($"Failed to extract number from: {input}");
            return 0; // Or handle this case as appropriate
        }
    }

}