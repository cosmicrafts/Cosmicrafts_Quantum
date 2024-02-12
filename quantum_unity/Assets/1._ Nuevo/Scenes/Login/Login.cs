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
        Instance = this;
    }
    private void OnDestroy()
    {
        LoginManager.Instance.CancelLogin();
    }
    
    
    public void UpdateWindow(CandidApiManager.LoginData state)
    {
        bool isLoading = state.state == CandidApiManager.DataState.Loading; ;

        if(!state.asAnon)
        {
            Debug.Log("Logged In");
            Debug.Log($"Principal: <b>\"{state.principal}\"</b>\nAccountId: <b>\"{state.accountIdentifier}\"</b>");
            UserLoginSuccessfull();
        }
        else//Logged In As Anon
        {
            Debug.Log("Logged in as Anon");
            Debug.Log($"Principal: <b>\"{state.principal}\"</b>\nAccountId: <b>\"{state.accountIdentifier}\"</b>");
            UserLoginSuccessfull();
        }
        
    }


    public void StartWebLogin()
    {
        LoadingPanel.Instance.ActiveLoadingPanel();
        //chooseLoginAnim.Play("ChooseLogin_Outro");
        
        CandidApiManager.Instance.StartLogin();
    }
    
    public async void UserLoginSuccessfull()
    {
        Debug.Log("Antes de getPlayer");
        var playerInfo = await CandidApiManager.Instance.CanisterLogin.GetPlayer();
        Debug.Log("despues de getPlayer");
        if (playerInfo.HasValue)
        {
            CanisterPK.CanisterLogin.Models.Player player = playerInfo.ValueOrDefault;
            Debug.Log(" ID: " + player.Id + " Lv: "+player.Level + " Name: " + player.Name);
            await Task.Delay(2000);
            GoToMenuScene();

        }
        else
        {
            Debug.Log("Player 2 Dont has value");
            LoadingPanel.Instance.DesactiveLoadingPanel();
            chooseUserAnim.Play("ChooseUsername_Intro");
        }
    }

    public void GoToMenuScene()
    {
        Debug.Log("Antes de cambiar de escena");
        LoadingPanel.Instance.ActiveLoadingPanel();
        SceneManager.LoadScene(1);
    }
    
    public async void SetPlayerName()
    {
        if (inputNameField.text != null)
        {
            LoadingPanel.Instance.ActiveLoadingPanel();
            var request =  await CandidApiManager.Instance.CanisterLogin.CreatePlayer(inputNameField.text);
            if (request.ReturnArg0)
            {
                Debug.Log(request.ReturnArg1);
                GoToMenuScene();
            }
            else
            {
                infoTxt.text = request.ReturnArg1;
                LoadingPanel.Instance.DesactiveLoadingPanel();
            }
            
        }
    }
    
    
    public void BackLoginMenu()
    {
        chooseUserAnim.Play("ChooseUsername_Outro"); 
        //chooseLoginAnim.Play("ChooseLogin_Intro");
    }

    

    
   

}
