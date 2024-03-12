using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EdjCase.ICP.Candid.Models;
using System.Numerics;
using CanisterPK.CanisterLogin;
using Candid;

public class NFTUpgradeUI : MonoBehaviour
{
    public Button upgradeButton;
    public TMP_Text notificationText;
    public NFTDisplay nftDisplay;

    private void Start()
    {
        upgradeButton.onClick.AddListener(OnUpgradeButtonPressed);
    }

    private async void OnUpgradeButtonPressed()
    {
        string tokenIdToUpgrade = nftDisplay.TokenId;
        if (string.IsNullOrEmpty(tokenIdToUpgrade))
        {
            Debug.LogError("Token ID is empty.");
            notificationText.text = "Token ID is empty.";
            return;
        }

        UnboundedUInt tokenID = UnboundedUInt.FromBigInteger(BigInteger.Parse(tokenIdToUpgrade));

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
}