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

/*
 * This is the UI Main menu controller
 * Manages the UI references and windows to navigate through the game menus and start the in-game scenes
 * Also recive the back-end functions to initialize the player data
 */

public class UIMainMenu : MonoBehaviour
{
    public bool getInfoFromCanister = true;
    //UI User data references
    List<UIPTxtInfo> UIPropertys;
    
    public UIMatchMaking uiMatchMaking;
    
   private void Awake()
   {
        //Find and save all the UI player properties
        UIPropertys = new List<UIPTxtInfo>();
        foreach (UIPTxtInfo prop in FindObjectsOfType<UIPTxtInfo>()) { UIPropertys.Add(prop); }
        
        //If the essential data doesn't exist...
        if (!GlobalGameData.Instance.userDataLoaded) { SaveData.LoadGameUser(); }
        else { Debug.Log("UserData is Already loaded in GGD"); }

        if (getInfoFromCanister) { GetInfoUserFromCanister(); }
        else { LoadingPanel.Instance.DesactiveLoadingPanel(); }
    }
    public async void GetInfoUserFromCanister()
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
    }
    
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
