namespace Boom
{
    using Boom.Utility;
    using Boom.Values;
    using Boom;
    using Candid;
    using Cysharp.Threading.Tasks;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using System;
    using Candid.World.Models;
    using UnityEngine.SceneManagement;
    using Newtonsoft.Json;
using System.Threading.Tasks;
using EdjCase.ICP.Candid.Models;
using UnityEngine.InputSystem;


    public class BoomLogin : MonoBehaviour
    {
        [SerializeField] private string mainScene = "Menu_Cosmic";
        [SerializeField] private TMP_InputField userNameInputField;
        [SerializeField] private TMP_Text actionLogText;
        [SerializeField] private Button actionButton;

        private void Awake()
        {
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(ActionButtonClickHandler);
            UserUtil.AddListenerMainDataChange<MainDataTypes.LoginData>(LoginDataChangeHandler);
            CandidApiManager.OnLoginCompletedEvent += HandleLoginCompleted;
        }
        private void HandleLoginCompleted(CandidApiManager.LoginData loginData)
    {
        // Handle login completion, e.g., transition scenes, update UI
        Debug.Log("Login Completed in BoomLogin");
        UpdateWindow(loginData);
    }

        public void StartWebLogin()
    {
        Debug.Log("[Login] Initiating Web login process...");
        LoadingPanel.Instance.ActiveLoadingPanel();
        CandidApiManager.Instance.StartLogin();
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

        public async void UserLoginSuccessfull()
        {
           
        }

        private void OnDestroy()
        {
            actionButton.onClick.RemoveListener(ActionButtonClickHandler);
            UserUtil.RemoveListenerMainDataChange<MainDataTypes.LoginData>(LoginDataChangeHandler);
            CandidApiManager.OnLoginCompletedEvent -= HandleLoginCompleted;
        }

        public void ActionButtonClickHandler()
        {
            ExecuteAction().Forget();
        }

        public async UniTaskVoid ExecuteAction()
        {
            var newUsername = userNameInputField.text;
            if (string.IsNullOrEmpty(newUsername)) return;

            var actionId = "set_username";
            var fields = new List<Field> { new("username", newUsername) };

            actionLogText.text = $"Processing Action of id: \"{actionId}\" with arguments:\n{JsonConvert.SerializeObject(fields)}";
            var actionResult = await ActionUtil.ProcessAction(actionId, fields);

            if (actionResult.IsErr)
            {
                actionLogText.text = actionResult.AsErr().content;
                return;
            }

            actionLogText.text = $"You have changed your username to: {newUsername}";
            SceneManager.LoadScene(mainScene);
        }

        private void LoginDataChangeHandler(MainDataTypes.LoginData data)
        {
            if (data.state != MainDataTypes.LoginData.State.LoggedIn) return;

            EntityUtil.TryGetFieldAsText(data.principal, "user_profile", "username", out var username, "None");
            UpdateGlobalGameData(username, data.principal);
            if (!string.IsNullOrEmpty(username) && username != "None")
            {
                SceneManager.LoadScene(mainScene);
            }
        }

        private void UpdateGlobalGameData(string username, string principalId)
        {
            if (!GlobalGameData.Instance.userDataLoaded)
            {
                SaveData.LoadGameUser();
            }

            var user = GlobalGameData.Instance.GetUserData();
            user.NikeName = username;
            user.WalletId = principalId;
            SaveData.SaveGameUser();
        }
    }
}