using UnityEngine;
using TMPro;
using System;
using CanisterPK.CanisterLogin.Models;


public class RewardsDisplay : MonoBehaviour
{
    // Reference to UI text components, assign them in the Unity Editor
    public TMP_Text idText;
    public TMP_Text rewardTypeText;
    public TMP_Text prizeAmountText;
    public TMP_Text progressText;
    public TMP_Text expirationText;
    public TMP_Text finishedText;
    public TMP_Text prizeTypeText;

    public void SetRewardData(RewardsUser reward)
    {
        // Handling RewardType display based on total required quantity
        string missionText = reward.RewardType switch
        {
            RewardType.GamesCompleted => $"Mission: Play {reward.Total} Games",
            RewardType.GamesWon => $"Mission: Win {reward.Total} Games",
            _ => $"Mission: Unknown Type" // Default case if there are other types
        };
        rewardTypeText.text = missionText;

        idText.text = $"ID: {reward.IdReward}";
        prizeAmountText.text = $"Prize Amount: {reward.PrizeAmount}";
        progressText.text = $"Progress: {reward.Progress}/{reward.Total}";

        // Date and time formatting to display "Time remaining"
        expirationText.text = CalculateTimeRemaining(reward.Expiration);

        finishedText.text = $"Finished: {(reward.Finished ? "Yes" : "No")}";
        prizeTypeText.text = $"Prize Type: {(reward.PrizeType == PrizeType.Shards ? "Shards" : reward.PrizeType == PrizeType.Chest ? "Chest" : "Flux")}";
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
}
