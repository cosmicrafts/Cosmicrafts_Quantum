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
    public TMP_Text walletIdText;
    public TMP_Text usernameText;
    public TMP_Text levelText; 

    private bool getInfoFromCanister = false;
    List<UIPTxtInfo> UIPropertys;
    public UIMatchMaking uiMatchMaking;
    
    private async void Awake()
    {
        UIPropertys = new List<UIPTxtInfo>();
        foreach (UIPTxtInfo prop in FindObjectsOfType<UIPTxtInfo>()) { UIPropertys.Add(prop); }
        
        // Get WalletID and username from AsyncLocalStorage and assign them to the TMP_Text variables
        string walletId = await AsyncLocalStorage.LoadDataAsync("PrincipalId");
        string username = await AsyncLocalStorage.LoadDataAsync("Username");
        string level = await AsyncLocalStorage.LoadDataAsync("Level");

        if (!string.IsNullOrEmpty(walletId))
        {
            if (walletId.Length > 8) // Check if WalletID is long enough to shorten
            {
                walletId = walletId.Substring(0, 5) + "..." + walletId.Substring(walletId.Length - 3);
            }
            walletIdText.text = walletId;
        }

        if (!string.IsNullOrEmpty(username))
        {
            usernameText.text = username;
        }

        if (!string.IsNullOrEmpty(level))
        {
            levelText.text = level;
        }

        if (getInfoFromCanister) { /*GetInfoUserFromCanister();*/ }
        else { LoadingPanel.Instance.DesactiveLoadingPanel(); }
    }
    
    public void ChangeLang(int lang)
    {
        PlayerPrefs.SetInt("GameLanguage", lang);
        RefreshAllPropertys();
    }
    
    public async void PlayCurrentMode()
    {
        string currentMatch = await AsyncLocalStorage.LoadDataAsync("CurrentMatch");
        switch (currentMatch)
        {
            case "multi":
                uiMatchMaking.StartSearch();
                break;
            case "bots":
                Debug.Log($"CURRENT MATCH: {currentMatch}");
                break;
            default:
                Debug.Log($"CURRENT MATCH: {currentMatch}");
                break;
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
