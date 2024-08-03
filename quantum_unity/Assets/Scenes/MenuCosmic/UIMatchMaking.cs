using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Candid;
using CanisterPK.CanisterLogin.Models;
using EdjCase.ICP.Candid.Models;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMatchMaking : MonoBehaviour
{ 
    [Header("UI Search")]
    public GameObject SearchingScreen;
    public UIMatchLoading UIMatchLoading;
    
    private bool sendPlayerActive = false;
    
    [System.Serializable]
    public class MatchPlayerData
    {
        public int userAvatar;
        public List<String> listSavedKeys;
    }
    
    //Start searching for a match
    public async void StartSearch()
    {
        SearchingScreen.SetActive(true);
        
        //Obtener lista del deck para enviar al canister
        //Code here:
        
        MatchPlayerData matchPlayerData = new MatchPlayerData();
        matchPlayerData.userAvatar = GlobalGameData.Instance.GetUserData().CharacterNFTId;
        matchPlayerData.listSavedKeys = new List<string>();

        Debug.Log(JsonUtility.ToJson(matchPlayerData));

        var matchSearchingInfo = await CandidApiManager.Instance.CanisterLogin.GetMatchSearching(JsonUtility.ToJson(matchPlayerData));
        
        Debug.Log("Status: "+ matchSearchingInfo.ReturnArg0 + " Int: " + matchSearchingInfo.ReturnArg1 + " text: " + matchSearchingInfo.ReturnArg2);
        
        if(matchSearchingInfo.ReturnArg0 == MMSearchStatus.Assigned)
        {
            bool isGameMatched = false;
            sendPlayerActive = true; SendPlayerActive();
            //Verificar si IsGameMatched, en un bucle recursivo
            while (!isGameMatched && SearchingScreen.activeSelf)
            {
                if(this.gameObject == null) { break; }
                var isGameMatchedRequest = await CandidApiManager.Instance.CanisterLogin.IsGameMatched();
                Debug.Log("Ya estoy asignado a una sala: " + matchSearchingInfo.ReturnArg1 +" espero ser matched: " + isGameMatchedRequest.ReturnArg1);
                isGameMatched = isGameMatchedRequest.ReturnArg0;
                Debug.Log("IsGameMatched: "+ isGameMatched );
                
                await Task.Delay(250);
                
                if(isGameMatched) { sendPlayerActive = false; MatchFound(); }
            }
        }
    }
    private async void SendPlayerActive()
    {
        while (sendPlayerActive && SearchingScreen.activeSelf)
        {
            if(this.gameObject == null) { break; }
            var isActive = await CandidApiManager.Instance.CanisterLogin.SetPlayerActive();
            Debug.Log("estoy activo: "+ isActive);

            if (!isActive)
            {
                CancelSearch();
                break;
            }
            
            await Task.Delay(5000);
        }
    }
    public async void CancelSearch()
    {
        var cancelMatchmaking = await CandidApiManager.Instance.CanisterLogin.CancelMatchmaking();
        Debug.Log("Quiero Cancelar la busqueda: " + cancelMatchmaking.ReturnArg1);
        if (cancelMatchmaking.ReturnArg0)
        {
            SearchingScreen.SetActive(false);
        }
    }
    
    public void MatchFound()
    {
        SearchingScreen.SetActive(false);
        UIMatchLoading.MatchPreStarting();
    }
    
    
   

    

}
