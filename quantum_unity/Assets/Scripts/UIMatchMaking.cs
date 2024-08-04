using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Candid;
using CanisterPK.CanisterLogin.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Cosmicrafts.Managers;
using Cosmicrafts.Data;

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
        public List<string> listSavedKeys;
    }

    public void StartSearch()
    {
        if (GameDataManager.Instance == null)
        {
            Debug.LogError("[UIMatchMaking] GameDataManager instance is null.");
            return;
        }

        SearchingScreen.SetActive(true);

        var playerData = GameDataManager.Instance.playerData;

        MatchPlayerData matchPlayerData = new MatchPlayerData
        {
            userAvatar = playerData.CharacterNFTId,
            listSavedKeys = playerData.DeckNFTsKeyIds
        };

        Debug.Log(JsonUtility.ToJson(matchPlayerData));

        var matchSearchingInfo = CandidApiManager.Instance.CanisterLogin.GetMatchSearching(JsonUtility.ToJson(matchPlayerData)).Result;

        Debug.Log("Status: " + matchSearchingInfo.ReturnArg0 + " Int: " + matchSearchingInfo.ReturnArg1 + " text: " + matchSearchingInfo.ReturnArg2);

        if (matchSearchingInfo.ReturnArg0 == MMSearchStatus.Assigned)
        {
            bool isGameMatched = false;
            sendPlayerActive = true;
            SendPlayerActive();

            while (!isGameMatched && SearchingScreen.activeSelf)
            {
                if (this.gameObject == null) { break; }
                var isGameMatchedRequest = CandidApiManager.Instance.CanisterLogin.IsGameMatched().Result;
                Debug.Log("Ya estoy asignado a una sala: " + matchSearchingInfo.ReturnArg1 + " espero ser matched: " + isGameMatchedRequest.ReturnArg1);
                isGameMatched = isGameMatchedRequest.ReturnArg0;
                Debug.Log("IsGameMatched: " + isGameMatched);

                Task.Delay(250).Wait();

                if (isGameMatched)
                {
                    sendPlayerActive = false;
                    MatchFound();
                }
            }
        }
    }

    private async void SendPlayerActive()
    {
        while (sendPlayerActive && SearchingScreen.activeSelf)
        {
            if (this.gameObject == null) { break; }
            var isActive = await CandidApiManager.Instance.CanisterLogin.SetPlayerActive();
            Debug.Log("estoy activo: " + isActive);

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
