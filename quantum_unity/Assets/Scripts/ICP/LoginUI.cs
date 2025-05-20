using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

namespace Cosmicrafts.ICP
{
    /// <summary>
    /// Handles the Login UI functionality and communication with the Vue frontend
    /// </summary>
    public class LoginUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] public GameObject loginPanel;
        [SerializeField] public GameObject loadingPanel;
        [SerializeField] public GameObject mainMenuPanel;
        [SerializeField] public Button loginButton;
        [SerializeField] public Button testLoginButton;
        [SerializeField] public TMP_Text principalIdText;
        [SerializeField] public TMP_Text statusText;
        [SerializeField] public TMP_Text usernameText;
        
        private async void Awake()
        {
            Debug.Log("[LoginUI] Awake called");
            
            // Hide all panels initially
            if (loginPanel) loginPanel.SetActive(false);
            if (loadingPanel) loadingPanel.SetActive(false);
            if (mainMenuPanel) mainMenuPanel.SetActive(false);
            
            // Setup button click listeners
            if (loginButton) loginButton.onClick.AddListener(OnLoginButtonClicked);
            if (testLoginButton) testLoginButton.onClick.AddListener(OnTestLoginButtonClicked);
            
            // Show loading by default
            ShowLoading("Initializing...");

            await WaitForCandidApiInitialization();
            await InitializeLogin();
        }
        
        private async UniTask WaitForCandidApiInitialization()
        {
            Debug.Log("[LoginUI] Waiting for ICPManager initialization...");

            while (ICPManager.Instance == null || ICPManager.Instance.MainCanister == null)
            {
                await UniTask.Yield();
            }

            Debug.Log("[LoginUI] ICPManager initialized");
        }

        private async UniTask InitializeLogin()
        {
            // Check if we should try auto-login
            if (ICPManager.Instance != null)
            {
                await ICPManager.Instance.TryAutoLogin();
            }
            else
            {
                Debug.LogError("[LoginUI] ICPManager.Instance is null in InitializeLogin");
                ShowLogin("Error: ICP Manager not found");
            }
        }
        
        private void Start()
        {
            // Subscribe to ICP Manager events
            if (ICPManager.Instance != null)
            {
                ICPManager.Instance.OnLoginSuccessful += OnLoginSuccessful;
                ICPManager.Instance.OnLoginFailed += OnLoginFailed;
                ICPManager.Instance.OnICPInitialized += OnICPInitialized;
            }
            else
            {
                Debug.LogError("[LoginUI] ICPManager.Instance is null in Start");
                ShowLogin("Error: ICP Manager not found");
            }
            
            // Subscribe to PlayerManager events
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.OnPlayerDataLoaded += OnPlayerDataLoaded;
                PlayerManager.Instance.OnPlayerDataError += OnPlayerDataError;
            }
            else
            {
                Debug.LogError("[LoginUI] PlayerManager.Instance is null in Start");
            }
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (ICPManager.Instance != null)
            {
                ICPManager.Instance.OnLoginSuccessful -= OnLoginSuccessful;
                ICPManager.Instance.OnLoginFailed -= OnLoginFailed;
                ICPManager.Instance.OnICPInitialized -= OnICPInitialized;
            }
            
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.OnPlayerDataLoaded -= OnPlayerDataLoaded;
                PlayerManager.Instance.OnPlayerDataError -= OnPlayerDataError;
            }
            
            // Remove button listeners
            if (loginButton) loginButton.onClick.RemoveListener(OnLoginButtonClicked);
            if (testLoginButton) testLoginButton.onClick.RemoveListener(OnTestLoginButtonClicked);
        }
        
        /// <summary>
        /// Called when the login button is clicked
        /// </summary>
        private void OnLoginButtonClicked()
        {
            Debug.Log("[LoginUI] Login button clicked");
            ShowLoading("Requesting login from frontend...");
            
            // In WebGL, use the WebGLBridge to request login
            if (WebGLBridge.Instance != null)
            {
                WebGLBridge.Instance.RequestLoginFromJavaScript();
            }
            else
            {
                Debug.LogError("[LoginUI] WebGLBridge.Instance is null");
                ShowLogin("Error: WebGL Bridge not found");
            }
        }
        
        /// <summary>
        /// Called when the test login button is clicked
        /// </summary>
        private void OnTestLoginButtonClicked()
        {
            Debug.Log("[LoginUI] Test login button clicked");
            ShowLoading("Generating test identity...");
            
            // Use ICPManager to create a random agent for testing
            if (ICPManager.Instance != null)
            {
                UniTask.Void(async () => 
                {
                    await ICPManager.Instance.CreateRandomAgentForTesting();
                });
            }
            else
            {
                Debug.LogError("[LoginUI] ICPManager.Instance is null in OnTestLoginButtonClicked");
                ShowLogin("Error: ICP Manager not found");
            }
        }
        
        /// <summary>
        /// Called when login is successful
        /// </summary>
        private void OnLoginSuccessful(string principalId)
        {
            Debug.Log($"[LoginUI] Login successful. Principal ID: {principalId}");
            
            // Update UI with principal ID
            if (principalIdText != null)
            {
                // Shorten the principal ID for display
                string displayId = principalId;
                if (displayId.Length > 15)
                {
                    displayId = displayId.Substring(0, 5) + "..." + displayId.Substring(displayId.Length - 5);
                }
                principalIdText.text = "Principal ID: " + displayId;
            }
            
            ShowLoading("Loading player data...");
        }
        
        /// <summary>
        /// Called when player data is loaded
        /// </summary>
        private void OnPlayerDataLoaded(PlayerData playerData)
        {
            Debug.Log($"[LoginUI] Player data loaded. Username: {playerData.Username}");
            
            // Update UI with player data
            if (usernameText != null)
            {
                usernameText.text = "Username: " + playerData.Username;
            }
            
            // Show main menu
            ShowMainMenu();
            
            // Notify WebGL frontend that login is complete
            if (Application.platform == RuntimePlatform.WebGLPlayer && WebGLBridge.Instance != null)
            {
                // Use JS functions here to notify Vue that login is complete
                Debug.Log("[LoginUI] Notifying WebGL frontend that login is complete");
                
                // We'll send a message to a GameObject in the scene that can be listened to from JS
                SendMessageToJavaScript("LoginComplete", playerData.Username);
            }
        }
        
        /// <summary>
        /// Called when there's an error loading player data
        /// </summary>
        private void OnPlayerDataError(string errorMessage)
        {
            Debug.LogError($"[LoginUI] Player data error: {errorMessage}");
            
            if (errorMessage.Contains("Player not found"))
            {
                // Player needs to be registered
                ShowMainMenu(); // For now just show main menu, later we can add registration UI
            }
            else
            {
                // Show login with error
                ShowLogin($"Error: {errorMessage}");
            }
        }
        
        /// <summary>
        /// Called when login fails
        /// </summary>
        private void OnLoginFailed()
        {
            Debug.LogError("[LoginUI] Login failed");
            ShowLogin("Login failed. Please try again.");
        }
        
        /// <summary>
        /// Called when ICP is initialized
        /// </summary>
        private void OnICPInitialized()
        {
            Debug.Log("[LoginUI] ICP initialized");
            
            // Check if we should try auto-login
            ICPManager.Instance.TryAutoLogin().Forget();
        }
        
        /// <summary>
        /// Shows the login panel
        /// </summary>
        private void ShowLogin(string message = null)
        {
            if (loginPanel) loginPanel.SetActive(true);
            if (loadingPanel) loadingPanel.SetActive(false);
            if (mainMenuPanel) mainMenuPanel.SetActive(false);
            
            if (statusText != null && !string.IsNullOrEmpty(message))
            {
                statusText.text = message;
            }
        }
        
        /// <summary>
        /// Shows the loading panel
        /// </summary>
        private void ShowLoading(string message = "Loading...")
        {
            if (loginPanel) loginPanel.SetActive(false);
            if (loadingPanel) loadingPanel.SetActive(true);
            if (mainMenuPanel) mainMenuPanel.SetActive(false);
            
            if (statusText != null)
            {
                statusText.text = message;
            }
        }
        
        /// <summary>
        /// Shows the main menu panel
        /// </summary>
        private void ShowMainMenu()
        {
            if (loginPanel) loginPanel.SetActive(false);
            if (loadingPanel) loadingPanel.SetActive(false);
            if (mainMenuPanel) mainMenuPanel.SetActive(true);
        }
        
        /// <summary>
        /// Handles the logout button click
        /// </summary>
        public void OnLogoutButtonClicked()
        {
            Debug.Log("[LoginUI] Logout button clicked");
            
            // Use ICPManager to logout
            if (ICPManager.Instance != null)
            {
                ICPManager.Instance.Logout();
                
                // Notify WebGL frontend
                if (Application.platform == RuntimePlatform.WebGLPlayer && WebGLBridge.Instance != null)
                {
                    WebGLBridge.Instance.NotifySignOut();
                }
                
                // Show login panel
                ShowLogin();
            }
            else
            {
                Debug.LogError("[LoginUI] ICPManager.Instance is null in OnLogoutButtonClicked");
            }
        }
        
        /// <summary>
        /// Sends a message to JavaScript using the unity-webgl bridge
        /// </summary>
        private void SendMessageToJavaScript(string eventName, string data)
        {
            // In Unity, we need a GameObject in the scene that will be the target for SendMessage
            // This will be picked up by the unity-webgl package's event listener system
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                // Create a GameObject to be the bridge target if it doesn't exist
                GameObject bridgeTarget = GameObject.Find("WebGLBridgeTarget");
                if (bridgeTarget == null)
                {
                    bridgeTarget = new GameObject("WebGLBridgeTarget");
                    DontDestroyOnLoad(bridgeTarget);
                }
                
                // Use sendMessage to call a method on the GameObject
                // This will be intercepted by the unity-webgl library
                // Example: unityContext.sendMessage('WebGLBridgeTarget', 'LoginComplete', 'username')
                Debug.Log($"[LoginUI] Sending message to JavaScript: {eventName}, {data}");
            }
        }
    }
} 