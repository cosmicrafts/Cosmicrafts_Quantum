using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Candid;
using CanisterPK.CanisterMatchMaking.Models;
using TowerRush;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;

public class UIMatchLoading : MonoBehaviour
{
    [Header("UI Match and Stats from Game")]
    public GameObject MatchLoadingScreen;
    public Text Txt_VsWalletId;
    public Text Txt_VsNikeName;
    public Text Txt_VsLevel;
    public Image Img_VsIcon;
    public Image Img_VsEmblem;
    
    //Loading Bar (used when a new scene is loading)
    public Image LocalGameLoadingBar;

    private static UIMatchLoading _instance;
    public static UIMatchLoading Instance {
        get 
        {
            if (_instance == null) { _instance = Instantiate( ResourcesServices.LoadUIMatchLoading() ).GetComponent<UIMatchLoading>(); }
            return _instance;
        }
    }

    private void Awake()
    {
        if (!_instance) { _instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
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
        
        var matchDataRequest = await CandidApiManager.Instance.CanisterMatchMaking.GetMyMatchData();
        
        if (matchDataRequest.ReturnArg0.HasValue)
        {
            CanisterPK.CanisterMatchMaking.Models.FullMatchData matchData = matchDataRequest.ReturnArg0.ValueOrDefault;

            GlobalGameData.Instance.actualRoom = "GameCosmicQuantum: " + matchData.GameId;
            GlobalGameData.Instance.actualNumberRoom = matchData.GameId;
            
            UserData UserData1 = new UserData();
            UserData UserData2 = new UserData();

            CanisterPK.CanisterMatchMaking.Models.FullPlayerInfo tempData1 = new FullPlayerInfo();
            CanisterPK.CanisterMatchMaking.Models.FullPlayerInfo tempData2 = new FullPlayerInfo();

                                              tempData1 = matchData.Player1;
            if (matchData.Player2.HasValue) { tempData2 = matchData.Player2.ValueOrDefault; }
            
            UserData1.WalletId = tempData1.Id.ToString();
            UserData1.NikeName = tempData1.PlayerName;
            UserData1.Level = (int) tempData1.Elo;
           
            if (tempData1.PlayerGameData.IsNullOrEmpty())
            {
                UserData1.CharacterNFTId = 1;
                UserData1.DeckNFTsKeyIds = new List<string>();
                Debug.Log("Error el jugador no tiene datos guardados");
            }
            else
            {
                UIMatchMaking.MatchPlayerData matchPlayerData1 = JsonUtility.FromJson<UIMatchMaking.MatchPlayerData>(tempData1.PlayerGameData);
                UserData1.CharacterNFTId = matchPlayerData1.userAvatar;
                UserData1.DeckNFTsKeyIds = matchPlayerData1.listSavedKeys; 
            }
            
            if (tempData2.PlayerGameData.IsNullOrEmpty())
            {
                UserData2.WalletId = "Error, no información";
                UserData2.NikeName = "Error, no información";
                UserData2.Level = 999;
                UserData1.CharacterNFTId = 1;
                UserData1.DeckNFTsKeyIds = new List<string>();
                Debug.Log("Error el jugador no tiene datos guardados");
            }
            else
            {
                UserData2.WalletId = tempData2.Id.ToString();
                UserData2.NikeName = tempData2.PlayerName;
                UserData2.Level = (int) tempData2.Elo;
                UIMatchMaking.MatchPlayerData matchPlayerData2 = JsonUtility.FromJson<UIMatchMaking.MatchPlayerData>(tempData2.PlayerGameData);
                UserData2.CharacterNFTId = matchPlayerData2.userAvatar;
                UserData2.DeckNFTsKeyIds = matchPlayerData2.listSavedKeys;
            }

          
           

            if ((int) matchDataRequest.ReturnArg1 != 0)
            {
                if ((int) matchDataRequest.ReturnArg1 == 1)
                {
                    MatchStarting(UserData1, UserData2);
                }
                else if ((int) matchDataRequest.ReturnArg1 == 2)
                {
                    MatchStarting(UserData2, UserData1);
                }
            }
            
            
        }
        else
        {
            Debug.Log("No hay info del match");
        }
        
    }
    
    public void MatchStarting(UserData MyUserData, UserData VsUserData)
    {
        Debug.Log("MATCH STARTING");
        
        Txt_VsWalletId.text = VsUserData.WalletId;
        Txt_VsNikeName.text = VsUserData.NikeName;
        Txt_VsLevel.text = VsUserData.Level.ToString();
        
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
