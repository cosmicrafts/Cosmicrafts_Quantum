using System.Collections;
using System.Collections.Generic;
using Candid;
using Cosmicrafts.MainCanister.Models;
using TowerRush;
using UnityEngine;
using UnityEngine.UI;
using Cosmicrafts.Managers;
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
            
            UserData userData1 = new UserData();
            UserData userData2 = new UserData();

            FullMatchData.Player1Info player1 = matchData.Player1;
            FullMatchData.Player2Info.Player2InfoValue player2 = matchData.Player2.ValueOrDefault;

            userData1.WalletId = player1.Id.ToString();
            userData1.NikeName = player1.Username;
            userData1.Level = (int)player1.Elo;

            if (string.IsNullOrEmpty(player1.PlayerGameData))
            {
                userData1.CharacterNFTId = 1;
                userData1.DeckNFTsKeyIds = new List<string>();
                Debug.Log("Error: player 1 has no saved data.");
            }
            else
            {
                UIMatchMaking.MatchPlayerData matchPlayerData1 = JsonUtility.FromJson<UIMatchMaking.MatchPlayerData>(player1.PlayerGameData);
                userData1.CharacterNFTId = matchPlayerData1.userAvatar;
                userData1.DeckNFTsKeyIds = matchPlayerData1.listSavedKeys; 
            }

            if (player2 == null || string.IsNullOrEmpty(player2.PlayerGameData))
            {
                userData2.WalletId = "Error, no information";
                userData2.NikeName = "Error, no information";
                userData2.Level = 999;
                userData2.CharacterNFTId = 1;
                userData2.DeckNFTsKeyIds = new List<string>();
                Debug.Log("Error: player 2 has no saved data.");
            }
            else
            {
                userData2.WalletId = player2.Id.ToString();
                userData2.NikeName = player2.Username;
                userData2.Level = (int)player2.Elo;
                UIMatchMaking.MatchPlayerData matchPlayerData2 = JsonUtility.FromJson<UIMatchMaking.MatchPlayerData>(player2.PlayerGameData);
                userData2.CharacterNFTId = matchPlayerData2.userAvatar;
                userData2.DeckNFTsKeyIds = matchPlayerData2.listSavedKeys;
            }

            if ((int)matchDataRequest.ReturnArg1 != 0)
            {
                if ((int)matchDataRequest.ReturnArg1 == 1)
                {
                    MatchStarting(userData1, userData2);
                }
                else if ((int)matchDataRequest.ReturnArg1 == 2)
                {
                    MatchStarting(userData2, userData1);
                }
            }
        }
        else
        {
            Debug.Log("No match data available.");
        }
    }
    
    public void MatchStarting(UserData myUserData, UserData vsUserData)
    {
        Debug.Log("MATCH STARTING");
        
        Txt_VsWalletId.text = vsUserData.WalletId;
        Txt_VsNikeName.text = vsUserData.NikeName;
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
