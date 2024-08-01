using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Threading.Tasks;
using CanisterPK.CanisterLogin.Models;
using EdjCase.ICP.Candid.Models;
using Candid;
using System;

public class RewardsManager : MonoBehaviour
{
    public static RewardsManager Instance { get; private set; }

    public TMP_Text rewardsCountText;
    public GameObject rewardPrefab;
    public Transform rewardsContainer;

    // Store missions here
    public List<MissionsUser> UserMissions { get; private set; }
    public List<MissionsUser> GeneralMissions { get; private set; }

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

        // Fetch and populate rewards UI
        await FetchAndPopulateRewardsUI();
    }

    private async Task FetchAndPopulateRewardsUI()
    {
        Debug.Log("[RewardsManager] Populating Missions UI");

        var fetchUserMissionsTask = SearchActiveUserMissions();
        var fetchGeneralMissionsTask = SearchActiveGeneralMissions();

        // Await the missions fetching tasks
        var activeUserMissions = await fetchUserMissionsTask;
        var activeGeneralMissions = await fetchGeneralMissionsTask;

        UserMissions = activeUserMissions;
        GeneralMissions = activeGeneralMissions;

        PopulateRewardsUI(activeUserMissions, activeGeneralMissions);
    }

    private void PopulateRewardsUI(List<MissionsUser> userMissions, List<MissionsUser> generalMissions)
    {
        List<MissionsUser> allMissions = new List<MissionsUser>();
        if (userMissions != null)
        {
            allMissions.AddRange(userMissions);
        }
        if (generalMissions != null)
        {
            allMissions.AddRange(generalMissions);
        }

        if (allMissions.Count == 0)
        {
            Debug.LogWarning("[RewardsManager] No missions to display.");
            return;
        }

        // Clean up any existing children in the container
        foreach (Transform child in rewardsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var mission in allMissions)
        {
            Debug.Log($"[RewardsManager] Instantiating mission with ID: {mission.IdMission}");
            InstantiateRewardPrefab(mission);
        }

        rewardsCountText.text = allMissions.Count.ToString();
    }

    private void InstantiateRewardPrefab(MissionsUser mission)
    {
        if (rewardPrefab == null)
        {
            Debug.LogError("Reward Prefab is not assigned!");
            return;
        }

        var instance = Instantiate(rewardPrefab, rewardsContainer);
        Debug.Log($"Instantiated prefab for mission ID: {mission.IdMission} - Parent: {instance.transform.parent.name}");

        instance.SetActive(true);

        var display = instance.GetComponent<RewardsDisplay>();
        if (display != null)
        {
            display.SetRewardData(mission);
            Debug.Log($"[RewardsManager] Reward data set for mission ID: {mission.IdMission}");
        }
        else
        {
            Debug.LogError("RewardsDisplay component is missing from the prefab!");
        }
    }

    public async Task RefreshUserMissions()
    {
        await FetchAndPopulateRewardsUI();
    }

    public async Task RefreshGeneralMissions()
    {
        await FetchAndPopulateRewardsUI();
    }

    public async Task<List<MissionsUser>> SearchActiveUserMissions()
    {
        try
        {
            Debug.Log("[RewardsManager] Searching active user missions...");
            var principal = GetPrincipal();
            var activeUserMissions = await CandidApiManager.Instance.CanisterLogin.SearchActiveUserMissions(principal);
            Debug.Log($"[RewardsManager] Found {activeUserMissions.Count} active user missions.");
            return activeUserMissions;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[RewardsManager] Error searching active user missions: {ex.Message}");
            return new List<MissionsUser>();
        }
    }

    public async Task<List<MissionsUser>> SearchActiveGeneralMissions()
    {
        try
        {
            Debug.Log("[RewardsManager] Searching active general missions...");
            var principal = GetPrincipal();
            var activeGeneralMissions = await CandidApiManager.Instance.CanisterLogin.SearchActiveGeneralMissions(principal);
            Debug.Log($"[RewardsManager] Found {activeGeneralMissions.Count} active general missions.");
            return activeGeneralMissions;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[RewardsManager] Error searching active general missions: {ex.Message}");
            return new List<MissionsUser>();
        }
    }

    public async Task<bool> ClaimUserReward(UnboundedUInt rewardID)
    {
        for (int attempt = 0; attempt < 3; attempt++)
        {
            try
            {
                var result = await CandidApiManager.Instance.CanisterLogin.ClaimUserReward(rewardID);
                if (result.ReturnArg0)
                {
                    Debug.Log("[RewardsManager] User reward claimed successfully.");
                    await RefreshUserMissions(); // Refresh the user missions after claiming
                    var principal = GetPrincipal();
                    await CandidApiManager.Instance.CanisterLogin.CreateUserMission(principal); // Create a new user mission
                    return true;
                }
                Debug.LogWarning("[RewardsManager] Failed to claim user reward.");
                return false;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[RewardsManager] Error claiming user reward (attempt {attempt + 1}/3): {ex.Message}");
                if (attempt < 2) await Task.Delay(1000); // Retry after a delay
            }
        }
        return false;
    }

    public async Task<bool> ClaimGeneralReward(UnboundedUInt rewardID)
    {
        for (int attempt = 0; attempt < 3; attempt++)
        {
            try
            {
                var result = await CandidApiManager.Instance.CanisterLogin.ClaimGeneralReward(rewardID);
                if (result.ReturnArg0)
                {
                    Debug.Log("[RewardsManager] General reward claimed successfully.");
                    await RefreshGeneralMissions(); // Refresh the general missions after claiming
                    return true;
                }
                Debug.LogWarning("[RewardsManager] Failed to claim general reward.");
                return false;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[RewardsManager] Error claiming general reward (attempt {attempt + 1}/3): {ex.Message}");
                if (attempt < 2) await Task.Delay(1000); // Retry after a delay
            }
        }
        return false;
    }

    private Principal GetPrincipal()
    {
        // Obtain the principal from GlobalGameData
        string userPrincipalId = GlobalGameData.Instance.GetUserData().WalletId;
        return Principal.FromText(userPrincipalId);
    }
}
