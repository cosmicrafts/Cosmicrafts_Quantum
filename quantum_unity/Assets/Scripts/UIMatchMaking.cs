using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Candid;
using Cosmicrafts.MainCanister.Models;
using UnityEngine;
using Cosmicrafts.Managers;

public class UIMatchMaking : MonoBehaviour
{
    [Header("UI Search")]
    public GameObject SearchingScreen;
    public UIMatchLoading UIMatchLoading;

    private bool sendPlayerActive = false;
    private bool isSearching = false;

    [Serializable]
    public class MatchPlayerData
    {
        public int userAvatar;
        public List<string> listSavedKeys;
    }

    public async void StartSearch()
    {
        if (GameDataManager.Instance == null)
        {
            Debug.LogError("[UIMatchMaking] GameDataManager instance is null.");
            return;
        }

        if (isSearching)
        {
            Debug.LogWarning("[UIMatchMaking] Already searching for a match.");
            return;
        }

        isSearching = true;
        SearchingScreen.SetActive(true);

        var playerData = GameDataManager.Instance.playerData;

        MatchPlayerData matchPlayerData = new MatchPlayerData
        {
            userAvatar = playerData.CharacterNFTId,
            listSavedKeys = playerData.DeckNFTsKeyIds
        };

        Debug.Log(JsonUtility.ToJson(matchPlayerData));

        var matchSearchingInfo = await CandidApiManager.Instance.MainCanister.GetMatchSearching(JsonUtility.ToJson(matchPlayerData));

        Debug.Log("Status: " + matchSearchingInfo.ReturnArg0 + " Int: " + matchSearchingInfo.ReturnArg1 + " text: " + matchSearchingInfo.ReturnArg2);

        if (matchSearchingInfo.ReturnArg0 == MMSearchStatus.Assigned)
        {
            bool isGameMatched = false;
            sendPlayerActive = true;
            SendPlayerActive();

            while (!isGameMatched && SearchingScreen.activeSelf)
            {
                if (this.gameObject == null) { break; }
                var isGameMatchedRequest = await CandidApiManager.Instance.MainCanister.IsGameMatched();
                Debug.Log("Ya estoy asignado a una sala: " + matchSearchingInfo.ReturnArg1 + " espero ser matched: " + isGameMatchedRequest.ReturnArg1);
                isGameMatched = isGameMatchedRequest.ReturnArg0;
                Debug.Log("IsGameMatched: " + isGameMatched);

                await Task.Delay(250);

                if (isGameMatched)
                {
                    sendPlayerActive = false;
                    MatchFound();
                }
            }
        }

        isSearching = false;
    }

    private async void SendPlayerActive()
    {
        while (sendPlayerActive && SearchingScreen.activeSelf)
        {
            if (this.gameObject == null) { break; }
            var isActive = await CandidApiManager.Instance.MainCanister.SetPlayerActive();
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
        sendPlayerActive = false;

        var cancelMatchmaking = await CandidApiManager.Instance.MainCanister.CancelMatchmaking();
        Debug.Log("Quiero Cancelar la busqueda: " + cancelMatchmaking.ReturnArg1);
        if (cancelMatchmaking.ReturnArg0)
        {
            SearchingScreen.SetActive(false);
        }

        isSearching = false;
    }

    public void MatchFound()
    {
        SearchingScreen.SetActive(false);
        UIMatchLoading.MatchPreStarting();
    }
}
