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

        // Fetch active user and general missions concurrently
        var fetchUserMissionsTask = SearchActiveUserMissions();
        var fetchGeneralMissionsTask = SearchActiveGeneralMissions();

        await Task.WhenAll(fetchUserMissionsTask, fetchGeneralMissionsTask);

        UserMissions = fetchUserMissionsTask.Result;
        GeneralMissions = fetchGeneralMissionsTask.Result;
    }

    public async Task RefreshUserMissions()
    {
        Debug.Log("[MissionManager] Refreshing user missions...");
        UserMissions = await SearchActiveUserMissions();
    }

    public async Task RefreshGeneralMissions()
    {
        Debug.Log("[MissionManager] Refreshing general missions...");
        GeneralMissions = await SearchActiveGeneralMissions();
    }

    private async Task<List<MissionsUser>> SearchActiveUserMissions()
    {
        var userMissions = await CandidApiManager.Instance.CanisterLogin.SearchActiveUserMissions();
        return userMissions;
    }

    private async Task<List<MissionsUser>> SearchActiveGeneralMissions()
    {
        var generalMissions = await CandidApiManager.Instance.CanisterLogin.SearchActiveGeneralMissions();
        return generalMissions;
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
}
