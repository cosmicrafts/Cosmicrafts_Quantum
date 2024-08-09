using System.Collections;
using System.Collections.Generic;
using Candid;
using Cosmicrafts.MainCanister.Models;
using TowerRush;
using UnityEngine;
using UnityEngine.UI;
using Cosmicrafts.Managers;
using Cosmicrafts.Data;

public class UIMatchLoading : MonoBehaviour
{
    [Header("UI Match and Stats from Game")]
    public GameObject MatchLoadingScreen;
    public Text Txt_VsWalletId;
    public Text Txt_VsNikeName;
    public Text Txt_VsLevel;
    public Image Img_VsIcon;
    public Image Img_VsEmblem;
    
    // Loading Bar (used when a new scene is loading)
    public Image LocalGameLoadingBar;

    private static UIMatchLoading _instance;
    public static UIMatchLoading Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = Instantiate(ResourcesServices.LoadUIMatchLoading()).GetComponent<UIMatchLoading>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MatchPreStarting()
    {
        Debug.Log("MATCH PRESTARTING");

        Txt_VsWalletId.text = "Loading";
        Txt_VsNikeName.text = "Loading";
        Txt_VsLevel.text = "Loading";
        
        Img_VsIcon.sprite = ResourcesServices.ValidateSprite(null);
        Img_VsEmblem.sprite = ResourcesServices.ValidateSprite(null);
        
        MatchLoadingScreen.SetActive(true);
        GetMatchData();
    }

    public async void GetMatchData()
    {
        var matchDataRequest = await CandidApiManager.Instance.MainCanister.GetMyMatchData();
        
        if (matchDataRequest.ReturnArg0.HasValue)
        {
            FullMatchData matchData = matchDataRequest.ReturnArg0.ValueOrDefault;

            GlobalGameData.Instance.actualRoom = "GameCosmicQuantum: " + matchData.MatchID;
            GlobalGameData.Instance.actualNumberRoom = matchData.MatchID;

            GameDataManager.Instance.playerData.actualNumberRoom = matchData.MatchID;
            GameDataManager.Instance.SavePlayerData();
            
            PlayerData player1Data = new PlayerData();
            PlayerData player2Data = new PlayerData();

            FullMatchData.Player1Info player1 = matchData.Player1;
            FullMatchData.Player2Info.Player2InfoValue player2 = matchData.Player2.ValueOrDefault;

            // Store data for Player 1
            player1Data.PrincipalId = player1.Id.ToString();
            player1Data.Username = player1.Username;
            player1Data.Level = (int)player1.Elo;

            if (string.IsNullOrEmpty(player1.PlayerGameData))
            {
                player1Data.CharacterNFTId = "1"; // Assuming this is a string
                player1Data.DeckNFTsKeyIds = new List<string>();
                Debug.Log("Error: player 1 has no saved data.");
            }
            else
            {
                // Instead of JSON deserialization, populate the data directly if available
                player1Data.CharacterNFTId = "SomeCharacterNFTId"; // Replace with actual data
                player1Data.DeckNFTsKeyIds = new List<string> { "Key1", "Key2" }; // Replace with actual data
            }

            // Store data for Player 2
            if (player2 == null || string.IsNullOrEmpty(player2.PlayerGameData))
            {
                player2Data.PrincipalId = "Error, no information";
                player2Data.Username = "Error, no information";
                player2Data.Level = 999;
                player2Data.CharacterNFTId = "1";
                player2Data.DeckNFTsKeyIds = new List<string>();
                Debug.Log("Error: player 2 has no saved data.");
            }
            else
            {
                player2Data.PrincipalId = player2.Id.ToString();
                player2Data.Username = player2.Username;
                player2Data.Level = (int)player2.Elo;
                player2Data.CharacterNFTId = "SomeCharacterNFTId"; // Replace with actual data
                player2Data.DeckNFTsKeyIds = new List<string> { "Key1", "Key2" }; // Replace with actual data
            }

            if ((int)matchDataRequest.ReturnArg1 != 0)
            {
                if ((int)matchDataRequest.ReturnArg1 == 1)
                {
                    MatchStarting(player1Data, player2Data);
                }
                else if ((int)matchDataRequest.ReturnArg1 == 2)
                {
                    MatchStarting(player2Data, player1Data);
                }
            }
        }
        else
        {
            Debug.Log("No match data available.");
        }
    }

    public void MatchStarting(PlayerData myUserData, PlayerData vsUserData)
    {
        Debug.Log("MATCH STARTING");
        
        Txt_VsWalletId.text = vsUserData.PrincipalId;
        Txt_VsNikeName.text = vsUserData.Username;
        Txt_VsLevel.text = vsUserData.Level.ToString();
        
        Img_VsIcon.sprite = ResourcesServices.ValidateSprite(null);
        Img_VsEmblem.sprite = ResourcesServices.ValidateSprite(null);
        
        StartCoroutine(LoadLocalGame());
    }

    
    IEnumerator LoadLocalGame()
    {
        yield return new WaitForSeconds(2f);
        Game.CurrentScene.FinishScene();
    }
    
    public void OnInitMatch()
    {
        Debug.Log("MATCH FINISH");
        Destroy(this.gameObject);
    }
}
