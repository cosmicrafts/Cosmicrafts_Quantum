using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Threading.Tasks;
using CanisterPK.CanisterLogin; // Ensure this uses the correct namespace for your project
using CanisterPK.CanisterLogin.Models; // Adjust if necessary based on actual namespace
using EdjCase.ICP.Candid.Models;
using Candid;

public class RewardsManager : MonoBehaviour
{
    public static RewardsManager Instance { get; private set; }

    public TMP_Text rewardsCountText;
    public GameObject rewardPrefab;
    public Transform rewardsContainer;

    // Assume apiClient is initialized somewhere, for example, in Start or Awake.
    private CanisterLoginApiClient apiClient;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    async void Start()
    {
        rewardPrefab.SetActive(false); // Ensure prefab is inactive by default
        await FetchUserRewards();
    }

    private async Task FetchUserRewards()
    {
        // Fetch rewards for the current user
        var rewards = await CandidApiManager.Instance.CanisterLogin.GetUserRewards();
        Debug.Log($"Fetched {rewards.Count} rewards.");

        // Log and handle each reward
        foreach (var reward in rewards)
        {
            // Assuming you want to log the detailed info of each reward for debugging
            Debug.Log($"Reward ID: {reward.IdReward}, " +
                    $"Prize Type: {reward.PrizeType}, " +
                    $"Prize Amount: {reward.PrizeAmount}, " +
                    $"Progress: {reward.Progress}/{reward.Total}, " +
                    $"Type: {reward.RewardType}");

            // Instantiate and set up each reward prefab
            InstantiateRewardPrefab(reward);
        }

        // Update UI based on fetched rewards
        if (rewards != null && rewards.Count > 0)
        {
            rewardsCountText.text = $"Rewards Available: {rewards.Count}";
            foreach (var reward in rewards)
            {
                InstantiateRewardPrefab(reward);
            }
        }
        else
        {
            rewardsCountText.text = "No Rewards Available";
        }
    }

    private void InstantiateRewardPrefab(RewardsUser reward)
    {
        var instance = Instantiate(rewardPrefab, rewardsContainer);
        instance.SetActive(true);

        var display = instance.GetComponent<RewardsDisplay>();
        if (display != null)
        {
            display.SetRewardData(reward);
        }
    }
}