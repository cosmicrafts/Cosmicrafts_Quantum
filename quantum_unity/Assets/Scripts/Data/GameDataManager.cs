using UnityEngine;
using Cosmicrafts.Data;
using System.Collections.Generic;

namespace Cosmicrafts.Managers
{
    public class GameDataManager : MonoBehaviour
    {
        public static GameDataManager Instance { get; private set; }

        public PlayerData playerData;
        public List<MissionData> playerMissions;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadPlayerData();
        }

        public void SavePlayerData()
        {
            // Implement saving logic
            PlayerPrefs.Save();
            Debug.Log("[GameDataManager] Player data saved.");
        }

        public void LoadPlayerData()
        {
            playerData = new PlayerData();
            playerMissions = new List<MissionData>();

            if (PlayerPrefs.HasKey("PrincipalId"))
            {
                // Implement loading logic
                Debug.Log("[GameDataManager] Player data loaded.");
            }
            else
            {
                Debug.LogWarning("[GameDataManager] No player data found. Using default values.");
            }
        }
    }
}
