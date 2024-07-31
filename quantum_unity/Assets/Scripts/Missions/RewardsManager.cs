using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        // Populate rewards UI
        if (MissionManager.Instance != null)
        {
            Debug.Log("[RewardsManager] Populating Missions UI");
            PopulateRewardsUI(MissionManager.Instance.UserMissions, MissionManager.Instance.GeneralMissions);
        }
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
}
