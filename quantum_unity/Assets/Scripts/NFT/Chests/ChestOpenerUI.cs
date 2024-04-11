using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EdjCase.ICP.Candid.Models;
using CanisterPK.CanisterLogin;
using CanisterPK.chests.Models;
using Candid;
using System;

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
            var apiClient = CandidApiManager.Instance.CanisterLogin;
            if (apiClient == null)
            {
                Debug.LogError("CanisterLoginApiClient is not initialized.");
                notificationText.text = "Error: API client not initialized.";
                return;
            }

            Debug.Log($"Triggering chest opening for Token ID: {selectedTokenId}");
            (bool success, string message) = await apiClient.OpenChests(selectedTokenId);

            if (success)
            {
                Debug.Log("Chest opened successfully.");
                notificationText.text = "Chest opened: " + message;
                rewardScreenUI.HandleRewardMessage(message);
                rewardScreenUI.OnChestOpenedSuccessfully();
                ChestManager.Instance.RemoveChestAndRefreshCount(selectedTokenId);
                // Consider what to do next - perhaps hide this UI or reset for another chest opening
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
}