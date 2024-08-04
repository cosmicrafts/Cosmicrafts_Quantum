using UnityEngine;
using Cosmicrafts.Data;

namespace Cosmicrafts.Managers
{
    public class GameDataManager : MonoBehaviour
    {
        public static GameDataManager Instance { get; private set; }

        public PlayerData playerData;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize player data or load from persistent storage
            LoadPlayerData();
        }

        public void SavePlayerData()
        {
            PlayerPrefs.SetString("PrincipalId", playerData.PrincipalId);
            PlayerPrefs.SetString("Username", playerData.Username);
            PlayerPrefs.SetInt("Level", playerData.Level);
            // Save additional fields as needed
            PlayerPrefs.Save();
            Debug.Log("[GameDataManager] Player data saved.");
        }

        public void LoadPlayerData()
        {
            playerData = new PlayerData(); // Create a new instance of PlayerData

            if (PlayerPrefs.HasKey("PrincipalId"))
            {
                playerData.PrincipalId = PlayerPrefs.GetString("PrincipalId");
                playerData.Username = PlayerPrefs.GetString("Username");
                playerData.Level = PlayerPrefs.GetInt("Level");
                // Load additional fields as needed
                Debug.Log("[GameDataManager] Player data loaded.");
            }
            else
            {
                Debug.LogWarning("[GameDataManager] No player data found. Using default values.");
            }
        }
    }
}
