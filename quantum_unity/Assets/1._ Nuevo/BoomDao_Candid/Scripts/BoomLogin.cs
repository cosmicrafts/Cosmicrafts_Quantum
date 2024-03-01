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
    using Newtonsoft.Json;
    using UnityEngine.SceneManagement;

    public class BoomLogin : MonoBehaviour
    {
        public static BoomLogin Instance { get; private set; }

        #region FIELDS

        [SerializeField] private string mainScene = "Menu_Cosmic";
        [SerializeField] private Animator chooseUserAnim;
        [SerializeField] TMP_InputField userNameInputField;
        [SerializeField] TMP_Text actionLogText;
        [SerializeField] Button actionButton;

        private Coroutine logCoroutine;
        string actionId = "set_username";

        #endregion

        #region MONO
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(ActionButtonClickHandler);
            UserUtil.AddListenerMainDataChange<MainDataTypes.LoginData>(LoginDataChangeHandler);
        }

        private void OnDestroy()
        {
            actionButton.onClick.RemoveListener(ActionButtonClickHandler);
            UserUtil.RemoveListenerMainDataChange<MainDataTypes.LoginData>(LoginDataChangeHandler);
        }

        private void OnEnable()
        {
            actionLogText.text = "...";
        }
        #endregion

        #region ACTION 
        public void ActionButtonClickHandler()
        {
            ExecuteAction().Forget();
        }

        public async UniTaskVoid ExecuteAction()
        {
            if (logCoroutine != null) StopCoroutine(logCoroutine);

            var newUsername = userNameInputField.text;
            if (string.IsNullOrEmpty(newUsername)) return;

            List<Field> fields = new() { new("username", newUsername) };
            actionLogText.text = $"Processing Action of id: \"{actionId}\" with arguments:\n{JsonConvert.SerializeObject(fields)}";
            var actionResult = await ActionUtil.ProcessAction(actionId, fields);

            if (actionResult.IsErr)
            {
                string errorMessage = actionResult.AsErr().content;
                Debug.LogError(errorMessage);
                logCoroutine = StartCoroutine(DisplayTempLog(errorMessage));
                return;
            }

            if (!actionResult.IsErr)
            {
                SceneManager.LoadScene(mainScene);
            }
            else
            {
                string errorMessage = actionResult.AsErr().content;
                Debug.LogError(errorMessage);
                logCoroutine = StartCoroutine(DisplayTempLog(errorMessage));
            }

            logCoroutine = StartCoroutine(DisplayTempLog($"You have changed your username to: {newUsername}"));
        }
        #endregion

        #region LOG
        IEnumerator DisplayTempLog(string message, float duration = 5f)
        {
            actionLogText.text = message;
            yield return new WaitForSeconds(duration);
            actionLogText.text = "...";
        }
        #endregion

        #region USER DATA
        private void LoginDataChangeHandler(MainDataTypes.LoginData data)
        {
            if (data.state != MainDataTypes.LoginData.State.LoggedIn) return;

            var principalId = data.principal;
            EntityUtil.TryGetFieldAsText(principalId, "user_profile", "username", out var username, "None");

            if (!string.IsNullOrEmpty(username) && username != "None")
            {
                UpdateGlobalGameData(username, principalId);
                SceneManager.LoadScene(mainScene);
            }
            else
            {
                chooseUserAnim.SetBool("ShowInput", true);
            }

            UpdateInputFieldContent(username);
        }

        private void UpdateGlobalGameData(string username, string principalId)
        {
            if (!GlobalGameData.Instance.userDataLoaded)
            {
                SaveData.LoadGameUser();
            }

            UserData user = GlobalGameData.Instance.GetUserData();
            user.NikeName = username;
            user.WalletId = principalId;

            SaveData.SaveGameUser();
            Debug.Log($"GlobalGameData updated: Username={username}, PrincipalId={principalId}");
        }

        private void UpdateInputFieldContent(string value)
        {
            userNameInputField.SetTextWithoutNotify(value);
        }
        #endregion
    }
}
