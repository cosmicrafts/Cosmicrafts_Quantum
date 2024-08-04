using System.Threading.Tasks;
using Candid;
using EdjCase.ICP.Candid.Models;
using TMPro;
using UnityEngine;
using System.Numerics;
using System;
using Cosmicrafts.Managers;

public class Login : MonoBehaviour
{
    public static Login Instance { get; private set; }
    public TMP_InputField inputNameField;
    public TMP_Text infoTxt;

    [SerializeField] private GameObject chooseUsername;
    [SerializeField] private GameObject loginCanvas;
    [SerializeField] private GameObject dashboardCanvas;

    private bool isMintingDeck = false;
    private bool isRegisteringPlayer = false;

    private async void Awake()
    {
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
            await Task.Yield();
        }

        Debug.Log("[Login] CandidApiManager initialized.");
    }

    private async void Start()
    {
        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.LoadPlayerData();
            Debug.Log("[Login] Player data loaded.");
        }
        else
        {
            Debug.LogError("[Login] GameDataManager instance is null in Start.");
        }
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
            UpdatePlayerDataAndTransition(player);
        }
        else
        {
            Debug.LogWarning("[Login] No player information available. Prompting user for username.");
            chooseUsername.SetActive(true);
        }
    }

    private async Task MintDeckAsync()
    {
        if (isMintingDeck)
        {
            Debug.LogWarning("[Login] Deck minting already in progress, skipping duplicate request.");
            return;
        }

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

        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.playerData.PrincipalId = state.principal;
            Debug.Log($"[Login] Player data updated with principal: {state.principal}");
        }
        else
        {
            Debug.LogError("[Login] GameDataManager instance is null in UpdateWindow.");
        }

        UserLoginSuccessful();
    }

    public void StartWebLogin()
    {
        Debug.Log("[Login] Initiating Web login process...");
        LoadingPanel.Instance.ActiveLoadingPanel();
        CandidApiManager.Instance.StartLogin();
    }

    public async void UserLoginSuccessful()
    {
        Debug.Log("[Login] Checking player information post-login...");
        var playerInfo = await CandidApiManager.Instance.CanisterLogin.GetPlayer();
        if (playerInfo.HasValue)
        {
            Debug.Log("[Login] Player information retrieved.");
            var player = playerInfo.ValueOrDefault;
            UpdatePlayerDataAndTransition(player);
        }
        else
        {
            Debug.LogWarning("[Login] No player information available. Prompting user for username.");
            LoadingPanel.Instance.DesactiveLoadingPanel();
            chooseUsername.SetActive(true);
        }
    }

    private async void UpdatePlayerDataAndTransition(CanisterPK.CanisterLogin.Models.Player player)
    {
        try
        {
            if (GameDataManager.Instance != null)
            {
                var playerData = GameDataManager.Instance.playerData;
                playerData.Level = (int)player.Level;
                playerData.Username = player.Username;
                playerData.AvatarID = (int)player.Avatar;
                playerData.Description = player.Description;
                playerData.Elo = player.Elo;
                playerData.Friends = player.Friends;
                playerData.RegistrationDate = (long)player.RegistrationDate.ToBigInteger();
                playerData.IsLoggedIn = true;

                GameDataManager.Instance.SavePlayerData();
                Debug.Log($"[Login] PlayerData updated and saved with Player Info - ID: {player.Id}, Level: {player.Level}, Username: {player.Username}");

                loginCanvas.SetActive(false);
                dashboardCanvas.SetActive(true);
            }
            else
            {
                Debug.LogError("[Login] GameDataManager instance is null in UpdatePlayerDataAndTransition.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Login] Error occurred during login process: {ex.Message}");
        }
    }

    public async void SetPlayerName()
    {
        if (!isRegisteringPlayer && !string.IsNullOrEmpty(inputNameField.text))
        {
            isRegisteringPlayer = true;

            Debug.Log($"[Login] Attempting to create a new player with name: {inputNameField.text}");
            LoadingPanel.Instance.ActiveLoadingPanel();
            UnboundedUInt avatarID = UnboundedUInt.FromBigInteger(BigInteger.One);
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
                    _ = MintDeckAsync();

                    UpdatePlayerDataAndTransition(player);
                }
                else
                {
                    Debug.LogWarning("[LoginPostCreate] No player information available. Prompting user for username.");
                    LoadingPanel.Instance.DesactiveLoadingPanel();
                    chooseUsername.SetActive(true);
                }
            }

            isRegisteringPlayer = false;
        }
    }

    public void BackLoginMenu()
    {
        Debug.Log("[Login] User selected to return to the login menu.");
        chooseUsername.SetActive(false);
    }
}
