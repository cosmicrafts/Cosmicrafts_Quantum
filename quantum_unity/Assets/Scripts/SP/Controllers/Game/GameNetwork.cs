namespace CosmicraftsSP {
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

/*
 * This is the in-game multiplayer data
 * Saves and manages the network data
 * Also defines the backend functions (JS)
 */

public static class GameNetwork
{
    //Game package data (MASTER)
    static NetGamePack GameNetPack;
    //Game package data (CLIENT)
    static NetClientGamePack ClientNetPack;

    //Init game packages
    public static void Start()
    {
        GameNetPack = new NetGamePack();
        ClientNetPack = new NetClientGamePack();
        GameNetPack.LastUpdate = DateTime.Now;
        ClientNetPack.LastUpdate = DateTime.Now;
    }

    //Returns the multiplayer game id (back end)
    public static int GetId()
    {
        return GameNetPack.GameId;
    }

    //Set the multiplayer game id
    public static void SetClientGameId(int gameId)
    {
        ClientNetPack.GameId = gameId;
    }

    //Set the package (master)
    public static void UpdateGameData(string json)
    {
        GameNetPack = JsonConvert.DeserializeObject<NetGamePack>(json);
    }

    //Set the package (client)
    public static void UpdateClientGameData(string json)
    {
        ClientNetPack = JsonConvert.DeserializeObject<NetClientGamePack>(json);
    }

    //Get the package (master)
    public static string GetJsonGameNetPack()
    {
        return JsonConvert.SerializeObject(GameNetPack);
    }

    //Get the package (client)
    public static string GetJsonClientGameNetPack()
    {
        return JsonConvert.SerializeObject(ClientNetPack);
    }

    //Set the status game 
    public static void SetGameStatus(NetGameStep step)
    {
        GameNetPack.GameStep = (int)step;
    }

    //set when the game started
    public static void SetGameStart(DateTime start)
    {
        GameNetPack.GameStart = start;
    }

    //get the game start
    public static DateTime GetStartTime()
    {
        return GameNetPack.GameStart;
    }

    //set when the cient send the last datetime update
    public static void SetClientLastUpdate(DateTime dateTime)
    {
        ClientNetPack.LastUpdate = dateTime;
    }

    //set when the master send the last datetime update
    public static void SetMasterLastUpdate(DateTime dateTime)
    {
        GameNetPack.LastUpdate = dateTime;
    }

    //set the winer
    public static void SetWinner(int GameWinner)
    {
        GameNetPack.GameWinner = GameWinner;
    }

    //get the winer
    public static int GetWinner()
    {
        return GameNetPack.GameWinner;
    }

    //get the game status
    public static NetGameStep GetGameStatus()
    {
        return (NetGameStep)GameNetPack.GameStep;
    }

    //Set the currents units in the game
    public static void SetGameUnits(List<NetUnitPack> netUnitPack)
    {
        GameNetPack.Units = netUnitPack;
    }

    //Get the current units in the game
    public static List<NetUnitPack> GetGameUnits()
    {
        return GameNetPack.Units;
    }

    //Set the deleted units of the game
    public static void SetGameDeletedUnits(List<int> netUnitsDeleted)
    {
        GameNetPack.DeleteIdsUnits = netUnitsDeleted;
    }

    //Get the deleted units of the game
    public static List<int> GetGameUnitsDeleted()
    {
        return GameNetPack.DeleteIdsUnits;
    }

    //Set the requested units from client
    public static void SetRequestedGameUnits(List<NetUnitPack> netUnitPack)
    {
        ClientNetPack.UnitsRequested = netUnitPack;
    }

    //Get the requested units of the game
    public static List<NetUnitPack> GetClientGameUnitsRequested()
    {
        return ClientNetPack.UnitsRequested;
    }

    //Set master game score
    public static void SetMasterScore(int score)
    {
        GameNetPack.MasterScore = score;
    }

    //Set client game score
    public static void SetClientScore(int score)
    {
        ClientNetPack.ClientScore = score;
    }

    //Set the basic master´s data (wallet id and username)
    public static void SetMasterData(string wid, string name)
    {
        GameNetPack.MasterWalletId = wid;
        GameNetPack.MasterPlayerName = name;
    }

    //Set the basic client´s data (wallet id and username)
    public static void SetClientData(string wid, string name)
    {
        GameNetPack.ClientWalletId = wid;
        GameNetPack.ClientPlayerName = name;
    }

    //Get the master wallet id
    public static string GetMasterWalletId()
    {
        return GameNetPack.MasterWalletId;
    }

    //Get the client wallet id
    public static string GetClientWalletId()
    {
        return GameNetPack.ClientWalletId;
    }

    //Returns the master NFT Character
    public static NFTsCharacter GetMasterNFTCharacter()
    {
        return GameNetPack.MasterCharacter;
    }

    //Returns the client NFT Character
    public static NFTsCharacter GetClientNFTCharacter()
    {
        return GameNetPack.ClientCharacter;
    }

    //Gets the master NFTs deck
    public static List<NetCardNft> GetMasterNFTDeck()
    {
        return GameNetPack.MasterDeck;
    }

    //Gets the client NFTs deck
    public static List<NetCardNft> GetClientNFTDeck()
    {
        return GameNetPack.ClientDeck;
    }

    //Get the enemy´s data
    public static UserGeneral GetVsData()
    {
        return GlobalManager.GMD.ImMaster ? 
            new UserGeneral() { 
                NikeName = GameNetPack.ClientPlayerName, 
                WalletId = GameNetPack.ClientWalletId,
                Xp = GameNetPack.ClientXp,
                Level = GameNetPack.ClientLvl,
                Avatar = GameNetPack.ClientAvatar
            } : 
            new UserGeneral() { 
                NikeName = GameNetPack.MasterPlayerName, 
                WalletId = GameNetPack.MasterWalletId,
                Xp = GameNetPack.MasterXp,
                Level = GameNetPack.MasterLvl,
                Avatar = GameNetPack.MasterAvatar
            };
    }

    //Get the VS player nft character
    public static NFTsCharacter GetVSnftCharacter()
    {
        return GlobalManager.GMD.ImMaster ? GameNetPack.ClientCharacter : GameNetPack.MasterCharacter;
    }

    //Get the VS player nfts deck
    public static List<NetCardNft> GetVSnftDeck()
    {
        return GlobalManager.GMD.ImMaster ? GameNetPack.ClientDeck : GameNetPack.MasterDeck;
    }

    //Check if the game lobby is full (ready to begin)
    public static bool GameRoomIsFull()
    {
        return !string.IsNullOrEmpty(GameNetPack.ClientWalletId) && !string.IsNullOrEmpty(GameNetPack.MasterWalletId);
    }

    //The main menu starts
    [DllImport("__Internal")]
    public static extern void JSDashboardStarts();

    //Save the game score
    [DllImport("__Internal")]
    public static extern void JSSaveScore(int score);

    //Save the game configuration
    [DllImport("__Internal")]
    public static extern void JSSavePlayerConfig(string json);

    //Save the current character of the player
    [DllImport("__Internal")]
    public static extern void JSSavePlayerCharacter(int nftid);

    //Send the game data (master's data)
    [DllImport("__Internal")]
    public static extern void JSSendMasterData(string json);

    //Send the game data (client's data)
    [DllImport("__Internal")]
    public static extern void JSSendClientData(string json);

    //The game starts
    [DllImport("__Internal")]
    public static extern void JSGameStarts();

    //Ends the game
    [DllImport("__Internal")]
    public static extern void JSExitGame();

    //Serch for a match
    [DllImport("__Internal")]
    public static extern void JSSearchGame(string json);

    //Cancel the match making
    [DllImport("__Internal")]
    public static extern void JSCancelGame(int gameId);

    [DllImport("__Internal")]
    public static extern void JSLoginPanel(string accountID);

    [DllImport("__Internal")]
    public static extern void JSWalletsLogin(string walletID);

   // [DllImport("__Internal")]
   // public static extern void RequestAnvilConnect(string anvilUrl);

    [DllImport("__Internal")]
    public static extern void JSAnvilConnect();

    [DllImport("__Internal")]
    public static extern void JSGetAnvilNfts(string anvilUrl);

    [DllImport("__Internal")]
    public static extern void JSClaimNft(int nftIndex);
    
    [DllImport("__Internal")]
    public static extern void JSClaimAllNft(string indexArray);
    
    [DllImport("__Internal")]
    public static extern void JSGoToMenu();


}
}
