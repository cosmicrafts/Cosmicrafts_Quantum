using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Linq;
using Candid;
using EdjCase.ICP.Candid.Models;
using TMPro;
using UnityEngine.Networking;

public class UIMainMenu : MonoBehaviour
{
    public TMP_Text walletIdText; // Public TMP_Text variable for WalletID
    public TMP_Text usernameText; // Public TMP_Text variable for username
    public TMP_Text levelText; 

    private bool getInfoFromCanister = false;
    List<UIPTxtInfo> UIPropertys;
    public UIMatchMaking uiMatchMaking;
    
    private void Awake()
    {
        UIPropertys = new List<UIPTxtInfo>();
        foreach (UIPTxtInfo prop in FindObjectsOfType<UIPTxtInfo>()) { UIPropertys.Add(prop); }
        
        // Get WalletID and username from GlobalGameData and assign them to the TMP_Text variables
        UserData userData = GlobalGameData.Instance.GetUserData();
        if (userData != null)
        {
            string walletId = userData.WalletId;
            if (walletId.Length > 8) // Check if WalletID is long enough to shorten
            {
                walletId = walletId.Substring(0, 5) + "..." + walletId.Substring(walletId.Length - 3);
            }
            walletIdText.text = walletId;
            usernameText.text = userData.NikeName;
            levelText.text = userData.Level.ToString();
        }

        
        if (getInfoFromCanister) { /*GetInfoUserFromCanister();*/ }
        else { LoadingPanel.Instance.DesactiveLoadingPanel(); }
    }
   
    /*public async void GetInfoUserFromCanister()
    {
        var playerDataRequest = await CandidApiManager.Instance.CanisterLogin.GetMyPlayerData();

        if (playerDataRequest.HasValue)
        {
            CanisterPK.CanisterLogin.Models.Player playerData = playerDataRequest.ValueOrDefault;
            UserData user = GlobalGameData.Instance.GetUserData();
            user.Level = (int)playerData.Level;
            user.NikeName = playerData.Name;
            user.WalletId = playerData.Id.ToString();

            Debug.Log("Nickname: " + user.NikeName +  " Level: " + user.Level + " WalletId: " + user.WalletId );
        }
        else { Debug.Log("playerDataRequest Dont HasValue"); }
        
        LoadingPanel.Instance.DesactiveLoadingPanel();
    }*/
    
    public void ChangeLang(int lang)
    {
        GlobalGameData.Instance.SetGameLanguage((Language)lang);
        RefreshAllPropertys();
    }
    
    public void PlayCurrentMode()
    {
        switch (GlobalGameData.Instance.GetUserData().config.currentMatch)
        {
            case TypeMatch.multi:
            {
                uiMatchMaking.StartSearch();
                break;
            }
            case TypeMatch.bots:
            {
                Debug.Log($"CURRENT MATCH: {GlobalGameData.Instance.GetUserData().config.currentMatch}");
                break;
            }
            default:
            {
                Debug.Log($"CURRENT MATCH: {GlobalGameData.Instance.GetUserData().config.currentMatch}");
                break;
            }
                
        }
    }

  

    //Refresh a specific UI propertie of the player
    public void RefreshProperty(PlayerProperty property)
    {
        foreach (UIPTxtInfo prop in UIPropertys.Where(f => f.Property == property)) { prop.LoadProperty(); }
    }
    //Refresh all the UI references properties of the player
    public void RefreshAllPropertys()
    {
        foreach (UIPTxtInfo prop in UIPropertys) { prop.LoadProperty(); }
    }
    
}
