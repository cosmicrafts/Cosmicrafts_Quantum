using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.MainCanister.Models;
using Candid;
using System;

namespace Cosmicrafts
{
    public class ChestOpenerUI : MonoBehaviour
    {
        public Button openButton;
        public TMP_Text notificationText;
        public GameObject rewardScreen;
        public RewardScreenUI rewardScreenUI;
        private ChestSO selectedChestSO;
        private UnboundedUInt selectedTokenId;

        private void Start()
        {
            openButton.onClick.AddListener(OnOpenButtonPressed);
        }

        public void SelectChestForOpening(ChestSO chestSO, UnboundedUInt tokenId)
        {
            selectedChestSO = chestSO;
            selectedTokenId = tokenId;
            rewardScreenUI.SetImage(chestSO.icon);
        }

public async void OnOpenButtonPressed()
{
    if (selectedChestSO == null || selectedTokenId == null)
    {
        Debug.LogError("No chest selected or chest data missing.");
        notificationText.text = "Error: No chest selected.";
        return;
    }
    Debug.Log($"Button pressed. Attempting to open chest: {selectedChestSO.chestName}");
    rewardScreenUI.ActivateRewardScreen();

    try
    {
        var apiClient = CandidApiManager.Instance.MainCanister;
        if (apiClient == null)
        {
            notificationText.text = "Error: API client not initialized.";
            return;
        }

        Debug.Log($"Triggering chest opening for Token ID: {selectedTokenId}");
        (bool success, string message) = await apiClient.OpenChest(selectedTokenId);

        if (success)
        {
            Debug.Log("Chest opened successfully.");
            notificationText.text = "Chest opened: " + message;

            // Parse the amount from the response message
            int stardustAmount = ParseAmountFromMessage(message);

            // Display the rewards using the parsed amount
            rewardScreenUI.DisplayRewards("Stardust", stardustAmount);

            rewardScreenUI.OnChestOpenedSuccessfully();
            ChestManager.Instance.RemoveChestAndRefreshCount(selectedTokenId);
        }
        else
        {
            Debug.LogError($"Failed to open chest: {message}");
            notificationText.text = $"Failed to open chest: {message}";
        }
    }
    catch (System.Exception ex)
    {
        Debug.LogError($"Exception during chest opening: {ex.Message}");
        notificationText.text = $"Exception during chest opening: {ex.Message}";
    }
}

private int ParseAmountFromMessage(string message)
{
    // Assuming the message format is "{\"token\":\"Stardust\", \"transaction_id\": 0, \"amount\": 101}"
    string amountString = ExtractValue(message, "\"amount\":", "}");
    int amount = 0;

    if (!int.TryParse(amountString, out amount))
    {
        Debug.LogError("Failed to parse amount from chest opening message: " + message);
    }

    return amount;
}

// Reuse the ExtractValue method from RewardScreenUI or implement a similar one in ChestOpenerUI
private string ExtractValue(string segment, string startDelimiter, string endDelimiter)
{
    int startIndex = segment.IndexOf(startDelimiter) + startDelimiter.Length;
    int endIndex = segment.IndexOf(endDelimiter, startIndex);
    if (endIndex == -1) endIndex = segment.Length; // Adjust if endDelimiter is not found.
    string value = segment.Substring(startIndex, endIndex - startIndex).Trim(new char[] { '\"', ' ', '}' });
    return value;
}
    }
}
