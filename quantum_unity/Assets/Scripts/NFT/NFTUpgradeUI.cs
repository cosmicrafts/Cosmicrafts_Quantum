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
    public NFTCardDetail nftCardDetail;  // Reference to the detailed view
    public TMP_Text levelInfoText;
    public TMP_Text hpInfoText;
    public TMP_Text damageInfoText;
    public NFTUpgradeScreenUI upgradeScreenUI;
    public shards shardsScript;

    [Header("NFT Display Info")]
    public TMP_Text nameText;
    public TMP_Text tokenIdText;
    public TMP_Text levelText;
    public Image avatarImage;
    public NotificationManager notificationManager;
    public TMP_Text tokenCostText;

    public static NFTUpgradeUI Instance;
    private bool isUpgradeInProgress = false;  // Flag to prevent multiple calls

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        FetchAndCheckShardsBalance();
        upgradeButton.onClick.AddListener(OnUpgradeButtonPressed);
        UpdateDisplayedNFTInfo();
    }

    private void FetchAndCheckShardsBalance()
    {
        if (nftCard != null)
        {
            int currentLevel = ExtractNumber(nftCard.GetValueFromStats("Level"));
            int upgradeCost = CalculateCost(currentLevel);
            upgradeCost += 1;  // Add 1 token fee

            tokenCostText.text = upgradeCost.ToString();

            BigInteger requiredShards = new BigInteger(upgradeCost);
            bool hasEnoughShards = shardsScript.CurrentBalance >= requiredShards;
            upgradeButton.interactable = hasEnoughShards;
            notificationText.text = hasEnoughShards ? "" : $"You need at least {upgradeCost} shards to upgrade.";

            // Set the color of the button and cost text based on the button state
            Color disabledColor = new Color32(0x9D, 0x9D, 0x9D, 0xFF); // #9D9D9D
            Color enabledColor = Color.white;

            tokenCostText.color = hasEnoughShards ? enabledColor : disabledColor;
            upgradeButton.GetComponentInChildren<TMP_Text>().color = hasEnoughShards ? enabledColor : disabledColor;
        }
    }

    private void DisplayUpgradeInfo(int currentLevel, int updatedLevel, int currentHP, int hpDiff, int currentDamage, int damageDiff)
    {
        levelInfoText.text = $"Level: {currentLevel} -> {updatedLevel}";
        hpInfoText.text = $"HP: {currentHP} + {hpDiff}";
        damageInfoText.text = $"Damage: {currentDamage} + {damageDiff}";
    }

    public async void OnUpgradeButtonPressed()
    {
        if (isUpgradeInProgress) return;  // Prevent multiple simultaneous calls
        isUpgradeInProgress = true;

        try
        {
            string tokenIdToUpgrade = nftCard.TokenId;
            if (string.IsNullOrEmpty(tokenIdToUpgrade))
            {
                Debug.LogError("Token ID is empty.");
                notificationText.text = "Token ID is empty.";
                isUpgradeInProgress = false;
                return;
            }

            LoadingPanel.Instance.ActiveLoadingPanel();

            int currentLevel = ExtractNumber(nftCard.GetValueFromStats("Level"));
            int currentHP = ExtractNumber(nftCard.GetValueFromStats("Health"));
            int currentDamage = ExtractNumber(nftCard.GetValueFromStats("Damage"));
            int upgradeCost = CalculateCost(currentLevel) + 1;  // Add 1 token fee

            // Send the upgrade request to the blockchain
            var apiClient = CandidApiManager.Instance.CanisterLogin;

            if (apiClient == null)
            {
                Debug.LogError("CanisterLoginApiClient is not initialized.");
                notificationText.text = "Error: API client not initialized.";
                isUpgradeInProgress = false;
                LoadingPanel.Instance.DesactiveLoadingPanel();
                return;
            }

            UnboundedUInt tokenID = UnboundedUInt.FromBigInteger(BigInteger.Parse(tokenIdToUpgrade));
            Debug.Log($"Triggering NFT upgrade for Token ID: {tokenIdToUpgrade}");

            (bool success, string message) = await apiClient.UpgradeNFT(tokenID);

            if (success)
            {
                Debug.Log("NFT upgrade successful!");
                notificationText.text = "Upgrade successful: " + message;

                // Calculate new values locally
                int newLevel = currentLevel + 1;
                int newHP = currentHP + (currentHP / 10);
                int newDamage = currentDamage + (currentDamage / 10);

                // Update local NFT data
                nftCard.UpdateStats(newLevel, newHP, newDamage);
                UpdateDisplayedNFTInfo();

                // Also update the detailed view
                if (nftCardDetail != null && nftCardDetail.TokenId == tokenIdToUpgrade)
                {
                    nftCardDetail.UpdateUI(nftCard.nftData);
                    nftCardDetail.UpdateStatsUI(); // Explicitly call to update the stats UI
                }

                // Display new values immediately
                DisplayUpgradeInfo(currentLevel, newLevel, currentHP, newHP - currentHP, currentDamage, newDamage - currentDamage);

                // Trigger UI update for upgrade screen
                upgradeScreenUI.ActivateUpgradeScreen(currentLevel, newLevel, currentHP, newHP - currentHP, currentDamage, newDamage - currentDamage);
                upgradeScreenUI.gameObject.SetActive(true);

                // Update the shard balance locally
                shardsScript.UpdateBalanceLocally(upgradeCost);

                // Refresh the upgrade cost and button state
                FetchAndCheckShardsBalance();
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
        finally
        {
            isUpgradeInProgress = false;
            LoadingPanel.Instance.DesactiveLoadingPanel();
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
            return -1;
        }
        else
        {
            Debug.LogError($"Failed to extract number from: {input}");
            return 0;
        }
    }

    private int CalculateCost(int level)
    {
        int cost = 9;
        for (int i = 2; i < level; i++)
        {
            cost += cost / 3; // Increase cost by ~33%
        }
        return cost;
    }

    private void UpdateDisplayedNFTInfo()
    {
        if (nftCard != null && nftCard.nftData != null)
        {
            nameText.text = nftCard.Name;
            tokenIdText.text = nftCard.TokenId;
            levelText.text = nftCard.Level;
            if (avatarImage != null)
            {
                avatarImage.sprite = nftCard.Avatar;
            }
            else
            {
                Debug.Log("avatarImage is null!");
            }

            // Refresh the upgrade cost and button state
            FetchAndCheckShardsBalance();
        }
    }

    public void HandleCardSelected(NFTCard selectedCard)
    {
        nftCard = selectedCard;
        UpdateDisplayedNFTInfo();

        // Also set the detailed view if it's the same card
        if (nftCardDetail != null && nftCardDetail.TokenId == selectedCard.TokenId)
        {
            nftCardDetail.SetNFTData(selectedCard.nftData);
            nftCardDetail.UpdateUI(selectedCard.nftData);
        }
    }

    public void TriggerUpgradeNotification()
    {
        if (nftCard != null)
        {
            string nftName = nftCard.Name;
            string nftTokenId = nftCard.TokenId;
            string nftLevel = nftCard.Level;

            string notificationMessage = $"{nftName} with ID {nftTokenId} has been upgraded to Level {ExtractNumber(nftLevel)}";
            notificationManager.ShowNotification(notificationMessage);
        }
        else
        {
            Debug.LogError("NFT card is null.");
        }
    }
}
