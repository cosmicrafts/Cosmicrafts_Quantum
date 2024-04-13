using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using CanisterPK.CanisterLogin.Models;
using EdjCase.ICP.Candid.Models;

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

    public void SetRewardData(RewardsUser reward)
    {
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
}
