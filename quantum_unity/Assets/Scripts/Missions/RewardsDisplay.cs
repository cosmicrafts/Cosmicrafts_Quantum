using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using CanisterPK.CanisterLogin.Models;
using EdjCase.ICP.Candid.Models;
using Candid;

public class RewardsDisplay : MonoBehaviour
{
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

    private MissionsUser rewardData;

    public Sprite fluxSprite;
    public Sprite shardsSprite;
    public Sprite chest1Sprite;
    public Sprite chest2Sprite;
    public Sprite chest3Sprite;
    public Sprite chest4Sprite;
    public Sprite chest5Sprite;
    public Sprite chest6Sprite;
    public Sprite chest7Sprite;
    public Sprite chest8Sprite;
    
    public GameObject panelToChangeColor;
    public Color gamesPlayedColor;
    public Color gamesWonColor;

    public shards ShardsScript;
    public flux FluxScript;
    public ChestManager ChestsScript;
    public Image claimButtonImage;
    public Image progressBar;

    public void SetRewardData(MissionsUser reward)
    {
        rewardData = reward;

        Debug.Log($"ID: {reward.IdMission}");
        Debug.Log($"Reward Type: {reward.RewardType}");
        Debug.Log($"Prize Amount: {reward.RewardAmount}");
        Debug.Log($"Progress: {reward.Progress}/{reward.Total}");
        Debug.Log($"Expiration: {CalculateTimeRemaining(reward.Expiration)}");
        Debug.Log($"Completed: {(reward.Finished ? "Yes" : "No")}");

        string gameOrGames = reward.Total == 1 ? "Game" : "Games";
        string winOrWins = reward.Total == 1 ? "Win" : "Wins";

        string missionText = reward.MissionType switch
        {
            MissionType.GamesCompleted => $"Play {reward.Total} {gameOrGames}",
            MissionType.GamesWon => $"{winOrWins} {reward.Total} {gameOrGames}",
            _ => "Unknown Mission"
        };
        rewardTypeText.text = missionText;

        idText.text = $"ID: {reward.IdMission}";
        prizeAmountText.text = $"{reward.RewardAmount}";
        progressText.text = $"{reward.Progress}/{reward.Total}";

        expirationText.text = CalculateTimeRemaining(reward.Expiration);

        finishedText.text = $"Completed: {(reward.Finished ? "Yes" : "No")}";
        prizeTypeText.text = $"Reward: {(reward.RewardType == MissionRewardType.Shards ? "Shards" : reward.RewardType == MissionRewardType.Chest ? "Chest" : "Flux")}";

        Sprite selectedSprite = GetPrizeSprite(reward.RewardType, reward.RewardAmount);
        prizeImage.sprite = selectedSprite;
        prizeImage.enabled = selectedSprite != null;

        panelToChangeColor.GetComponent<Image>().color = reward.MissionType == MissionType.GamesCompleted ? gamesPlayedColor : gamesWonColor;

        bool isClaimable = reward.Finished;
        claimButtonImage.enabled = isClaimable;
        claimButton.interactable = isClaimable;

        float progressRatio = (float)(ulong)reward.Progress / (float)(ulong)reward.Total;
        progressBar.fillAmount = progressRatio;
    }

    private DateTime UnixTimeStampToDateTime(ulong unixTimeStamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds((double)unixTimeStamp / 1000000000);
        return dateTime.ToLocalTime();
    }

    private string CalculateTimeRemaining(ulong unixTimeStamp)
    {
        // Blockchain time is in UTC, so we need to convert it to local time
        DateTime blockchainTimeUtc = UnixTimeStampToDateTime(unixTimeStamp);
        DateTime localTime = blockchainTimeUtc.ToLocalTime();
        
        DateTime now = DateTime.Now; // Local machine time

        TimeSpan timeRemaining = localTime - now;

        if (timeRemaining.TotalSeconds < 0)
        {
            return "Expired";
        }

        string formattedTime = $"{(int)timeRemaining.TotalDays}d {(int)timeRemaining.Hours}h {(int)timeRemaining.Minutes}m";
        return $"Time remaining: {formattedTime}";
    }

    private Sprite GetPrizeSprite(MissionRewardType prizeType, UnboundedUInt prizeAmount)
    {
        return prizeType switch
        {
            MissionRewardType.Shards => shardsSprite,
            MissionRewardType.Flux => fluxSprite,
            MissionRewardType.Chest => SelectChestSprite(prizeAmount),
            _ => null
        };
    }

    private Sprite SelectChestSprite(UnboundedUInt prizeAmount)
    {
        int amount = (int)prizeAmount;

        return amount switch
        {
            1 => chest1Sprite,
            2 => chest2Sprite,
            3 => chest3Sprite,
            4 => chest4Sprite,
            5 => chest5Sprite,
            6 => chest6Sprite,
            7 => chest7Sprite,
            8 => chest8Sprite,
            _ => null
        };
    }
public async void OnClaimButtonClicked()
{
    if (rewardData != null && rewardData.Finished)
    {
        LoadingPanel.Instance.ActiveLoadingPanel();
        bool success;

        if (RewardsManager.Instance.UserMissions.Contains(rewardData))
        {
            success = await RewardsManager.Instance.ClaimUserReward(rewardData.IdMission);
            await RewardsManager.Instance.RefreshUserMissions(); // Refresh the missions after claiming the reward
        }
        else if (RewardsManager.Instance.GeneralMissions.Contains(rewardData))
        {
            success = await RewardsManager.Instance.ClaimGeneralReward(rewardData.IdMission);
            await RewardsManager.Instance.RefreshGeneralMissions(); // Refresh the missions after claiming the reward
        }
        else
        {
            Debug.LogError("Unknown mission type.");
            success = false;
        }

        if (success)
        {
            Debug.Log("Reward claimed successfully!");
            LoadingPanel.Instance.DesactiveLoadingPanel();
            Destroy(gameObject);
            ShowRewardNotification(rewardData);
        }
        else
        {
            LoadingPanel.Instance.DesactiveLoadingPanel();
            Debug.LogError("Failed to claim reward.");
        }
    }
    else
    {
        Debug.LogError("No reward data available to claim.");
    }
}

    private void ShowRewardNotification(MissionsUser reward)
    {
        string notificationMessage = reward.RewardType switch
        {
            MissionRewardType.Flux => $"You received {reward.RewardAmount} Flux!",
            MissionRewardType.Shards => $"You received {reward.RewardAmount} Shards!",
            MissionRewardType.Chest => $"You received a {GetChestRarity(reward.RewardAmount)} Metacube!",
            _ => string.Empty
        };

        if (!string.IsNullOrEmpty(notificationMessage) && notificationManager != null)
        {
            notificationManager.ShowNotification(notificationMessage);
        }
    }

    private string GetChestRarity(UnboundedUInt prizeAmount)
    {
        int amount = (int)prizeAmount;
        return amount switch
        {
            >= 7 => "Mythical",
            6 => "Legendary",
            5 => "Epic",
            4 => "Superior",
            3 => "Rare",
            2 => "Common",
            _ => "Basic"
        };
    }
}
