using UnityEngine;
using System;
using EdjCase.ICP.Candid.Models;

public class GlobalGameData : MonoBehaviour
{
    private static GlobalGameData _instance;
    public static GlobalGameData Instance {
        get 
        {
            if (_instance == null) { _instance = Instantiate( ResourcesServices.LoadGlobalManager().GetComponent<GlobalGameData>() ); }
            return _instance;
        }
        set
        {
            if(_instance &&_instance.gameObject) { Destroy(_instance.gameObject); }
            _instance = value;
        }
    }
    private void Awake() {  
        if(!_instance){ _instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }
    
    public bool UserIsInit() { return userData != null; }
    public bool userDataLoaded = false;
    
    UserData userData;
    public string actualRoom = "TestingRoom";
    public int avatarId = 1;
    public UnboundedUInt actualNumberRoom = 0;

    public event Action<int> OnAvatarIdChanged;
    
    public UserData GetUserData() { if (userData == null) { userData = new UserData(); } return userData; }
    public void SetUserData(UserData _userData) { userData = _userData; }
    
    
    public Language GetGameLanguage() { return (Language)GetUserData().config.language; }
    public void SetGameLanguage(Language language) {
        Lang.SetLang(language);
        GetUserData().config.language = (int)language;
        SaveData.SaveGameUser();
    }
    
    public void SetUserCharacterNFTId(int NFTid)
    {
        GetUserData().CharacterNFTId = NFTid;
        SaveData.SaveGameUser();
    }
    public void SetUserAvatar(int AvatarID)
    {
        GetUserData().AvatarID = AvatarID;
        SetAvatarId(AvatarID);
        SaveData.SaveGameUser();
    }
    public void SetCurrentMatch(TypeMatch typeMatch)
    {
        GetUserData().config.currentMatch = typeMatch;
        SaveData.SaveGameUser();
    }
    public void ClearUser() { userData = null; }
    public string GetVersion() { return Application.version; }
    public void SetAvatarId(int id)
    {
        if (avatarId != id)
        {
            avatarId = id;
            OnAvatarIdChanged?.Invoke(id);
        }
    }
}
