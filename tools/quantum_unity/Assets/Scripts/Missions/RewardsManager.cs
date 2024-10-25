using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Cosmicrafts
{
    public class RewardsManager : MonoBehaviour
    {
        public static RewardsManager Instance { get; private set; }

        public TMP_Text rewardsCountText;
        public GameObject rewardPrefab;
        public Transform rewardsContainer;

        private List<RewardsDisplay> rewardDisplays = new List<RewardsDisplay>();

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

        private async void Start()
        {
            rewardPrefab.SetActive(false);
            
            // Fetch fresh data
            await MissionManager.Instance.FetchUserMissions();
            await MissionManager.Instance.FetchGeneralMissions();
            
            // Populate UI with the newly fetched data
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

            // Clear previous reward displays
            foreach (var display in rewardDisplays)
            {
                Destroy(display.gameObject);
            }
            rewardDisplays.Clear();

            // Create new reward displays
            foreach (var mission in allMissions)
            {
                var instance = Instantiate(rewardPrefab, rewardsContainer);
                var display = instance.GetComponent<RewardsDisplay>();
                if (display != null)
                {
                    display.SetRewardData(mission);
                    display.gameObject.SetActive(true);
                    rewardDisplays.Add(display);
                }
                else
                {
                    Debug.LogError("RewardsDisplay component is missing from the prefab!");
                    Destroy(instance);
                }
            }

            rewardsCountText.text = allMissions.Count.ToString();
        }

        public void RefreshMissions()
        {
            PopulateRewardsUI();
        }
    }
}
