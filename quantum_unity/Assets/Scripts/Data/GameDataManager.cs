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
            PlayerPrefs.Save();
            Debug.Log("[GameDataManager] Player data saved.");
        }

        public void LoadPlayerData()
        {
            playerData = new PlayerData();

            if (PlayerPrefs.HasKey("PrincipalId"))
            {

                Debug.Log("[GameDataManager] Player data loaded.");
            }
            else
            {
                Debug.LogWarning("[GameDataManager] No player data found. Using default values.");
            }
        }
    }
}
