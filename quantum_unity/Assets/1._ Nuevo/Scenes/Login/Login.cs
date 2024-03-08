using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Candid;
using EdjCase.ICP.Candid.Models;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public static Login Instance { get; private set; }
    public TMP_InputField inputNameField;
    public TMP_Text infoTxt;

    [SerializeField] private GameObject chooseUsername;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("[Login] Instance already exists. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("[Login] Component Awake() - Login instance initialized.");
    }

    private void OnDestroy()
    {
        Debug.Log("[Login] Component OnDestroy() - Cleaning up before destruction.");
        if (LoginManager.Instance != null) LoginManager.Instance.CancelLogin();
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
            CanisterPK.CanisterLogin.Models.Player player = playerInfo.ValueOrDefault;
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
        // Update GlobalGameData with player details immediately
        UserData user = GlobalGameData.Instance.GetUserData();
        user.Level = (int)player.Level;
        user.NikeName = player.Name;
        user.WalletId = player.Id.ToString();
        
        // Assume this logs imply the data is now updated in memory
        Debug.Log($"[Login] UserData updated with Player Info - ID: {player.Id}, Level: {player.Level}, Name: {player.Name}");

        // Direct transition to main menu scene without further checks
        // await Task.Delay(3000); 
        Debug.Log("[Login] Transitioning to the main menu scene...");
        SceneManager.LoadScene(1);
    }

    public async void SetPlayerName()
    {
        if (!string.IsNullOrEmpty(inputNameField.text))
        {
            Debug.Log($"[Login] Attempting to create a new player with name: {inputNameField.text}");
            LoadingPanel.Instance.ActiveLoadingPanel();
            var request = await CandidApiManager.Instance.CanisterLogin.CreatePlayer(inputNameField.text);

            if (request.ReturnArg0)
            {
                Debug.Log($"[Login] Player creation successful. Player ID: {request.ReturnArg1}");
                var playerInfo = await CandidApiManager.Instance.CanisterLogin.GetPlayer();
                if (playerInfo.HasValue)
                {
                    Debug.Log("[LoginPostCreate] Player information retrieved.");
                    CanisterPK.CanisterLogin.Models.Player player = playerInfo.ValueOrDefault;
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
