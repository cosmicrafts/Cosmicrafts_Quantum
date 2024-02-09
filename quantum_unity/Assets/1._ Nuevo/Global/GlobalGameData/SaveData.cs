using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

//Game configuration structure (will be encrypted)

public enum TypeMatch { bots, multi }

[System.Serializable] public class Config
{
    public int language = 0;
    
    public TypeMatch currentMatch = TypeMatch.multi;
    public int modeSelected = 9;
}
[System.Serializable] public class UserData
{
    public Config config = new Config();
    
    public int Id { get; set; }
    public string WalletId = "TestWalletId";
    public string NikeName = "NewNikename";
    public int Level { get; set; }
    public int AvatarID = 1;
    public int CharacterNFTId = 0;
    public List<int> DeckNFTsId;
    public List<string> DeckNFTsKeyIds;
    public string Token { get; set; }
    public string Email { get; set; }
    public int Rol { get; set; }
    public bool Online { get; set; }
    public string Region { get; set; }
    public string SocialId { get; set; }
    public DateTime LastConnection { get; set; }
    public DateTime Registered { get; set; }
}

/*
 * This is the local data controller
 * Is used to save information, like the game configuration, on the device */
public static class SaveData 
{
    public readonly static string keyUser = "UserData";
    //Load the configuration data
    public static void LoadGameUser()
    {
        Debug.Log("Load Game User");
        UserData userData = JsonConvert.DeserializeObject<UserData>(PlayerPrefs.GetString(keyUser));
        
        if (userData == null)
        {
            //Create a new game config and set it
            UserData newUserData = new UserData();
            GlobalGameData.Instance.SetUserData(newUserData);
        }
        else
        {
            //Set the configuration
            GlobalGameData.Instance.SetUserData(userData);
        }
        GlobalGameData.Instance.userDataLoaded = true;
    }
    public static void SaveGameUser(){ PlayerPrefs.SetString( keyUser, JsonConvert.SerializeObject(GlobalGameData.Instance.GetUserData()) ); }
    
   
}
