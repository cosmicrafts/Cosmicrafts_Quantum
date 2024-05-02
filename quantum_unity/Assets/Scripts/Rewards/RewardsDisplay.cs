using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using CanisterPK.CanisterLogin.Models;
using CanisterPK.CanisterLogin;
using EdjCase.ICP.Candid.Models;
using System.Threading.Tasks;
using Candid;

public class RewardsDisplay : MonoBehaviour
{
    // Existing fields...
    public TMP_Text idText;
    public TMP_Text rewardTypeText;
    public TMP_Text prizeAmountText;
    public TMP_Text progressText;
    public TMP_Text expirationText;
    public TMP_Text finishedText;
    public TMP_Text prizeTypeText;
    public Image prizeImage;
    public Button claimButton;
    public NotificationManager notificationManager;

    private RewardsUser rewardData;

    public Sprite fluxSprite;
    public Sprite shardsSprite;
    public Sprite chest1Sprite;
    public Sprite chest2Sprite;
    public Sprite chest3Sprite;
    public Sprite chest4Sprite;
    public Sprite chest5Sprite;
    public Sprite chest6Sprite;

    public GameObject panelToChangeColor;
    public Color gamesPlayedColor;
    public Color gamesWonColor;

    public shards ShardsScript;
    public flux FluxScript;
    public ChestManager ChestsScript;
    public Image claimButtonImage;
    public Image progressBar;


    public void SetRewardData(RewardsUser reward)
    {

        Debug.Log($"ID: {reward.IdReward}");
        Debug.Log($"Reward Type: {reward.RewardType}");
        Debug.Log($"Prize Amount: {reward.PrizeAmount}");
        Debug.Log($"Progress: {reward.Progress}/{reward.Total}");
        Debug.Log($"Expiration: {CalculateTimeRemaining(reward.Expiration)}");
        Debug.Log($"Completed: {(reward.Finished ? "Yes" : "No")}");
        Debug.Log($"Prize Type: {(reward.PrizeType == PrizeType.Shards ? "Shards" : reward.PrizeType == PrizeType.Chest ? "Chest" : "Flux")}");

        // Determine singular or plural form for game(s) based on reward.Total
        string gameOrGames = reward.Total == 1 ? "Game" : "Games";
        string winOrWins = reward.Total == 1 ? "Win" : "Wins";

        string missionText = reward.RewardType switch
        {
            RewardType.GamesCompleted => $"Play {reward.Total} {gameOrGames}",
            RewardType.GamesWon => $"{winOrWins} {reward.Total} {gameOrGames}",
            _ => "Unknown Mission" // Default case if there are other types
        };
        rewardTypeText.text = missionText;

        idText.text = $"ID: {reward.IdReward}";
        prizeAmountText.text = $"{reward.PrizeAmount}";
        progressText.text = $"{reward.Progress}/{reward.Total}";

        // Date and time formatting to display "Time remaining"
        expirationText.text = CalculateTimeRemaining(reward.Expiration);

        finishedText.text = $"Completed: {(reward.Finished ? "Yes" : "No")}";
        prizeTypeText.text = $"Reward: {(reward.PrizeType == PrizeType.Shards ? "Shards" : reward.PrizeType == PrizeType.Chest ? "Chest" : "Flux")}";

        Sprite selectedSprite = GetPrizeSprite(reward.PrizeType, reward.PrizeAmount);
        prizeImage.sprite = selectedSprite;
        prizeImage.enabled = selectedSprite != null;

        panelToChangeColor.GetComponent<Image>().color = reward.RewardType == RewardType.GamesCompleted ? gamesPlayedColor : gamesWonColor;

        rewardData = reward;
        bool isClaimable = reward.Finished;
        claimButtonImage.enabled = isClaimable; // Enable or disable the claim button based on completion status
        claimButton.interactable = isClaimable;

        // Update the progress bar
        float progressRatio = (float)reward.Progress / (float)reward.Total;
        progressBar.fillAmount = progressRatio;
    }


