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

public class BoomLogin : MonoBehaviour
{
    [SerializeField] private string mainScene = "Menu_Cosmic";
    [SerializeField] private TMP_InputField userNameInputField;
    [SerializeField] private TMP_Text actionLogText;
    [SerializeField] private Button actionButton;

    private string storedPrincipalId; // Added field to store the principal ID

    private void Awake()
    {
        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(ActionButtonClickHandler);
        UserUtil.AddListenerMainDataChange<MainDataTypes.LoginData>(LoginDataChangeHandler);
    }

    private void OnDestroy()
    {
        actionButton.onClick.RemoveListener(ActionButtonClickHandler);
        UserUtil.RemoveListenerMainDataChange<MainDataTypes.LoginData>(LoginDataChangeHandler);
    }

    public void ActionButtonClickHandler()
    {
        LoadingPanel.Instance.ActiveLoadingPanel();
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

        // Use the stored principal ID for further actions
        await UpdateGlobalGameData(newUsername, storedPrincipalId);

        SceneManager.LoadScene(mainScene);
        LoadingPanel.Instance.DesactiveLoadingPanel();
    }

    private void LoginDataChangeHandler(MainDataTypes.LoginData data)
    {
        if (data.state != MainDataTypes.LoginData.State.LoggedIn) return;

        EntityUtil.TryGetFieldAsText(data.principal, "user_profile", "username", out var username, "None");
        

        //Comment to always prompt username
        if (!string.IsNullOrEmpty(username) && username != "None")
        {
        SceneManager.LoadScene(mainScene);
        }

        UpdateGlobalGameData(username, data.principal);
        storedPrincipalId = data.principal;
    }

    private async UniTask UpdateGlobalGameData(string username, string principalId)
    {
        if (!GlobalGameData.Instance.userDataLoaded)
        {
           // await SaveData.LoadGameUserAsync();
        }
    }
}