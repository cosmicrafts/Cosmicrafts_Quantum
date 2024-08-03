using System.Threading.Tasks;
using Candid;
using EdjCase.ICP.Candid.Models;
using TMPro;
using TowerRush;
using UnityEngine;
using System.Numerics;
using System;

public class Login : MonoBehaviour
{
    public static Login Instance { get; private set; }
    public TMP_InputField inputNameField;
    public TMP_Text infoTxt;

    [SerializeField] private GameObject chooseUsername;
    [SerializeField] private GameObject loginCanvas;
    [SerializeField] private GameObject dashboardCanvas;

    private bool isMintingDeck = false; // Flag to prevent duplicate minting calls

    private async void Awake()
    {
        GlobalGameData.Instance = null;

        if (Instance != null)
        {
            Debug.LogWarning("[Login] Instance already exists. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("[Login] Component Awake() - Login instance initialized.");

        await WaitForCandidApiInitialization();
        await InitializeLogin();
    }

    private async Task WaitForCandidApiInitialization()
    {
        Debug.Log("[Login] Waiting for CandidApiManager initialization...");

        while (CandidApiManager.Instance == null || CandidApiManager.Instance.CanisterLogin == null)
        {
            await Task.Yield(); // Wait until CandidApiManager and CanisterLogin are initialized
        }

        Debug.Log("[Login] CandidApiManager initialized.");
    }

    private void Start()
    {
        // Ensure login canvas is active and dashboard canvas is inactive at the start
        loginCanvas.SetActive(true);
        dashboardCanvas.SetActive(false);
    }

    private void OnDestroy()
    {
        Debug.Log("[Login] Component OnDestroy() - Cleaning up before destruction.");
        if (LoginManager.Instance != null) LoginManager.Instance.CancelLogin();
    }

    private async Task InitializeLogin()
    {
        Debug.Log("[Login] Initializing login...");

        var playerInfo = await CandidApiManager.Instance.CanisterLogin.GetPlayer();
        if (playerInfo.HasValue)
        {
            Debug.Log("[Login] Player already exists.");
            var player = playerInfo.ValueOrDefault;
            UpdateUserDataAndTransition(player);
        }
        else
        {
            Debug.LogWarning("[Login] No player information available. Prompting user for username.");

            // Start minting in the background
            _ = MintDeckAsync();
            chooseUsername.SetActive(true);
        }
    }

    private async Task MintDeckAsync()
    {
        if (isMintingDeck) return; // Prevent duplicate calls
        isMintingDeck = true;

        Debug.Log("[Login] Initiating deck minting...");

        var mintInfo = await CandidApiManager.Instance.testnft.MintDeck();
        if (mintInfo.ReturnArg0)
        {
            Debug.Log("[Login] MINT SUCCESS");
        }
        else
        {
            Debug.LogError("[Login] ERROR MINT NFTs");
        }
    }

    public void UpdateWindow(CandidApiManager.LoginData state)
    {
        Debug.Log($"[Login] UpdateWindow called with state: {state.state}, IsAnon: {state.asAnon}, Principal: {state.principal}, AccountId: {state.accountIdentifier}");
        bool isLoading = state.state == CandidApiManager.DataState.Loading;

        UserData user = GlobalGameData.Instance.GetUserData();
        user.WalletId = state.principal;
        Debug.Log($"[Login] Global game data updated with principal: {state.principal}");

        UserLoginSuccessfull();
    }

    public void StartWebLogin()
    {
        Debug.Log("[Login] Initiating Web login process...");
        LoadingPanel.Instance.ActiveLoadingPanel();
        CandidApiManager.Instance.StartLogin();
    }

    public async void UserLoginSuccessfull()
    {
        Debug.Log("[Login] Checking player information post-login...");
        var playerInfo = await CandidApiManager.Instance.CanisterLogin.GetPlayer();
        if (playerInfo.HasValue)
        {
            Debug.Log("[Login] Player information retrieved.");
            var player = playerInfo.ValueOrDefault;
            UpdateUserDataAndTransition(player);
        }
        else
        {
            Debug.LogWarning("[Login] No player information available. Prompting user for username.");
            LoadingPanel.Instance.DesactiveLoadingPanel();
            chooseUsername.SetActive(true);
        }
    }

    private async void UpdateUserDataAndTransition(CanisterPK.CanisterLogin.Models.Player player)
    {
        try
        {
            // Update GlobalGameData with player details immediately
            UserData user = GlobalGameData.Instance.GetUserData();
            user.Level = (int)player.Level;
            user.NikeName = player.Username;
            user.WalletId = player.Id.ToString();
            SaveData.SaveGameUser();

            Debug.Log($"[Login] UserData updated with Player Info - ID: {player.Id}, Level: {player.Level}, Username: {player.Username}");
            // Transition to the dashboard canvas
            Debug.Log("[Login] Transitioning to the dashboard...");
            loginCanvas.SetActive(false);
            Game.CurrentScene.FinishScene();
            dashboardCanvas.SetActive(true);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Login] Error occurred during login process: {ex.Message}");
        }
    }

    public async void SetPlayerName()
    {
        if (!string.IsNullOrEmpty(inputNameField.text))
        {
            Debug.Log($"[Login] Attempting to create a new player with name: {inputNameField.text}");
            LoadingPanel.Instance.ActiveLoadingPanel();
            // Hardcoding AvatarID as 1 for now
            UnboundedUInt avatarID = UnboundedUInt.FromBigInteger(BigInteger.One); // This can be changed later
            var request = await CandidApiManager.Instance.CanisterLogin.RegisterPlayer(inputNameField.text, avatarID);

            if (request.ReturnArg0)
            {
                Debug.Log($"[Login] Player creation successful. Player ID: {request.ReturnArg1}");
                var playerInfo = await CandidApiManager.Instance.CanisterLogin.GetPlayer();
                if (playerInfo.HasValue)
                {
                    Debug.Log("[LoginPostCreate] Player information retrieved.");
                    var player = playerInfo.ValueOrDefault;

                    Debug.Log("[Login] INIT MINT");
                    _ = MintDeckAsync(); // Mint in the background

                    UpdateUserDataAndTransition(player);
                }
                else
                {
                    Debug.LogWarning("[LoginPostCreate] No player information available. Prompting user for username.");
                    LoadingPanel.Instance.DesactiveLoadingPanel();
                    chooseUsername.SetActive(true);
                }
            }
        }
    }

    public void BackLoginMenu()
    {
        Debug.Log("[Login] User selected to return to the login menu.");
        chooseUsername.SetActive(false);
    }
}
