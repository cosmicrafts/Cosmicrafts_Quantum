using UnityEngine;
using System.Runtime.InteropServices;

namespace Cosmicrafts.ICP
{
    /// <summary>
    /// Target for JavaScript communications using the unity-webgl library.
    /// This script handles events that will be called from JavaScript.
    /// </summary>
    public class WebGLBridgeTarget : MonoBehaviour
    {
        // Reference for unity-webgl library to call
        private static WebGLBridgeTarget _instance;
        
        // JavaScript interop methods (must be extern)
        [DllImport("__Internal")]
        private static extern void DispatchLoginResult(string result);
        
        [DllImport("__Internal")]
        private static extern void DispatchLogoutResult(string result);
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            Debug.Log("[WebGLBridgeTarget] Initialized");
        }
        
        /// <summary>
        /// Called by JavaScript when an identity is received
        /// </summary>
        public void ReceiveIdentity(string identityJson)
        {
            Debug.Log("[WebGLBridgeTarget] Received identity from JavaScript");
            
            if (ICPManager.Instance != null)
            {
                ICPManager.Instance.CreateAgentFromWeb(identityJson).Forget();
            }
            else
            {
                Debug.LogError("[WebGLBridgeTarget] ICPManager.Instance is null");
            }
        }
        
        /// <summary>
        /// Called by JavaScript to perform a test login (for development purposes)
        /// </summary>
        public void TestLogin()
        {
            Debug.Log("[WebGLBridgeTarget] Test login requested from JavaScript");
            
            if (ICPManager.Instance != null)
            {
                ICPManager.Instance.CreateRandomAgentForTesting().Forget();
            }
            else
            {
                Debug.LogError("[WebGLBridgeTarget] ICPManager.Instance is null");
            }
        }
        
        /// <summary>
        /// Called by JavaScript to logout
        /// </summary>
        public void Logout()
        {
            Debug.Log("[WebGLBridgeTarget] Logout requested from JavaScript");
            
            if (ICPManager.Instance != null)
            {
                ICPManager.Instance.Logout();
                
                try
                {
                    if (Application.platform == RuntimePlatform.WebGLPlayer)
                    {
                        DispatchLogoutResult("success");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[WebGLBridgeTarget] Error notifying JS of logout: {e.Message}");
                }
            }
            else
            {
                Debug.LogError("[WebGLBridgeTarget] ICPManager.Instance is null");
            }
        }
        
        /// <summary>
        /// Called by JavaScript to check if the user is logged in
        /// </summary>
        public string GetLoginStatus()
        {
            if (ICPManager.Instance != null)
            {
                bool isLoggedIn = ICPManager.Instance.IsLoggedIn;
                string principalId = ICPManager.Instance.PrincipalId ?? "";
                
                return $"{{\"isLoggedIn\":{isLoggedIn.ToString().ToLower()},\"principalId\":\"{principalId}\"}}";
            }
            
            return "{\"isLoggedIn\":false,\"principalId\":\"\"}";
        }
        
        /// <summary>
        /// Called by the LoginUI to notify login completion
        /// </summary>
        public void LoginComplete(string username)
        {
            Debug.Log($"[WebGLBridgeTarget] Login complete for user: {username}");
            
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                try
                {
                    // Create a JSON result to send back to JavaScript
                    string result = $"{{\"success\":true,\"username\":\"{username}\",\"principalId\":\"{ICPManager.Instance.PrincipalId}\"}}";
                    DispatchLoginResult(result);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[WebGLBridgeTarget] Error notifying JS of login completion: {e.Message}");
                }
            }
        }
        
        /// <summary>
        /// Called by JavaScript to get player data
        /// </summary>
        public string GetPlayerData()
        {
            if (PlayerManager.Instance != null && PlayerManager.Instance.PlayerData != null)
            {
                var playerData = PlayerManager.Instance.PlayerData;
                return JsonUtility.ToJson(playerData);
            }
            
            return "{}";
        }
    }
} 