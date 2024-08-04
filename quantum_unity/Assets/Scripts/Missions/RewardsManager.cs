using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using EdjCase.ICP.Candid.Models;
using Candid;
using CanisterPK.CanisterLogin.Models;
using Cosmicrafts.Data;

public class RewardsManager : MonoBehaviour
{
    public static RewardsManager Instance { get; private set; }

    public TMP_Text rewardsCountText;
    public GameObject rewardPrefab;
    public Transform rewardsContainer;

    public List<MissionsUser> UserMissions { get; private set; }
    public List<MissionsUser> GeneralMissions { get; private set; }

    public event Action<MissionsUser> OnMissionClaimed;
    public event Action<MissionsUser> OnMissionUpdated;
    public event Action<MissionsUser> OnMissionRemoved;

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
        await FetchAndPopulateRewardsUI();
    }

    private async Task FetchAndPopulateRewardsUI()
    {
        Debug.Log("[RewardsManager] Populating Missions UI");

        var fetchUserMissionsTask = SearchActiveUserMissions();
        var fetchGeneralMissionsTask = SearchActiveGeneralMissions();

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

        int currentChildIndex = 0;

        foreach (var mission in allMissions)
        {
            Debug.Log($"[RewardsManager] Processing mission with ID: {mission.IdMission}");

            if (currentChildIndex < rewardsContainer.childCount)
            {
                var child = rewardsContainer.GetChild(currentChildIndex);
                var display = child.GetComponent<RewardsDisplay>();
                if (display != null)
                {
                    display.SetRewardData(mission);
                    child.gameObject.SetActive(true);
                    Debug.Log($"[RewardsManager] Updated reward for mission ID: {mission.IdMission}");
                }
                else
                {
                    Debug.LogError("RewardsDisplay component is missing from the child prefab!");
                }
            }
            else
            {
                InstantiateRewardPrefab(mission);
            }

            currentChildIndex++;
        }

        for (int i = currentChildIndex; i < rewardsContainer.childCount; i++)
        {
            rewardsContainer.GetChild(i).gameObject.SetActive(false);
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
            var principal = await GetPrincipalAsync();
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
            var principal = await GetPrincipalAsync();
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

    private async Task<Principal> GetPrincipalAsync()
    {
        var userData = await AsyncDataManager.LoadPlayerDataAsync();
        if (userData == null)
        {
            Debug.LogError("Failed to load player data.");
            return null;
        }
        return Principal.FromText(userData.PrincipalId);
    }

    private void OnEnable()
    {
        OnMissionClaimed += HandleMissionClaimed;
        OnMissionRemoved += HandleMissionRemoved;
    }

    private void OnDisable()
    {
        OnMissionClaimed -= HandleMissionClaimed;
        OnMissionRemoved -= HandleMissionRemoved;
    }

    private void HandleMissionClaimed(MissionsUser mission)
    {
        foreach (Transform child in rewardsContainer)
        {
            var display = child.GetComponent<RewardsDisplay>();
            if (display != null && display.rewardData == mission)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    private void HandleMissionRemoved(MissionsUser mission)
    {
        foreach (Transform child in rewardsContainer)
        {
            var display = child.GetComponent<RewardsDisplay>();
            if (display != null && display.rewardData == mission)
            {
                display.SetRewardData(null);
                child.gameObject.SetActive(false);
                break;
            }
        }
    }

    public async Task<bool> ClaimUserReward(UnboundedUInt rewardID)
    {
        try
        {
            var result = await CandidApiManager.Instance.CanisterLogin.ClaimUserReward(rewardID);
            if (result.ReturnArg0)
            {
                Debug.Log("[RewardsManager] User reward claimed successfully.");
                var mission = UserMissions.Find(m => m.IdMission == rewardID);
                if (mission != null)
                {
                    UserMissions.Remove(mission);
                    OnMissionClaimed?.Invoke(mission);
                }

                var principal = await GetPrincipalAsync();
                var createMissionTask = await CandidApiManager.Instance.CanisterLogin.CreateUserMission(principal);
                if (createMissionTask.ReturnArg0)
                {
                    Debug.Log("[RewardsManager] New user mission created successfully.");
                    await FetchAndPopulateRewardsUI();
                }
                else
                {
                    Debug.LogWarning("[RewardsManager] Failed to create new user mission.");
                }

                return true;
            }
            Debug.LogWarning("[RewardsManager] Failed to claim user reward.");
            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[RewardsManager] Error claiming user reward: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ClaimGeneralReward(UnboundedUInt rewardID)
    {
        try
        {
            var result = await CandidApiManager.Instance.CanisterLogin.ClaimGeneralReward(rewardID);
            if (result.ReturnArg0)
            {
                Debug.Log("[RewardsManager] General reward claimed successfully.");
                var mission = GeneralMissions.Find(m => m.IdMission == rewardID);
                if (mission != null)
                {
                    GeneralMissions.Remove(mission);
                    OnMissionClaimed?.Invoke(mission);
                }
                return true;
            }
            Debug.LogWarning("[RewardsManager] Failed to claim general reward.");
            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[RewardsManager] Error claiming general reward: {ex.Message}");
            return false;
        }
    }
}
