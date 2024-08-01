using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using CanisterPK.CanisterLogin.Models;
using EdjCase.ICP.Candid.Models;
using Newtonsoft.Json;
using Candid;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    // Store missions here
    public List<MissionsUser> UserMissions { get; private set; }
    public List<MissionsUser> GeneralMissions { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.LogWarning("[MissionManager] Instance already exists. Destroying duplicate.");
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("[MissionManager] Awake - MissionManager instance initialized.");
    }

    public async Task InitializeMissions()
    {
        Debug.Log("[MissionManager] Initializing missions");

        // Fetch user and general missions concurrently
        var fetchUserMissionsTask = FetchUserMissions();
        var fetchGeneralMissionsTask = FetchGeneralMissions();

        await Task.WhenAll(fetchUserMissionsTask, fetchGeneralMissionsTask);

        UserMissions = fetchUserMissionsTask.Result;
        GeneralMissions = fetchGeneralMissionsTask.Result;

        Debug.Log($"[MissionManager] Fetched {UserMissions.Count} user missions.");
        Debug.Log($"[MissionManager] Fetched {GeneralMissions.Count} general missions.");
    }

    public async Task RefreshUserMissions()
    {
        Debug.Log("[MissionManager] Refreshing user missions...");
        UserMissions = await FetchUserMissions();
        Debug.Log($"[MissionManager] Refreshed user missions: {UserMissions.Count}");
    }

    public async Task RefreshGeneralMissions()
    {
        Debug.Log("[MissionManager] Refreshing general missions...");
        GeneralMissions = await FetchGeneralMissions();
        Debug.Log($"[MissionManager] Refreshed general missions: {GeneralMissions.Count}");
    }

    private async Task<List<MissionsUser>> FetchUserMissions()
    {
        var userMissions = await CandidApiManager.Instance.CanisterLogin.GetUserMissions();
        LogRawResponse(userMissions);
        return userMissions;
    }

    private async Task<List<MissionsUser>> FetchGeneralMissions()
    {
        var generalMissions = await CandidApiManager.Instance.CanisterLogin.GetGeneralMissions();
        LogRawResponse(generalMissions);
        return generalMissions;
    }

    private void LogRawResponse<T>(T response)
    {
        string rawResponse = JsonConvert.SerializeObject(response, Formatting.Indented);
        Debug.Log($"[MissionManager] Raw response: {rawResponse}");
    }

    public async Task<bool> ClaimUserReward(UnboundedUInt rewardID)
    {
        var result = await CandidApiManager.Instance.CanisterLogin.ClaimUserReward(rewardID);
        if (result.ReturnArg0)
        {
            Debug.Log("[MissionManager] User reward claimed successfully.");
            await RefreshUserMissions(); // Refresh the user missions after claiming
            return true;
        }
        Debug.LogWarning("[MissionManager] Failed to claim user reward.");
        return false;
    }

    public async Task<bool> ClaimGeneralReward(UnboundedUInt rewardID)
    {
        var result = await CandidApiManager.Instance.CanisterLogin.ClaimGeneralReward(rewardID);
        if (result.ReturnArg0)
        {
            Debug.Log("[MissionManager] General reward claimed successfully.");
            await RefreshGeneralMissions(); // Refresh the general missions after claiming
            return true;
        }
        Debug.LogWarning("[MissionManager] Failed to claim general reward.");
        return false;
    }

    public async Task<List<MissionsUser>> SearchActiveUserMissions()
    {
        Debug.Log("[MissionManager] Searching active user missions...");
        var activeUserMissions = await CandidApiManager.Instance.CanisterLogin.SearchActiveUserMissions();
        Debug.Log($"[MissionManager] Found {activeUserMissions.Count} active user missions.");
        return activeUserMissions;
    }

    public async Task<List<MissionsUser>> SearchActiveGeneralMissions()
    {
        Debug.Log("[MissionManager] Searching active general missions...");
        var activeGeneralMissions = await CandidApiManager.Instance.CanisterLogin.SearchActiveGeneralMissions();
        Debug.Log($"[MissionManager] Found {activeGeneralMissions.Count} active general missions.");
        return activeGeneralMissions;
    }
}