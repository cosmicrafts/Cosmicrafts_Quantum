using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cosmicrafts.Data;

public enum PlayerProperty
{
    Name,
    PrincipalId,
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
    public PlayerProperty Property;

    private PlayerData playerData;

    async void Start()
    {
        playerData = await AsyncDataManager.LoadPlayerDataAsync();
        LoadProperty();
    }

    void OnDestroy()
    {
        LoadProperty();
    }

    public void SetText(string text)
    {
        Text myText = GetComponent<Text>();
        if (myText != null) { myText.text = text; }

        TMP_Text myTmp = GetComponent<TMP_Text>();
        if (myTmp != null) { myTmp.text = text; }
    }

    public void LoadProperty()
    {
        if (playerData == null) { return; }

        switch (Property)
        {
            case PlayerProperty.Name:
                SetText(playerData.Username);
                break;
            case PlayerProperty.PrincipalId:
                SetText(Utils.GetWalletIDShort(playerData.PrincipalId));
                break;
            case PlayerProperty.Level:
                SetText($"{Lang.GetText("mn_lvl")} {playerData.Level}");
                break;
            case PlayerProperty.Xp:
                SetText($"Error XP");
                break;
            case PlayerProperty.XpProgress:
                SetText($"XX/XX");
                break;
            case PlayerProperty.Xpbar:
                Debug.Log("Error XPBar UIPTxtInfo");
                break;
            case PlayerProperty.Character:
                Debug.Log("Error CharacterImage UIPTxtInfo");
                break;
            case PlayerProperty.CharacterName:
                SetText(Lang.GetEntityName(playerData.CharacterNFTId.ToString()));
                break;
            case PlayerProperty.Avatar:
                Debug.Log("Error Avatar UIPTxtInfo");
                break;
            case PlayerProperty.Score:
                SetText("ErrorScore");
                break;
            case PlayerProperty.Emblem:
                Debug.Log("Error Emblem UIPTxtInfo");
                break;
            case PlayerProperty.Description:
                Debug.Log("Error Description UIPTxtInfo");
                break;
            case PlayerProperty.CurrentElo:
                SetText(EloManagement.Instance.CurrentEloPoints.ToString());
                break;
            case PlayerProperty.CurrentLeague:
                LeagueSO currentLeague = LeagueManager.Instance.GetCurrentLeague(EloManagement.Instance.CurrentEloPoints);
                if (currentLeague != null)
                {
                    SetText(currentLeague.leagueName);
                }
                break;
            case PlayerProperty.CurrentSubLeague:
                LeagueSO currentSubLeague = LeagueManager.Instance.GetCurrentLeague(EloManagement.Instance.CurrentEloPoints);
                if (currentSubLeague != null)
                {
                    SetText(currentSubLeague.subLeagueName);
                }
                break;
            case PlayerProperty.CurrentLeagueIcon:
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
