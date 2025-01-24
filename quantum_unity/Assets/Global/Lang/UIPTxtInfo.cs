﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * This code shows a player property on a UI element (UI component)
 */

//Types of properties of the player
public enum PlayerProperty
{
    Name,
    WalletId,
    Level,
    Xp,
    Xpbar,
    Character,
    Avatar,
    Score,
    XpProgress,
    Emblem,
    CharacterName,
    Description,
    CurrentElo,
    CurrentLeague,
    CurrentSubLeague,
    CurrentLeagueIcon
}

public class UIPTxtInfo : MonoBehaviour
{
    //The player property
    public PlayerProperty Property;

    //Load and show the property when begins
    void Start()
    {
        LoadProperty();
    }

    void OnDestroy()
    {
        LoadProperty();
    }

    public void SetText(string text)
    {
        Text mytext = GetComponent<Text>();
        if (mytext != null) { mytext.text = text; }

        TMP_Text mytmp = GetComponent<TMP_Text>();
        if (mytmp != null) { mytmp.text = text; }
    }
    public void LoadProperty()
    {
        //Check if the player data exist
        if (!GlobalGameData.Instance.UserIsInit()) { return; }

        //Show the selected property
        switch (Property)
        {
            case PlayerProperty.Name:
                {
                    UserData user = GlobalGameData.Instance.GetUserData();
                    SetText(user.NikeName);
                }
                break;
            case PlayerProperty.WalletId:
                {
                    UserData user = GlobalGameData.Instance.GetUserData();
                    SetText(Utils.GetWalletIDShort(user.WalletId)); 
                }
                break;
            case PlayerProperty.Level:
                {
                    SetText($"{Lang.GetText("mn_lvl")} {GlobalGameData.Instance.GetUserData().Level}");
                }
                break;
            case PlayerProperty.Xp:
                {
                    SetText($"Error XP");
                }
                break;
            case PlayerProperty.XpProgress:
                {
                    SetText($"XX/XX");
                }
                break;
            case PlayerProperty.Xpbar:
                {
                    Debug.Log("Error XPBar UIPTxtInfo");
                    /*UserProgress userProgress = GlobalGameData.Instance.GetUserProgress();
                    Image myimage = GetComponent<Image>();
                    myimage.fillAmount = (float)userProgress.GetXp() / (float)userProgress.GetNextXpGoal();*/
                }
                break;
            case PlayerProperty.Character:
                {
                    Debug.Log("Error CharacterImage UIPTxtInfo");
                    /*NFTsCharacter nFTsCharacter = GlobalGameData.Instance.GetUserCharacter();
                    Image myimage = GetComponent<Image>();
                    myimage.sprite = ResourcesServices.ValidateSprite(nFTsCharacter.IconSprite);*/
                }
                break;
            case PlayerProperty.CharacterName:
                {
                    string key = GlobalGameData.Instance.GetUserData().CharacterNFTId.ToString();
                    SetText(Lang.GetEntityName(key));
                }
                break;
            case PlayerProperty.Avatar:
                {
                    Debug.Log("Error Avatar UIPTxtInfo");
                    /*User user = GlobalGameData.Instance.GetUserData();
                    Image myimage = GetComponent<Image>();
                    myimage.sprite = ResourcesServices.LoadAvatarUser(user.Avatar);*/
                }
                break;
            case PlayerProperty.Score:
                {
                    SetText("ErrorScore");
                }
                break;
            case PlayerProperty.Emblem:
                {
                    Debug.Log("Error Emblem UIPTxtInfo");
                    /*NFTsCharacter nFTsCharacter = GlobalGameData.Instance.GetUserCharacter();
                    Image myimage = GetComponent<Image>();
                    myimage.sprite = ResourcesServices.LoadCharacterEmblem(nFTsCharacter.KeyId);*/
                }
                break;
            case PlayerProperty.Description:
                {
                    Debug.Log("Error Description UIPTxtInfo");
                    /*string key = GlobalGameData.Instance.GetUserCharacter().KeyId;
                    SetText(Lang.GetEntityDescription(key));*/
                }
                break;
            case PlayerProperty.CurrentElo:
            // Directly use the current ELO points stored in EloManagement
            SetText(EloManagement.Instance.CurrentEloPoints.ToString());
            break;

        case PlayerProperty.CurrentLeague:
            // Fetch the current league based on the current ELO
            LeagueSO currentLeague = LeagueManager.Instance.GetCurrentLeague(EloManagement.Instance.CurrentEloPoints);
            if (currentLeague != null)
            {
                SetText(currentLeague.leagueName);
            }
            break;

        case PlayerProperty.CurrentSubLeague:
            // Fetch the current subleague based on the current ELO
            LeagueSO currentSubLeague = LeagueManager.Instance.GetCurrentLeague(EloManagement.Instance.CurrentEloPoints);
            if (currentSubLeague != null)
            {
                SetText(currentSubLeague.subLeagueName);
            }
            break;

        case PlayerProperty.CurrentLeagueIcon:
            // Fetch the league icon based on the current ELO
            LeagueSO leagueWithIcon = LeagueManager.Instance.GetCurrentLeague(EloManagement.Instance.CurrentEloPoints);
            Image myImage = GetComponent<Image>();
            if (myImage != null && leagueWithIcon != null)
            {
                myImage.sprite = leagueWithIcon.leagueSprite;
            }
            break;
        }
    }
}
