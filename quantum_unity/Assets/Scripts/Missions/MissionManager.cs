using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using EdjCase.ICP.Candid.Models;
using CanisterPK.CanisterLogin.Models;
using Cosmicrafts.Managers;
using Candid;
using System;
using Cosmicrafts.Data;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    public List<MissionData> userMissions;
    public List<MissionData> generalMissions;
    public List<MissionData> achievements;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Fetch missions when the script awakens
        InitializeAndFetchMissions();
    }

    private async void InitializeAndFetchMissions()
    {
        await WaitForCandidApiManagerInitialization();
        await FetchAllMissions();
    }

    private async Task WaitForCandidApiManagerInitialization()
    {
        int retryCount = 0;
        int maxRetries = 10;
        while ((CandidApiManager.Instance == null || CandidApiManager.Instance.CanisterLogin == null) && retryCount < maxRetries)
        {
            Debug.LogWarning("[MissionManager] Waiting for CandidApiManager.Instance and CanisterLogin...");
            await Task.Delay(100); // Wait for 100 milliseconds before retrying
            retryCount++;
        }

        if (CandidApiManager.Instance == null)
        {
            Debug.LogError("[MissionManager] CandidApiManager.Instance is still null after retries.");
        }
        else if (CandidApiManager.Instance.CanisterLogin == null)
        {
            Debug.LogError("[MissionManager] CandidApiManager.Instance.CanisterLogin is still null after retries.");
        }
    }

    private async Task FetchAllMissions()
    {
        try
        {
            Debug.Log("[MissionManager] Fetching all missions...");

            // Start fetching both user and general missions simultaneously
            var fetchUserMissionsTask = FetchUserMissions();
            var fetchGeneralMissionsTask = FetchGeneralMissions();

            // Wait for both tasks to complete
            await Task.WhenAll(fetchUserMissionsTask, fetchGeneralMissionsTask);

            // Assign the results
            userMissions = fetchUserMissionsTask.Result;
            generalMissions = fetchGeneralMissionsTask.Result;

            Debug.Log($"[MissionManager] Fetched {userMissions.Count} user missions and {generalMissions.Count} general missions.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[MissionManager] Error fetching missions: {ex.Message}");
        }
    }

    private async Task<List<MissionData>> FetchUserMissions()
    {
        try
        {
            Debug.Log("[MissionManager] Fetching user missions...");

            var missions = await CandidApiManager.Instance.CanisterLogin.GetUserMissions();
            Debug.Log($"[MissionManager] Found {missions.Count} user missions.");
            return ConvertToMissionDataList(missions);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[MissionManager] Error fetching user missions: {ex.Message}");
            return new List<MissionData>();
        }
    }

    private async Task<List<MissionData>> FetchGeneralMissions()
    {
        try
        {
            Debug.Log("[MissionManager] Fetching general missions...");

            var missions = await CandidApiManager.Instance.CanisterLogin.GetGeneralMissions();
            Debug.Log($"[MissionManager] Found {missions.Count} general missions.");
            return ConvertToMissionDataList(missions);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[MissionManager] Error fetching general missions: {ex.Message}");
            return new List<MissionData>();
        }
    }

    private List<MissionData> ConvertToMissionDataList(List<MissionsUser> missions)
    {
        List<MissionData> missionDataList = new List<MissionData>();
        foreach (var mission in missions)
        {
            MissionData missionData = ScriptableObject.CreateInstance<MissionData>();
            missionData.expiration = mission.Expiration;
            missionData.finishDate = mission.FinishDate;
            missionData.finished = mission.Finished;
            missionData.idMission = (int)mission.IdMission;
            missionData.missionType = mission.MissionType;
            missionData.progress = (int)mission.Progress;
            missionData.rewardAmount = (int)mission.RewardAmount;
            missionData.rewardType = mission.RewardType;
            missionData.startDate = mission.StartDate;
            missionData.total = (int)mission.Total;
            missionDataList.Add(missionData);

            Debug.Log($"[MissionManager] Converted mission: {mission.IdMission}, Reward Amount: {mission.RewardAmount}");
        }
        return missionDataList;
    }
}
