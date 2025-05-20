using UnityEngine;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;

namespace Cosmicrafts.ICP
{
    /// <summary>
    /// Bridge between Unity WebGL and JavaScript for ICP authentication
    /// </summary>
    public class WebGLBridge : MonoBehaviour
    {
        public static WebGLBridge Instance { get; private set; }

        // JavaScript functions we'll call from Unity
        [DllImport("__Internal")]
        private static extern void RequestLogin();

        [DllImport("__Internal")]
        private static extern void NotifyGameReady();

        [DllImport("__Internal")]
        private static extern void SignOut();

        // Flag to track initialization
        private bool isInitialized = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[WebGLBridge] Instance initialized");
        }

        private void Start()
        {
            // Initialize with a slight delay to ensure everything else is set up
            UniTask.Void(async () =>
            {
                await UniTask.DelayFrame(10);
                Initialize();
            });
        }

        /// <summary>
        /// Initialize the bridge and notify JavaScript that the game is ready
        /// </summary>
        public void Initialize()
        {
            if (isInitialized)
                return;

            Debug.Log("[WebGLBridge] Initializing WebGL bridge");

            // Check if we're running in WebGL build
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                NotifyJavaScriptGameReady();
            }
            else
            {
                Debug.Log("[WebGLBridge] Not running in WebGL, skipping JavaScript notification");
            }

            isInitialized = true;
        }

        /// <summary>
        /// Notify JavaScript that the game is ready to receive messages
        /// </summary>
        private void NotifyJavaScriptGameReady()
        {
            try
            {
                Debug.Log("[WebGLBridge] Notifying JavaScript that game is ready");
                NotifyGameReady();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[WebGLBridge] Error notifying JavaScript: {e.Message}");
            }
        }

        /// <summary>
        /// Request login from JavaScript
        /// </summary>
        public void RequestLoginFromJavaScript()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                Debug.Log("[WebGLBridge] Requesting login from JavaScript");
                try
                {
                    RequestLogin();
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[WebGLBridge] Error requesting login: {e.Message}");
                }
            }
            else
            {
                Debug.LogWarning("[WebGLBridge] Not running in WebGL, can't request login from JavaScript");
            }
        }

        /// <summary>
        /// Notify JavaScript that the user has signed out
        /// </summary>
        public void NotifySignOut()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                Debug.Log("[WebGLBridge] Notifying JavaScript of sign out");
                try
                {
                    SignOut();
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[WebGLBridge] Error notifying sign out: {e.Message}");
                }
            }
        }

        /// <summary>
        /// Called from JavaScript when identity is received
        /// </summary>
        public void ReceiveIdentityFromJavaScript(string identityJson)
        {
            Debug.Log("[WebGLBridge] Received identity from JavaScript");
            
            // Forward to the ICP Manager
            if (ICPManager.Instance != null)
            {
                UniTask.Void(async () => 
                {
                    await ICPManager.Instance.CreateAgentFromWeb(identityJson);
                });
            }
            else
            {
                Debug.LogError("[WebGLBridge] ICPManager.Instance is null, cannot process identity");
            }
        }
    }
} 