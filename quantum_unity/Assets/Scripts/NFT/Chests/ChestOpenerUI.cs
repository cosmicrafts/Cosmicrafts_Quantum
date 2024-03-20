using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EdjCase.ICP.Candid.Models;
using System.Numerics;
using CanisterPK.CanisterLogin; 
using CanisterPK.chests.Models;
using Candid;
using System;

public class ChestOpenerUI : MonoBehaviour
{
    public Button openButton; 
    public TMP_Text notificationText;
    public GameObject rewardScreen;
    public Image chestImage;
    public RewardScreenUI rewardScreenUI;

    private void Start()
    {
        openButton.onClick.AddListener(OnOpenButtonPressed);
    }

    private async void OnOpenButtonPressed()
    {
        
        Debug.Log("Button pressed. Attempting to activate reward screen.");

        RewardScreenUI rewardScreenUI = rewardScreen.GetComponent<RewardScreenUI>();
        if (rewardScreenUI != null)
        {
            rewardScreenUI.onActivateRewardScreen.Invoke(); // Invoke the event to activate the screen
            rewardScreenUI.SetImage(chestImage.sprite);
        }
        else
        {
            Debug.LogError("RewardScreenUI component not found on the reward screen GameObject.");
            return;
        }

        ChestTokenStorage tokenStorage = GetComponent<ChestTokenStorage>();
        if (tokenStorage == null) 
        {
            Debug.LogError("Chest does not have ChestTokenStorage component.");
            notificationText.text = "Error: Chest data missing.";
            return;
        }

        // Correctly use 'tokenId' obtained from the ChestTokenStorage
        UnboundedUInt tokenId = tokenStorage.tokenId;

        try
        {
            var apiClient = CandidApiManager.Instance.CanisterLogin; 
            if (apiClient == null)
            {
                Debug.LogError("CanisterLoginApiClient is not initialized.");
                notificationText.text = "Error: API client not initialized.";
                return;
            }

            // Correctly log and use 'tokenId' in the call
            Debug.Log($"Triggering chest opening for Token ID: {tokenId}");
            (bool success, string message) = await apiClient.OpenChests(tokenId); // Correct variable name

            if (success)
            {
                Debug.Log("[ChestOpenerUI] Chest opened successfully. Rewards fetched set to true.");

                notificationText.text = "Chest opened: " + message;

                rewardScreenUI.HandleRewardMessage(message);

                rewardScreenUI.OnChestOpenedSuccessfully();

                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Chest opening failed: " + message);
                notificationText.text = "Failed to open chest: " + message;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Exception during chest opening: {ex.Message}");
            notificationText.text = $"Exception during chest opening: {ex.Message}";
        }
    }
}
