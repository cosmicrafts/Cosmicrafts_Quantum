using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Threading.Tasks;
using CanisterPK.CanisterLogin;
using CanisterPK.CanisterLogin.Models;
using EdjCase.ICP.Candid.Models;
using Candid;

public class RewardsManager : MonoBehaviour
{
    public static RewardsManager Instance { get; private set; }

    public TMP_Text rewardsCountText;
    public GameObject rewardPrefab;
    public Transform rewardsContainer;

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
        rewardPrefab.SetActive(false);
        await FetchUserRewards();
    }

    private async Task FetchUserRewards()
    {
        var rewards = await CandidApiManager.Instance.CanisterLogin.GetUserRewards();
        Debug.Log($"Fetched {rewards.Count} rewards.");

        foreach (var reward in rewards)
        {
            InstantiateRewardPrefab(reward);
        }

        if (rewards != null && rewards.Count > 0)
        {
            rewardsCountText.text = $"Missions Available: {rewards.Count}";
        }
        else
        {
            rewardsCountText.text = "All Missions completed";
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