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

    public List<MissionData> playerMissions;
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
        FetchAllMissions();
    }

    private async void FetchAllMissions()
    {
        playerMissions = await FetchUserMissions();
        generalMissions = await FetchGeneralMissions();
        Debug.Log($"[MissionManager] Fetched {playerMissions.Count} user missions and {generalMissions.Count} general missions.");
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
            missionData.idMission = mission.IdMission;
            missionData.missionType = mission.MissionType;
            missionData.progress = mission.Progress;
            missionData.rewardAmount = mission.RewardAmount;
            missionData.rewardType = mission.RewardType;
            missionData.startDate = mission.StartDate;
            missionData.total = mission.Total;
            missionDataList.Add(missionData);

            Debug.Log($"[MissionManager] Converted mission: {mission.IdMission}");
        }
        return missionDataList;
    }
}
