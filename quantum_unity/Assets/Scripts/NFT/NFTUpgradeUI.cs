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
    public NFTUpgradeScreenUI upgradeScreenUI;
    public shards ShardsScript;

    [Header("NFT Display Info")]
    public TMP_Text nameText;
    public TMP_Text tokenIdText;
    public TMP_Text levelText;
    public Image avatarImage;
    public NotificationManager notificationManager;

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
        BigInteger requiredShards = new BigInteger(10);  // Assuming 10 is the correct cost
        bool hasEnoughShards = ShardsScript.CurrentBalance >= requiredShards;
        upgradeButton.interactable = hasEnoughShards;
        notificationText.text = hasEnoughShards ? "" : "You need at least 10 shards to upgrade.";
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

            Debug.Log($"Token ID to Upgrade: {tokenIdToUpgrade}");

            UnboundedUInt tokenID = UnboundedUInt.FromBigInteger(BigInteger.Parse(tokenIdToUpgrade));

            Debug.Log($"Token ID as UnboundedUInt: {tokenID}");

            var apiClient = CandidApiManager.Instance.CanisterLogin;

            if (apiClient == null)
            {
                Debug.LogError("CanisterLoginApiClient is not initialized.");
                notificationText.text = "Error: API client not initialized.";
                isUpgradeInProgress = false;
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
                if (updatedData != null)
                {
                    nftCard.SetNFTData(updatedData);
                    UpdateDisplayedNFTInfo();
                    int updatedLevel = ExtractNumber(nftCard.GetValueFromStats("Level"));
                    int updatedHP = ExtractNumber(nftCard.GetValueFromStats("Health"));
                    int updatedDamage = ExtractNumber(nftCard.GetValueFromStats("Damage"));
                    DisplayUpgradeInfo(currentLevel, updatedLevel, currentHP, updatedHP - currentHP, currentDamage, updatedDamage - currentDamage);
                    upgradeScreenUI.ActivateUpgradeScreen(currentLevel, updatedLevel, currentHP, updatedHP - currentHP, currentDamage, updatedDamage - currentDamage);
                }
                LoadingPanel.Instance.DesactiveLoadingPanel();
                upgradeScreenUI.gameObject.SetActive(true);
                //ShardsScript.FetchBalance();
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
        }
    }

    public void HandleCardSelected(NFTCard selectedCard)
    {
        nftCard = selectedCard;
        UpdateDisplayedNFTInfo();
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