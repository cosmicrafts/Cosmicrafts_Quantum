using UnityEngine;
using System;
using EdjCase.ICP.Candid.Models;

public class GlobalGameData : MonoBehaviour
{
    private static GlobalGameData _instance;
    public static GlobalGameData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Instantiate(ResourcesServices.LoadGlobalManager().GetComponent<GlobalGameData>());
            }
            return _instance;
        }
        set
        {
            if (_instance && _instance.gameObject)
            {
                Destroy(_instance.gameObject);
            }
            _instance = value;
        }
    }

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool UserIsInit() { return userData != null; }
    public bool userDataLoaded = false;

    UserData userData;
    public string actualRoom = "TestingRoom";
    public int avatarId = 1;
    public UnboundedUInt actualNumberRoom = 0;

    public event Action<int> OnAvatarIdChanged;

    public UserData GetUserData()
    {
        if (userData == null)
        {
            userData = new UserData();
        }
        return userData;
    }

    public void SetUserData(UserData _userData)
    {
        userData = _userData;
    }


    public void SetCurrentMatch(TypeMatch typeMatch)
    {
        GetUserData().config.currentMatch = typeMatch;
        SaveData.SaveGameUser();
    }

}
