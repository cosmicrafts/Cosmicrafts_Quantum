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

                    // Calculate token amounts based on rarity
                    var (shardsAmount, fluxAmount) = CalculateTokensAmount(selectedChestSO.rarity);

                    // Display the calculated rewards
                    rewardScreenUI.DisplayRewards("Shards", shardsAmount);
                    rewardScreenUI.DisplayRewards("Flux", fluxAmount);

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

        private (int shardsAmount, int fluxAmount) CalculateTokensAmount(int rarity)
        {
            int factor = 1;

            if (rarity <= 5)
            {
                factor = (int)Math.Pow(2, rarity - 1);
            }
            else if (rarity <= 10)
            {
                factor = (int)(Math.Pow(2, 5) * Math.Pow(3, rarity - 6));
            }
            else if (rarity <= 15)
            {
                factor = (int)(Math.Pow(2, 5) * Math.Pow(3, 5) * Math.Pow(5, rarity - 11));
            }
            else if (rarity <= 20)
            {
                factor = (int)(Math.Pow(2, 5) * Math.Pow(3, 5) * Math.Pow(5, 5) * Math.Pow(11, rarity - 16));
            }
            else
            {
                factor = (int)(Math.Pow(2, 5) * Math.Pow(3, 5) * Math.Pow(5, 5) * Math.Pow(11, 5) * Math.Pow(21, rarity - 21));
            }

            int shardsAmount = 12 * factor;
            int fluxAmount = 4 * factor;

            return (shardsAmount, fluxAmount);
        }

    }
}