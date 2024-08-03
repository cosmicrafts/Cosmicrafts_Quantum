using System;
using UnityEngine;
using EdjCase.ICP.Candid.Models;

public class GlobalGameData : MonoBehaviour
{
    private static GlobalGameData _instance;
    private static bool isApplicationQuitting = false;

    public static GlobalGameData Instance {
        get 
        {
            if (_instance == null && !isApplicationQuitting)
            {
                var globalGameDataPrefab = ResourcesServices.LoadGlobalManager();
                if (globalGameDataPrefab != null)
                {
                    _instance = Instantiate(globalGameDataPrefab).GetComponent<GlobalGameData>();
                    Debug.Log("GlobalGameData instantiated from: " + System.Environment.StackTrace);
                    DontDestroyOnLoad(_instance.gameObject);
                }
                else
                {
                    Debug.LogError("GlobalGameData prefab not found in Resources.");
                }
            }
            return _instance;
        }
        set
        {
            if (_instance && _instance.gameObject)
            {
                Destroy(_instance.gameObject);
                Debug.Log("GlobalGameData instance destroyed from setter.");
            }
            _instance = value;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GlobalGameData set in Awake.");
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            Debug.Log("GlobalGameData duplicate destroyed in Awake.");
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            Debug.Log("GlobalGameData instance destroyed.");
            _instance = null;
        }
    }

    private void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }

    public bool UserIsInit() { return userData != null; }
    public bool userDataLoaded = false;

    private UserData userData;
    public string actualRoom = "TestingRoom";
    public int avatarId = 1;
    public UnboundedUInt actualNumberRoom = 0;

    public event Action<int> OnAvatarIdChanged;

    public UserData GetUserData() { if (userData == null) { userData = new UserData(); } return userData; }
    public void SetUserData(UserData _userData) { userData = _userData; }

    public Language GetGameLanguage() { return (Language)GetUserData().config.language; }
    public void SetGameLanguage(Language language)
    {
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
            Debug.Log($"GlobalGameData: Setting avatar ID to {id}");
            OnAvatarIdChanged?.Invoke(id);
        }
    }

    // Save and load seed phrase
    public void SaveSeedPhrase(string seedPhrase)
    {
        PlayerPrefs.SetString("SeedPhrase", seedPhrase);
        PlayerPrefs.Save();
    }

    public string LoadSeedPhrase()
    {
        return PlayerPrefs.HasKey("SeedPhrase") ? PlayerPrefs.GetString("SeedPhrase") : null;
    }
}
