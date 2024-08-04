using UnityEngine;
using Cosmicrafts.Data;
using Candid;
using System.Threading.Tasks;
using EdjCase.ICP.Candid.Models;
using System.Numerics;

namespace Cosmicrafts.Managers
{
    public class GameDataManager : MonoBehaviour
    {
        public static GameDataManager Instance { get; private set; }

        public PlayerData playerData;
        private bool isUpdatingAvatar = false; // Flag to prevent multiple updates

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
            PlayerPrefs.SetInt("AvatarID", playerData.AvatarID); // Save AvatarID
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
                playerData.AvatarID = PlayerPrefs.GetInt("AvatarID", 1);
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
