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

        private void Start()
        {
            rewardPrefab.SetActive(false);
            ObjectPoolManager.Instance.CreatePool(rewardPrefab, 25);
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

            // Ensure we have enough reward displays
            while (rewardDisplays.Count < allMissions.Count)
            {
                var instance = ObjectPoolManager.Instance.GetObject(rewardPrefab, rewardsContainer, Vector3.zero, Quaternion.identity);
                var display = instance.GetComponent<RewardsDisplay>();
                if (display != null)
                {
                    rewardDisplays.Add(display);
                }
                else
                {
                    Debug.LogError("RewardsDisplay component is missing from the prefab!");
                    ObjectPoolManager.Instance.ReturnObject(instance);
                }
            }

            // Update existing reward displays with new data
            for (int i = 0; i < allMissions.Count; i++)
            {
                rewardDisplays[i].SetRewardData(allMissions[i]);
                rewardDisplays[i].gameObject.SetActive(true);
            }

            // Deactivate and return any extra reward displays to the pool
            for (int i = allMissions.Count; i < rewardDisplays.Count; i++)
            {
                rewardDisplays[i].gameObject.SetActive(false);
                ObjectPoolManager.Instance.ReturnObject(rewardDisplays[i].gameObject);
            }

            rewardsCountText.text = allMissions.Count.ToString();
        }

        public void RefreshMissions()
        {
            PopulateRewardsUI();
        }
    }
}