    private DateTime UnixTimeStampToDateTime(ulong unixTimeStamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp / 1000000000).ToLocalTime();
        return dateTime;
    }

    private string CalculateTimeRemaining(ulong unixTimeStamp)
    {
        DateTime now = DateTime.UtcNow;
        DateTime expiryDate = UnixTimeStampToDateTime(unixTimeStamp);
        TimeSpan timeRemaining = expiryDate - now;

        if (timeRemaining.TotalSeconds < 0)
        {
            return "Expired";
        }

        string formattedTime = $"{(int)timeRemaining.TotalDays}d {(int)timeRemaining.Hours}h {(int)timeRemaining.Minutes}m";
        return $"Time remaining: {formattedTime}";
    }

    private Sprite GetPrizeSprite(PrizeType prizeType, UnboundedUInt prizeAmount)
    {
        switch (prizeType)
        {
            case PrizeType.Shards:
                return shardsSprite;
            case PrizeType.Flux:
                return fluxSprite;
            case PrizeType.Chest:
                return SelectChestSprite(prizeAmount);
            default:
                return null; // Handle unknown prize type
        }
    }

   private Sprite SelectChestSprite(UnboundedUInt prizeAmount)
    {
        int amount = (int)prizeAmount; // Assuming this cast is safe given your data model

        switch (amount)
        {
            case 1:
                return chest1Sprite;
            case 2:
                return chest2Sprite;
            case 3:
                return chest3Sprite;
            case 4:
                return chest4Sprite;
            case 5:
                return chest5Sprite;
            case 6:
                return chest6Sprite;
            default:
                return null; // Handle cases where the prize amount doesn't match expected values
        }
    }

    public async void OnClaimButtonClicked()
    {
        
        if (rewardData != null && rewardData.Finished)
        {
            LoadingPanel.Instance.ActiveLoadingPanel();
            UnboundedUInt rewardID = rewardData.IdReward; // Access the ID from the stored reward data
            var claimResult = await CandidApiManager.Instance.CanisterLogin.ClaimReward(rewardID);
            if (claimResult.ReturnArg0)
            {
                Debug.Log("Reward claimed successfully!");
                LoadingPanel.Instance.DesactiveLoadingPanel();

                // Perform actions based on the reward type
                switch (rewardData.PrizeType)
                {
                    case PrizeType.Flux:
                        FluxScript.FetchBalance();
                        break;
                    case PrizeType.Shards:
                        ShardsScript.FetchBalance();
                        break;
                    case PrizeType.Chest:
                        ChestsScript.UpdateOwnedChests();
                        break;
                    default:
                        Debug.LogError("Unknown reward type");
                        break;
                }
                Destroy(gameObject);
                ShowRewardNotification(rewardData);
            }
            else
            {
                LoadingPanel.Instance.DesactiveLoadingPanel();
                Debug.LogError($"Failed to claim reward: {claimResult.ReturnArg1}");
                // Optionally handle error or inform the user about the failure
            }
        }
        else
        {
            Debug.LogError("No reward data available to claim.");
        }
    }

    private void ShowRewardNotification(RewardsUser reward)
    {
        string notificationMessage = "";
        switch (reward.PrizeType)
        {
            case PrizeType.Flux:
                notificationMessage = $"You received {reward.PrizeAmount} Flux!";
                break;
            case PrizeType.Shards:
                notificationMessage = $"You received {reward.PrizeAmount} Shards!";
                break;
            case PrizeType.Chest:
                notificationMessage = $"You received a chest with rarity {GetChestRarity(reward.PrizeAmount)}!";
                break;
            default:
                break;
        }

        if (!string.IsNullOrEmpty(notificationMessage) && notificationManager != null)
        {
            notificationManager.ShowNotification(notificationMessage);
        }
    }
    private string GetChestRarity(UnboundedUInt prizeAmount)
    {
        // Implement logic to determine chest rarity based on prize amount
        // For example:
        int amount = (int)prizeAmount;
        if (amount >= 5)
        {
            return "Legendary";
        }
        else if (amount >= 3)
        {
            return "Epic";
        }
        else if (amount >= 1)
        {
            return "Rare";
        }
        else
        {
            return "Common";
        }
    }
}
