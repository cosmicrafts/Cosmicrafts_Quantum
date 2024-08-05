using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
using EdjCase.ICP.Candid.Models;
using CanisterPK.CanisterLogin.Models;
using Candid;
using Cosmicrafts.Data;
using System;
using Cosmicrafts.Managers;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    public List<MissionData> userMissions;
    public List<MissionData> generalMissions;
    public List<MissionData> achievements;

    private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

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
        MissionEvents.RaiseMissionsFetched();
    }

    private async Task WaitForCandidApiManagerInitialization()
    {
        int retryCount = 0;
        int maxRetries = 2;
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

            var user = GameDataManager.Instance.playerData;
            var principal = Principal.FromText(user.PrincipalId);
            var missions = await CandidApiManager.Instance.CanisterLogin.SearchActiveUserMissions(principal);
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

            var user = GameDataManager.Instance.playerData;
            var principal = Principal.FromText(user.PrincipalId);
            var missions = await CandidApiManager.Instance.CanisterLogin.SearchActiveGeneralMissions(principal);
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

    public List<MissionData> GetUserMissions()
    {
        return userMissions;
    }

    public List<MissionData> GetGeneralMissions()
    {
        return generalMissions;
    }

    public async Task<bool> ClaimUserReward(int rewardID)
    {
        try
        {
            var rewardIDUnbounded = UnboundedUInt.FromUInt64((ulong)rewardID);
            var result = await CandidApiManager.Instance.CanisterLogin.ClaimUserReward(rewardIDUnbounded);
            if (result.ReturnArg0)
            {
                Debug.Log("[MissionManager] User reward claimed successfully.");
                var mission = userMissions.Find(m => m.idMission == rewardID);
                if (mission != null)
                {
                    userMissions.Remove(mission);
                    MissionEvents.RaiseMissionsFetched(); // Trigger the event after updating the missions
                }
                return true;
            }
            Debug.LogWarning("[MissionManager] Failed to claim user reward.");
            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[MissionManager] Error claiming user reward: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ClaimGeneralReward(int rewardID)
    {
        try
        {
            var rewardIDUnbounded = UnboundedUInt.FromUInt64((ulong)rewardID);
            var result = await CandidApiManager.Instance.CanisterLogin.ClaimGeneralReward(rewardIDUnbounded);
            if (result.ReturnArg0)
            {
                Debug.Log("[MissionManager] General reward claimed successfully.");
                var mission = generalMissions.Find(m => m.idMission == rewardID);
                if (mission != null)
                {
                    generalMissions.Remove(mission);
                    MissionEvents.RaiseMissionsFetched(); // Trigger the event after updating the missions
                }
                return true;
            }
            Debug.LogWarning("[MissionManager] Failed to claim general reward.");
            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[MissionManager] Error claiming general reward: {ex.Message}");
            return false;
        }
    }
}
