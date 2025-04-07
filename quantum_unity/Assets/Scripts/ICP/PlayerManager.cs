using System;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Cosmicrafts.MainCanister.Models;

namespace Cosmicrafts.ICP
{
    /// <summary>
    /// Manages player data after authentication with ICP
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }
        
        // Player data
        public PlayerData PlayerData { get; private set; }
        
        // Events
        public event Action<PlayerData> OnPlayerDataLoaded;
        public event Action<string> OnPlayerDataError;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("[PlayerManager] Instance already exists. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Initialize player data
            PlayerData = new PlayerData();
            
            Debug.Log("[PlayerManager] Instance initialized");
        }
        
        private void Start()
        {
            // Subscribe to ICP Manager events
            if (ICPManager.Instance != null)
            {
                ICPManager.Instance.OnLoginSuccessful += OnLoginSuccessful;
                ICPManager.Instance.OnICPInitialized += OnICPInitialized;
            }
            else
            {
                Debug.LogError("[PlayerManager] ICPManager.Instance is null in Start");
            }
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (ICPManager.Instance != null)
            {
                ICPManager.Instance.OnLoginSuccessful -= OnLoginSuccessful;
                ICPManager.Instance.OnICPInitialized -= OnICPInitialized;
            }
        }
        
        /// <summary>
        /// Called when ICP login is successful
        /// </summary>
        private void OnLoginSuccessful(string principalId)
        {
            Debug.Log($"[PlayerManager] Login successful. Principal ID: {principalId}");
            
            // Store principal ID in player data
            PlayerData.PrincipalId = principalId;
            
            // Load player data from canister
            LoadPlayerDataAsync().Forget();
        }
        
        /// <summary>
        /// Called when ICP is initialized
        /// </summary>
        private void OnICPInitialized()
        {
            Debug.Log("[PlayerManager] ICP initialized");
        }
        
        /// <summary>
        /// Loads player data from the canister
        /// </summary>
        public async UniTaskVoid LoadPlayerDataAsync()
        {
            Debug.Log("[PlayerManager] Loading player data from canister");
            
            if (ICPManager.Instance == null || ICPManager.Instance.MainCanister == null)
            {
                Debug.LogError("[PlayerManager] ICPManager.Instance or MainCanister is null");
                OnPlayerDataError?.Invoke("ICPManager not initialized");
                return;
            }
            
            try
            {
                // Get player data from canister
                var playerInfo = await ICPManager.Instance.MainCanister.GetPlayer();
                
                if (playerInfo.HasValue)
                {
                    // Player exists
                    var player = playerInfo.ValueOrDefault;
                    
                    // Update player data
                    UpdatePlayerData(player);
                    
                    // Save player data to local storage
                    SavePlayerData();
                    
                    Debug.Log($"[PlayerManager] Player data loaded. Username: {PlayerData.Username}, Level: {PlayerData.Level}");
                    OnPlayerDataLoaded?.Invoke(PlayerData);
                }
                else
                {
                    // Player doesn't exist yet
                    Debug.LogWarning("[PlayerManager] Player data not found on canister");
                    OnPlayerDataError?.Invoke("Player not found");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[PlayerManager] Error loading player data: {ex.Message}");
                OnPlayerDataError?.Invoke($"Error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Updates player data from canister data
        /// </summary>
        private void UpdatePlayerData(Player player)
        {
            PlayerData.Level = (int)player.Level;
            PlayerData.Username = player.Username;
            PlayerData.AvatarID = (int)player.Avatar;
            PlayerData.Description = player.Description;
            PlayerData.Elo = player.Elo;
            PlayerData.RegistrationDate = (long)player.RegistrationDate.ToBigInteger();
            PlayerData.IsLoggedIn = true;
        }
        
        /// <summary>
        /// Saves player data to local storage
        /// </summary>
        public void SavePlayerData()
        {
            string playerDataJson = JsonUtility.ToJson(PlayerData);
            AsyncLocalStorage.SaveDataAsync("playerData", playerDataJson).Forget();
            Debug.Log("[PlayerManager] Player data saved to local storage");
        }
        
        /// <summary>
        /// Loads player data from local storage
        /// </summary>
        public async UniTask<bool> LoadPlayerDataFromStorageAsync()
        {
            string playerDataJson = await AsyncLocalStorage.LoadDataAsync("playerData");
            if (!string.IsNullOrEmpty(playerDataJson))
            {
                try
                {
                    PlayerData = JsonUtility.FromJson<PlayerData>(playerDataJson);
                    Debug.Log($"[PlayerManager] Player data loaded from storage. Username: {PlayerData.Username}");
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[PlayerManager] Error parsing player data JSON: {ex.Message}");
                }
            }
            
            Debug.LogWarning("[PlayerManager] No player data found in storage");
            return false;
        }
        
        /// <summary>
        /// Clears player data
        /// </summary>
        public void ClearPlayerData()
        {
            PlayerData = new PlayerData();
            AsyncLocalStorage.DeleteData("playerData");
            Debug.Log("[PlayerManager] Player data cleared");
        }
    }
    
    /// <summary>
    /// Player data structure
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        public string PrincipalId;
        public string Username;
        public int Level;
        public int AvatarID;
        public string Description;
        public double Elo;
        public bool IsLoggedIn;
        public long RegistrationDate;
        
        // Game-specific data
        public int Stardust;  // In-game currency
        
        // Add more game-specific data as needed
        
        public PlayerData()
        {
            PrincipalId = "";
            Username = "Guest";
            Level = 1;
            AvatarID = 0;
            Description = "";
            Elo = 1000;
            IsLoggedIn = false;
            RegistrationDate = 0;
            Stardust = 0;
        }
    }
} 