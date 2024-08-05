using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cosmicrafts.Managers;

public class RewardsManager : MonoBehaviour
{
    public static RewardsManager Instance { get; private set; }

    public TMP_Text rewardsCountText;
    public GameObject rewardPrefab;
    public Transform rewardsContainer;

    private void Awake()
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

    private void OnEnable()
    {
        MissionEvents.OnMissionsFetched += PopulateRewardsUI;
    }

    private void OnDisable()
    {
        MissionEvents.OnMissionsFetched -= PopulateRewardsUI;
    }

    private void Start()
    {
        rewardPrefab.SetActive(false);
        PopulateRewardsUI();
    }

    private void PopulateRewardsUI()
    {
        Debug.Log("[RewardsManager] Populating Missions UI");

        List<MissionData> userMissions = MissionManager.Instance.GetUserMissions();
        List<MissionData> generalMissions = MissionManager.Instance.GetGeneralMissions();

        List<MissionData> allMissions = new List<MissionData>();
        allMissions.AddRange(userMissions);
        allMissions.AddRange(generalMissions);

        if (allMissions.Count == 0)
        {
            Debug.LogWarning("[RewardsManager] No missions to display.");
            return;
        }

        int currentChildIndex = 0;

        foreach (var mission in allMissions)
        {
            Debug.Log($"Processing mission ID: {mission.idMission} at index: {currentChildIndex}");

            if (currentChildIndex < rewardsContainer.childCount)
            {
                var child = rewardsContainer.GetChild(currentChildIndex);
                var display = child.GetComponent<RewardsDisplay>();
                if (display != null)
                {
                    Debug.Log($"Updating existing reward display for mission ID: {mission.idMission}");
                    display.SetRewardData(mission);
                    child.gameObject.SetActive(true);
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

    private void InstantiateRewardPrefab(MissionData mission)
    {
        if (rewardPrefab == null)
        {
            Debug.LogError("Reward Prefab is not assigned!");
            return;
        }

        Debug.Log($"Instantiating prefab for mission ID: {mission.idMission}");

        var instance = Instantiate(rewardPrefab, rewardsContainer);
        if (instance == null)
        {
            Debug.LogError("Failed to instantiate prefab.");
            return;
        }

        instance.SetActive(true);

        var display = instance.GetComponent<RewardsDisplay>();
        if (display != null)
        {
            Debug.Log($"Setting reward data for mission ID: {mission.idMission}");
            display.SetRewardData(mission);
        }
        else
        {
            Debug.LogError("RewardsDisplay component is missing from the prefab!");
        }
    }

    public void RefreshMissions()
    {
        PopulateRewardsUI();
    }
}
