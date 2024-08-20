namespace CosmicraftsSP {
    using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * This code manages the multiplayer matchmaking and show the UI matching elements
 * Also Manages the multiplayer lobby and opens the multiplayer game scene when the players are ready
 */

public class UIMatchMaking : MonoBehaviour
{
    //Searching match screen reference
    public GameObject SearchingScreen;
    //Match found screen reference
    public GameObject MatchScreen;

    //1 SEC. delta time
    WaitForSeconds DeltaOne;

    //Cancel Searching button
    public GameObject CancelButton;

    //Search Status text reference
    public Text StatusGame;
    //Searching Icon reference
    public GameObject SearchIcon;
    //Match found Icon reference
    public GameObject FoundIcon;

    //UI Players properties and icons references
    public Text Txt_VsWalletId;
    public Text Txt_VsNikeName;
    public Text Txt_VsLevel;
    public Image Img_VsIcon;
    public Image Img_VsEmblem;

    //UI count down and tips references
    public Text Txt_CountDown;
    public Text Txt_Tips;

    //Player data
    User MyUserData;
    UserGeneral VsUserData;

    //The current serching was canceled
    bool IsCanceled;

    //Game start count down
    int CoutDown;

    // Start is called before the first frame update
    void Start()
    {
        //Set the delta time
        DeltaOne = new WaitForSeconds(1f);
    }

    //Start searching for a match
    public void StartSearch()
    {
        //Get the player's data
        MyUserData = GlobalManager.GMD.GetUserData();
        //If this is a debug build...
        if (GlobalManager.GMD.DebugMode)
        {
            //Make a fake VS player
            GlobalManager.GMD.ImMaster = false;
            GlobalManager.GMD.SetVsUser(new UserGeneral()
            {
                NikeName = "Vs Player",
                WalletId = "SomeWalletIdRandomNumbers",
                Level = 23,
                Xp = 175,
                Avatar = 2
            });
            SceneManager.LoadScene(2);
            return;
        }
        //Shown the corresponding UI
        SearchingScreen.SetActive(true);
        MatchScreen.SetActive(false);
        CancelButton.SetActive(false);
        SearchIcon.SetActive(true);
        FoundIcon.SetActive(false);
        StatusGame.text = Lang.GetText("mn_matchmaking");
        //Initialize de network variables
        GameNetwork.Start();
        IsCanceled = false;
        CoutDown = 5;
        //Enter to the searching loop
        StartCoroutine(Searching());
    }

    //Stop the searching
    public void CancelSearch()
    {
        SearchingScreen.SetActive(false);
        IsCanceled = true;
        if (GameNetwork.GetId() != 0)
        {
            GameNetwork.JSCancelGame(GameNetwork.GetId());
        }
        StopAllCoroutines();
    }

    //Load the Enemy player data
    void UpdateVsData()
    {
        VsUserData = GameNetwork.GetVsData();
        UpdateUI_VSData();
        GlobalManager.GMD.SetVsUser(VsUserData);
    }

    //Update the UI Enemy data
    void UpdateUI_VSData()
    {
        Txt_VsWalletId.text = Utils.GetWalletIDShort(VsUserData.WalletId);
        Txt_VsNikeName.text = VsUserData.NikeName;
        Txt_VsLevel.text = $"{Lang.GetText("mn_lvl")} {VsUserData.Level}";
        Img_VsIcon.sprite = ResourcesServices.LoadCharacterIcon(GameNetwork.GetVSnftCharacter().KeyId);
        Img_VsEmblem.sprite = ResourcesServices.LoadCharacterEmblem(GameNetwork.GetVSnftCharacter().KeyId);
    }

    //Called from the WEB, set the current match info
    public void GLGetICPData(string json)
    {
        if (IsCanceled)
            return;
        GameNetwork.UpdateGameData(json);
        GlobalManager.GMD.ImMaster = GameNetwork.GetMasterWalletId() == MyUserData.WalletId;
    }

    //Searching loop
    IEnumerator Searching()
    {
        //Make a resume of the player data
        string MyJsonProfile = JsonConvert.SerializeObject(GlobalManager.GMD.BuildMyProfileHasVS());
        if (GlobalManager.GMD.IsProductionWeb())
        {
            //Send a request for a match with the player´s data
            GameNetwork.JSSearchGame(MyJsonProfile);
        }

        //Wait for match
        yield return new WaitUntil(() => GameNetwork.GetId() != 0);

        //Check if the player cancels the operation
        if (IsCanceled)
            yield return null;

        //Join to the match
        Debug.Log($"Match: {GameNetwork.GetId()}");
        //Update the match info
        GameNetwork.SetClientGameId(GameNetwork.GetId());
        //Update the UI
        CancelButton.SetActive(true);
        SearchIcon.SetActive(false);
        FoundIcon.SetActive(true);

        //Wait for the other player
        yield return new WaitUntil(() => GameNetwork.GameRoomIsFull());

        //Check if the player cancels the operation
        if (IsCanceled)
            yield return null;

        //Update the UI data
        UpdateVsData();
        //Show the lobby with the players
        SearchingScreen.SetActive(false);
        MatchScreen.SetActive(true);
        Txt_Tips.text = Lang.GetText($"mn_tip_{UnityEngine.Random.Range(1,4)}");
        Txt_CountDown.text = CoutDown.ToString();

        //Load the game scene but not open it
        AsyncOperation loading = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
        loading.allowSceneActivation = false;
        //Begin the count down
        yield return DeltaOne;

        while (CoutDown > 0)
        {
            CoutDown--;
            Txt_CountDown.text = CoutDown.ToString();
            yield return DeltaOne;
        }
        //Go to the Game Scene
        Debug.Log("--LOADING GAME SCENE--");
        loading.allowSceneActivation = true;
    }
}
}