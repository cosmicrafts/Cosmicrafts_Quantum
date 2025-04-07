using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Cosmicrafts.ICP
{
    /// <summary>
    /// Helper class for asynchronous local storage operations
    /// </summary>
    public static class AsyncLocalStorage
    {
        private const string KEY_PREFIX = "Cosmicrafts_";
        
        /// <summary>
        /// Saves data to local storage
        /// </summary>
        public static async UniTask SaveDataAsync(string key, string value)
        {
            // Add a small delay to simulate async operation
            await UniTask.DelayFrame(1);
            
            // Save the data with a prefix to avoid conflicts
            PlayerPrefs.SetString(KEY_PREFIX + key, value);
            PlayerPrefs.Save();
            
            Debug.Log($"[AsyncLocalStorage] Saved data for key: {key}");
        }
        
        /// <summary>
        /// Loads data from local storage
        /// </summary>
        public static async UniTask<string> LoadDataAsync(string key)
        {
            // Add a small delay to simulate async operation
            await UniTask.DelayFrame(1);
            
            // Try to get the data with our prefix
            string result = PlayerPrefs.GetString(KEY_PREFIX + key, null);
            
            if (string.IsNullOrEmpty(result))
            {
                Debug.Log($"[AsyncLocalStorage] No data found for key: {key}");
                return null;
            }
            
            Debug.Log($"[AsyncLocalStorage] Loaded data for key: {key}");
            return result;
        }
        
        /// <summary>
        /// Deletes data from local storage
        /// </summary>
        public static void DeleteData(string key)
        {
            PlayerPrefs.DeleteKey(KEY_PREFIX + key);
            PlayerPrefs.Save();
            
            Debug.Log($"[AsyncLocalStorage] Deleted data for key: {key}");
        }
        
        /// <summary>
        /// Checks if data exists in local storage
        /// </summary>
        public static bool HasData(string key)
        {
            return PlayerPrefs.HasKey(KEY_PREFIX + key);
        }
        
        /// <summary>
        /// Clears all Cosmicrafts data from local storage
        /// </summary>
        public static void ClearAllData()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            
            Debug.Log("[AsyncLocalStorage] Cleared all data");
        }
    }
} 