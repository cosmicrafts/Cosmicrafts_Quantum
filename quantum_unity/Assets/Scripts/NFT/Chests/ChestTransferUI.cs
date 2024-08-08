using UnityEngine;
using TMPro;
using EdjCase.ICP.Candid.Models; 
using UnityEngine.UI;
using System;

namespace Cosmicrafts
{
    public class ChestTransferUI : MonoBehaviour
    {
        public TMP_InputField recipientPrincipalInput;
        public Button transferButton;
        public TMP_Text notificationText;
        public ChestManager chestManager;
        public TMP_Text chestNameText;
        public TMP_Text tokenIdText;
        private UnboundedUInt selectedChestTokenId;
        public Image chestImage;
        private ChestInstance selectedChestInstance;
        public NotificationManager notificationManager;

        private void Start()
        {
            transferButton.onClick.AddListener(OnTransferButtonPressed);
        }

        public void SetSelectedChest(ChestInstance chestInstance)
        {
            if (chestInstance == null)
            {
                Debug.LogError("SetSelectedChest was called with a null chestInstance.");
                return;
            }

            // Log details of chestInstance to ensure they are correct
            Debug.Log($"Chest selected: {chestInstance.chestSO.chestName}, Token ID: {chestInstance.tokenId}");

            selectedChestInstance = chestInstance;
            selectedChestTokenId = chestInstance.tokenId;

            if (chestNameText != null)
            {
                chestNameText.text = chestInstance.chestSO.chestName;
                Debug.Log("Chest name set on UI.");
            }
            else
            {
                Debug.LogError("chestNameText is not assigned in the inspector.");
            }

            if (tokenIdText != null)
            {
                tokenIdText.text = selectedChestTokenId.ToString(); // Update Token ID text on the UI
                Debug.Log("Token ID set on UI.");
            }
            else
            {
                Debug.LogError("tokenIdText is not assigned in the inspector.");
            }

            if (chestImage != null)
            {
                chestImage.sprite = chestInstance.chestSO.icon;
                Debug.Log("Chest image set on UI.");
            }
            else
            {
                Debug.LogError("chestImage is not assigned in the inspector.");
            }
        }


        private async void OnTransferButtonPressed()
        {

            Debug.Log("Transfer button pressed.");
            if (selectedChestInstance == null || selectedChestTokenId == null)
            {
                notificationText.text = "No chest selected.";
                Debug.LogError("No chest selected or chest token ID is null.");
                return;
            }

            string recipientPrincipalText = recipientPrincipalInput.text.Trim();
            if (string.IsNullOrEmpty(recipientPrincipalText))
            {
                notificationText.text = "Recipient Principal is empty.";
                return;
            }

            try
            {
                LoadingPanel.Instance.ActiveLoadingPanel();
                Debug.Log($"Initiating transfer for {selectedChestTokenId} to {recipientPrincipalText}");
                await chestManager.TransferChest(selectedChestInstance.chestSO, selectedChestTokenId, recipientPrincipalText);
                notificationText.text = "Chest transfer initiated. Please wait...";
                LoadingPanel.Instance.DesactiveLoadingPanel();

                // Show the notification when the transfer is successful
                string notificationMessage = $"{selectedChestInstance.chestSO.chestName} with ID {selectedChestTokenId} has been transferred.";
                notificationManager.ShowNotification(notificationMessage);

            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception during chest transfer: {ex}");
                notificationText.text = $"Exception during chest transfer: {ex.Message}";
                LoadingPanel.Instance.DesactiveLoadingPanel();
            }
        }
    }
}