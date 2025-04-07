using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Cosmicrafts.ICP
{
    /// <summary>
    /// Helper script to set up the login scene with all required components
    /// This is meant to be used by developers in the Unity Editor to quickly set up the login scene
    /// </summary>
    [ExecuteInEditMode]
    public class LoginSceneSetup : MonoBehaviour
    {
        [Header("Core Components")]
        public bool createManagerComponents = true;
        
        [Header("UI Components")]
        public bool createUIComponents = true;
        
        [ContextMenu("Setup Login Scene")]
        public void SetupLoginScene()
        {
            if (createManagerComponents)
            {
                CreateManagerComponents();
            }
            
            if (createUIComponents)
            {
                CreateUIComponents();
            }
            
            Debug.Log("[LoginSceneSetup] Login scene setup complete");
        }
        
        private void CreateManagerComponents()
        {
            // Create ICPManager if it doesn't exist
            if (FindObjectOfType<ICPManager>() == null)
            {
                GameObject icpManagerObj = new GameObject("ICPManager");
                icpManagerObj.AddComponent<ICPManager>();
                Debug.Log("[LoginSceneSetup] Created ICPManager");
            }
            
            // Create WebGLBridge if it doesn't exist
            if (FindObjectOfType<WebGLBridge>() == null)
            {
                GameObject webGLBridgeObj = new GameObject("WebGLBridge");
                webGLBridgeObj.AddComponent<WebGLBridge>();
                Debug.Log("[LoginSceneSetup] Created WebGLBridge");
            }
            
            // Create PlayerManager if it doesn't exist
            if (FindObjectOfType<PlayerManager>() == null)
            {
                GameObject playerManagerObj = new GameObject("PlayerManager");
                playerManagerObj.AddComponent<PlayerManager>();
                Debug.Log("[LoginSceneSetup] Created PlayerManager");
            }
        }
        
        private void CreateUIComponents()
        {
            // Create Canvas if it doesn't exist
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
                
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                Debug.Log("[LoginSceneSetup] Created Canvas");
                
                // Create EventSystem if it doesn't exist
                if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
                {
                    GameObject eventSystemObj = new GameObject("EventSystem");
                    eventSystemObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
                    eventSystemObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                    Debug.Log("[LoginSceneSetup] Created EventSystem");
                }
            }
            
            // Create Login UI panels
            GameObject loginUIObj = new GameObject("LoginUI");
            loginUIObj.transform.SetParent(canvas.transform, false);
            LoginUI loginUI = loginUIObj.AddComponent<LoginUI>();
            
            // Create Login Panel
            GameObject loginPanel = CreatePanel("LoginPanel", loginUIObj.transform);
            
            // Create Loading Panel
            GameObject loadingPanel = CreatePanel("LoadingPanel", loginUIObj.transform);
            
            // Create Main Menu Panel
            GameObject mainMenuPanel = CreatePanel("MainMenuPanel", loginUIObj.transform);
            
            // Add buttons and text to Login Panel
            Button loginButton = CreateButton("LoginButton", loginPanel.transform, "Login with Internet Identity");
            Button testLoginButton = CreateButton("TestLoginButton", loginPanel.transform, "Login with Test Identity");
            
            RectTransform loginButtonRect = loginButton.GetComponent<RectTransform>();
            loginButtonRect.anchoredPosition = new Vector2(0, 50);
            
            RectTransform testLoginButtonRect = testLoginButton.GetComponent<RectTransform>();
            testLoginButtonRect.anchoredPosition = new Vector2(0, -50);
            
            // Add status text to Loading Panel
            TMP_Text statusText = CreateText("StatusText", loadingPanel.transform, "Loading...");
            statusText.alignment = TextAlignmentOptions.Center;
            
            // Add Principal ID text to Main Menu Panel
            TMP_Text principalIdText = CreateText("PrincipalIDText", mainMenuPanel.transform, "Principal ID: Not Logged In");
            principalIdText.alignment = TextAlignmentOptions.Center;
            RectTransform principalIdTextRect = principalIdText.GetComponent<RectTransform>();
            principalIdTextRect.anchoredPosition = new Vector2(0, 150);
            
            // Add Username and Level text to Main Menu Panel
            TMP_Text usernameText = CreateText("UsernameText", mainMenuPanel.transform, "Username: Guest");
            usernameText.alignment = TextAlignmentOptions.Center;
            RectTransform usernameTextRect = usernameText.GetComponent<RectTransform>();
            usernameTextRect.anchoredPosition = new Vector2(0, 50);
            
            // Add Logout button to Main Menu Panel
            Button logoutButton = CreateButton("LogoutButton", mainMenuPanel.transform, "Logout");
            RectTransform logoutButtonRect = logoutButton.GetComponent<RectTransform>();
            logoutButtonRect.anchoredPosition = new Vector2(0, -150);
            
            // Create Play Button
            Button playButton = CreateButton("PlayButton", mainMenuPanel.transform, "Play Game");
            RectTransform playButtonRect = playButton.GetComponent<RectTransform>();
            playButtonRect.anchoredPosition = new Vector2(0, -50);
            playButtonRect.sizeDelta = new Vector2(300, 80);
            
            // Connect UI elements to LoginUI script
            loginUI.loginPanel = loginPanel;
            loginUI.loadingPanel = loadingPanel;
            loginUI.mainMenuPanel = mainMenuPanel;
            loginUI.loginButton = loginButton;
            loginUI.testLoginButton = testLoginButton;
            loginUI.principalIdText = principalIdText;
            loginUI.statusText = statusText;
            
            // Add onClick listener for logout button
            logoutButton.onClick.AddListener(loginUI.OnLogoutButtonClicked);
            
            // Add onClick listener for play button - this will be connected to scene transition logic in the game
            playButton.onClick.AddListener(() => {
                Debug.Log("[LoginUI] Play button clicked. Implement scene transition logic here.");
            });
            
            Debug.Log("[LoginSceneSetup] Created Login UI components");
        }
        
        private GameObject CreatePanel(string name, Transform parent)
        {
            GameObject panel = new GameObject(name);
            panel.transform.SetParent(parent, false);
            
            RectTransform rectTransform = panel.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            
            Image image = panel.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0.8f);
            
            return panel;
        }
        
        private Button CreateButton(string name, Transform parent, string text)
        {
            GameObject buttonObj = new GameObject(name);
            buttonObj.transform.SetParent(parent, false);
            
            RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(300, 60);
            
            Image image = buttonObj.AddComponent<Image>();
            image.color = new Color(0.2f, 0.2f, 0.2f, 1);
            
            Button button = buttonObj.AddComponent<Button>();
            button.targetGraphic = image;
            ColorBlock colors = button.colors;
            colors.normalColor = new Color(0.2f, 0.2f, 0.2f, 1);
            colors.highlightedColor = new Color(0.3f, 0.3f, 0.3f, 1);
            colors.pressedColor = new Color(0.1f, 0.1f, 0.1f, 1);
            button.colors = colors;
            
            // Add Text
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            
            RectTransform textRectTransform = textObj.AddComponent<RectTransform>();
            textRectTransform.anchorMin = Vector2.zero;
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.sizeDelta = Vector2.zero;
            
            TMP_Text tmpText = textObj.AddComponent<TextMeshProUGUI>();
            tmpText.text = text;
            tmpText.color = Color.white;
            tmpText.fontSize = 24;
            tmpText.alignment = TextAlignmentOptions.Center;
            
            return button;
        }
        
        private TMP_Text CreateText(string name, Transform parent, string text)
        {
            GameObject textObj = new GameObject(name);
            textObj.transform.SetParent(parent, false);
            
            RectTransform rectTransform = textObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(600, 100);
            
            TMP_Text tmpText = textObj.AddComponent<TextMeshProUGUI>();
            tmpText.text = text;
            tmpText.color = Color.white;
            tmpText.fontSize = 28;
            
            return tmpText;
        }
    }
} 