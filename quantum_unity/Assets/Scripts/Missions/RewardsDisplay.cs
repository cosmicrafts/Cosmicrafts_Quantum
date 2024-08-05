using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cosmicrafts.Managers;
using System;
using CanisterPK.CanisterLogin.Models;
using EdjCase.ICP.Candid.Models;
using Candid;
using System.Threading.Tasks;
using System;

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

    public MissionData rewardData { get; private set; }

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

    public void SetRewardData(MissionData reward)
    {
        rewardData = reward;

        idText.text = $"ID: {reward.idMission}";
        rewardTypeText.text = GetMissionTypeText(reward.missionType, reward.total);
        prizeAmountText.text = $"{reward.rewardAmount}";
        progressText.text = $"{reward.progress}/{reward.total}";

        expirationText.text = CalculateTimeRemaining(reward.expiration);

        finishedText.text = $"Completed: {(reward.finished ? "Yes" : "No")}";
        prizeTypeText.text = $"Reward: {(reward.rewardType == MissionRewardType.Shards ? "Shards" : reward.rewardType == MissionRewardType.Chest ? "Chest" : "Flux")}";

        prizeImage.sprite = GetPrizeSprite(reward.rewardType, reward.rewardAmount);
        panelToChangeColor.GetComponent<Image>().color = GetMissionTypeColor(reward.missionType);

        bool isClaimable = reward.finished || reward.total == 0;
        claimButtonImage.enabled = isClaimable;
        claimButton.interactable = isClaimable;

        float progressRatio = reward.total == 0 ? 1f : (float)reward.progress / reward.total;
        progressBar.fillAmount = progressRatio;
    }

    private string GetMissionTypeText(MissionType missionType, int total)
    {
        return missionType switch
        {
            MissionType.GamesCompleted => $"Play {total} {(total == 1 ? "Game" : "Games")}",
            MissionType.GamesWon => $"Win {total} {(total == 1 ? "Game" : "Games")}",
            MissionType.DamageDealt => $"Deal {total} Damage",
            MissionType.DamageTaken => $"Take {total} Damage",
            MissionType.EnergyUsed => $"Use {total} Energy",
            MissionType.FactionPlayed => $"Play as {total} Different Factions",
            MissionType.GameModePlayed => $"Play {total} Different Game Modes",
            MissionType.Kills => $"Achieve {total} Kills",
            MissionType.UnitsDeployed => $"Deploy {total} Units",
            MissionType.XPEarned => $"Earn {total} XP",
            _ => "Unknown Mission"
        };
    }

    private DateTime UnixTimeStampToDateTime(ulong unixTimeStamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp / 1000000000);
        return dateTime.ToLocalTime();
    }

    private string CalculateTimeRemaining(ulong unixTimeStamp)
    {
        DateTime blockchainTimeUtc = UnixTimeStampToDateTime(unixTimeStamp);
        DateTime localTime = blockchainTimeUtc.ToLocalTime();

        DateTime now = DateTime.Now;

        TimeSpan timeRemaining = localTime - now;

        if (timeRemaining.TotalSeconds < 0)
        {
            return "Expired";
        }

        string formattedTime = $"{(int)timeRemaining.TotalDays}d {(int)timeRemaining.Hours}h {(int)timeRemaining.Minutes}m";
        return $"Time remaining: {formattedTime}";
    }

    private Sprite GetPrizeSprite(MissionRewardType prizeType, int prizeAmount)
    {
        return prizeType switch
        {
            MissionRewardType.Shards => shardsSprite,
            MissionRewardType.Flux => fluxSprite,
            MissionRewardType.Chest => SelectChestSprite(prizeAmount),
            _ => null
        };
    }

    private Sprite SelectChestSprite(int prizeAmount)
    {
        return prizeAmount switch
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

    private Color GetMissionTypeColor(MissionType missionType)
    {
        return missionType switch
        {
            MissionType.GamesCompleted => gamesPlayedColor,
            MissionType.GamesWon => gamesWonColor,
            MissionType.DamageDealt => gamesPlayedColor,
            MissionType.DamageTaken => gamesWonColor,
            MissionType.EnergyUsed => gamesPlayedColor,
            MissionType.FactionPlayed => gamesWonColor,
            MissionType.GameModePlayed => gamesPlayedColor,
            MissionType.Kills => gamesWonColor,
            MissionType.UnitsDeployed => gamesPlayedColor,
            MissionType.XPEarned => gamesWonColor,
            _ => gamesPlayedColor
        };
    }

    public async void OnClaimButtonClicked()
    {
        if (rewardData != null && rewardData.finished)
        {
            LoadingPanel.Instance.ActiveLoadingPanel();
            bool success = await ClaimReward();

            if (success)
            {
                Debug.Log("Reward claimed successfully!");
                LoadingPanel.Instance.DesactiveLoadingPanel();
                ShowRewardNotification(rewardData);
                RefreshUI();
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

    private async Task<bool> ClaimReward()
    {
        bool success;

        if (MissionManager.Instance.userMissions.Contains(rewardData))
        {
            success = await MissionManager.Instance.ClaimUserReward(rewardData.idMission);
        }
        else if (MissionManager.Instance.generalMissions.Contains(rewardData))
        {
            success = await MissionManager.Instance.ClaimGeneralReward(rewardData.idMission);
        }
        else
        {
            Debug.LogError("Unknown mission type.");
            success = false;
        }

        return success;
    }

    private void ShowRewardNotification(MissionData reward)
    {
        string notificationMessage = reward.rewardType switch
        {
            MissionRewardType.Flux => $"You received {reward.rewardAmount} Flux!",
            MissionRewardType.Shards => $"You received {reward.rewardAmount} Shards!",
            MissionRewardType.Chest => $"You received a {GetChestRarity(reward.rewardAmount)} Metacube!",
            _ => string.Empty
        };

        if (!string.IsNullOrEmpty(notificationMessage) && notificationManager != null)
        {
            notificationManager.ShowNotification(notificationMessage);
        }
    }

    private string GetChestRarity(int prizeAmount)
    {
        return prizeAmount switch
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

    private void RefreshUI()
    {
        RewardsManager.Instance.RefreshMissions();
    }
}
