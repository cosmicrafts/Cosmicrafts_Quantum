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
    string mainScene = "Menu";
    
    //[SerializeField] private Animator chooseLoginAnim;
    [SerializeField] private Animator chooseUserAnim;

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

    private void Update()

    {

        // Check if Enter key is pressed

        if (Input.GetKeyDown(KeyCode.Return))

        {

            SetPlayerName(); // Trigger OK action

        }

        // Check if Escape key is pressed

        else if (Input.GetKeyDown(KeyCode.Escape))

        {

            BackLoginMenu(); // Trigger Cancel action

        }

    }

    private void OnDestroy()
    {
        Debug.Log("[Login] Component OnDestroy() - Cleaning up before destruction.");
        if (LoginManager.Instance != null) LoginManager.Instance.CancelLogin();
    }
    
    
    public void UpdateWindow(CandidApiManager.LoginData state)
    {
        Debug.Log($"[Login] UpdateWindow called with state: {state.state}, IsAnon: {state.asAnon}, Principal: {state.principal}, AccountId: {state.accountIdentifier}");
        bool isLoading = state.state == CandidApiManager.DataState.Loading; ;

        if(!state.asAnon)
        {
            Debug.Log("[Login]Logged In");
            Debug.Log($"[Login]Principal: <b>\"{state.principal}\"</b>\nAccountId: <b>\"{state.accountIdentifier}\"</b>");
            UserLoginSuccessfull();
        }
        else//Logged In As Anon
        {
            Debug.Log("[Login]Logged in as Anon");
            Debug.Log($"[Login]Principal: <b>\"{state.principal}\"</b>\nAccountId: <b>\"{state.accountIdentifier}\"</b>");
            UserLoginSuccessfull();
        }
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
        Debug.Log("[Login] Player information retrieved.");
        if (playerInfo.HasValue)
        {
        Debug.Log("[Login] HasValue");
        
            CanisterPK.CanisterLogin.Models.Player player = playerInfo.ValueOrDefault;
            GoToMenuScene(player);
        }
        else
        {
            Debug.Log("[Login] No player information available. Prompting user for username.");
            LoadingPanel.Instance.DesactiveLoadingPanel();
            chooseUserAnim.Play("ChooseUsername_Intro");
        }
    }

     public void GoToMenuScene(CanisterPK.CanisterLogin.Models.Player player)
    {
        Debug.Log($"[Login] Player Info - ID: {player.Id}, Level: {player.Level}, Name: {player.Name}");
        
        //If the essential data doesn't exist...
        if (!GlobalGameData.Instance.userDataLoaded) { SaveData.LoadGameUser(); }
        else { Debug.Log("UserData is Already loaded in GGD"); }
        
        UserData user = GlobalGameData.Instance.GetUserData();
        user.Level = (int)player.Level;
        user.NikeName = player.Name;
        user.WalletId = player.Id.ToString();
        //End load info
        
        Debug.Log("[Login] Transitioning to the main menu scene...");
        LoadingPanel.Instance.ActiveLoadingPanel();
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
                Debug.Log("[LoginPostCreate] Player information retrieved.");
                if (playerInfo.HasValue)
                {
                    CanisterPK.CanisterLogin.Models.Player player = playerInfo.ValueOrDefault;
                    GoToMenuScene(player);
                }
                else
                {
                    Debug.LogWarning("[LoginPostCreate] No player information available. Prompting user for username.");
                    Debug.LogWarning("ERROR ON Retrieve info from created user");
                    LoadingPanel.Instance.DesactiveLoadingPanel();
                    chooseUserAnim.Play("ChooseUsername_Intro");
                }
                
               
            }
            else
            {
                Debug.LogError($"[Login] Player creation failed. Error: {request.ReturnArg1}");
                infoTxt.text = "Error: " + request.ReturnArg1;
                LoadingPanel.Instance.DesactiveLoadingPanel();
            }
        }
        else
        {
            Debug.LogWarning("[Login] No name entered. Player name creation aborted.");
        }
    }
    
    public void BackLoginMenu()
    {
        Debug.Log("[Login] User selected to return to the login menu.");
        chooseUserAnim.Play("ChooseUsername_Outro");
    }
}
