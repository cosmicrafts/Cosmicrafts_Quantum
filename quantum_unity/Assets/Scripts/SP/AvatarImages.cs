namespace CosmicraftsSP {
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarImages : MonoBehaviour
{


    public Transform contentAvatars;
    public GameObject avatarPrefab;
    private int avatarNumber = 30;

    private void Start()
    {
        InstanceAvatarsButtons();
    }

    public void InstanceAvatarsButtons()
    {
        for (int i = 1; i <= avatarNumber; i++)
        {
            int avatarIndex = i;
            GameObject avatarButton = Instantiate(avatarPrefab, contentAvatars);
            avatarButton.GetComponent<Image>().sprite = ResourcesServices.LoadAvatarUser(avatarIndex);
            avatarButton.transform.GetComponentInChildren<Button>().onClick.AddListener(() => { SetAvatarsSprites(avatarIndex); });
            avatarButton.SetActive(true);
        }
    }
    
    public void SetAvatarsSprites(int newAvatarIndex)
    {
        PlayerPrefs.SetInt("savedAvatar", newAvatarIndex);

        User userData = GlobalManager.GMD.GetUserData();
        userData.Avatar = newAvatarIndex;
        GlobalManager.GMD.SetUser(userData);
        
        UIMainMenu.Menu.RefreshProperty(PlayerProperty.Avatar);
    }
}
}