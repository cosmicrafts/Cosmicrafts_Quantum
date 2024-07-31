using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Threading.Tasks;
using CanisterPK.CanisterLogin;
using CanisterPK.CanisterLogin.Models;
using EdjCase.ICP.Candid.Models;

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

        // Initialize MissionManager if it doesn't exist
        if (MissionManager.Instance == null)
        {
            GameObject missionManagerObject = new GameObject("MissionManager");
            DontDestroyOnLoad(missionManagerObject);
            MissionManager missionManager = missionManagerObject.AddComponent<MissionManager>();

            await missionManager.InitializeMissions();
        }

        // Wait for MissionManager to initialize and fetch missions
        await WaitForMissionManagerInitialization();

        // Populate rewards UI
        if (MissionManager.Instance != null)
        {
            Debug.Log("[RewardsManager] Populating User Missions UI");
            PopulateRewardsUI(MissionManager.Instance.UserMissions, "User");

            Debug.Log("[RewardsManager] Populating General Missions UI");
            PopulateRewardsUI(MissionManager.Instance.GeneralMissions, "General");
        }
    }

    private async Task WaitForMissionManagerInitialization()
    {
        // Ensure MissionManager is initialized and has fetched the missions
        while (MissionManager.Instance == null || MissionManager.Instance.UserMissions == null || MissionManager.Instance.GeneralMissions == null)
        {
            Debug.LogWarning("Waiting for MissionManager to initialize...");
            await Task.Delay(500); // Wait for 500ms before checking again
        }
    }

    private void PopulateRewardsUI(List<MissionsUser> missions, string missionType)
    {
        if (missions == null)
        {
            Debug.LogWarning($"[RewardsManager] {missionType} Missions list is null.");
            return;
        }

        if (missions.Count == 0)
        {
            Debug.LogWarning($"[RewardsManager] No {missionType} missions to display.");
            return;
        }

        // Clean up any existing children in the container
        foreach (Transform child in rewardsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var mission in missions)
        {
            Debug.Log($"[RewardsManager] Instantiating {missionType} mission with ID: {mission.IdMission}");
            InstantiateRewardPrefab(mission);
        }

        rewardsCountText.text = missions.Count.ToString();
    }

    private void InstantiateRewardPrefab(MissionsUser mission)
    {
        if (rewardPrefab == null)
        {
            Debug.LogError("Reward Prefab is not assigned!");
            return;
        }

        var instance = Instantiate(rewardPrefab, rewardsContainer);
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
}
