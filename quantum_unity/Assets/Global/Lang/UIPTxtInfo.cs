using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cosmicrafts.Data;
using Cosmicrafts.Managers;

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
    CurrentLeagueIcon,
    Friends,
    RegistrationDate,
    Email
}

public class UIPTxtInfo : MonoBehaviour
{
    public PlayerProperty Property;

    private PlayerData playerData;

    void Start()
    {
        if (GameDataManager.Instance != null)
        {
            playerData = GameDataManager.Instance.playerData;
            LoadProperty();
        }
        else
        {
            Debug.LogError("[UIPTxtInfo] GameDataManager instance is null in Start.");
        }
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

private string ConvertUnixTimestampToDateString(long unixTimestampInNanoseconds)
{
    try
    {
        // Convert nanoseconds to milliseconds
        long unixTimestampInMilliseconds = unixTimestampInNanoseconds / 1_000_000;

        // Ensure the timestamp is within the valid range for DateTimeOffset
        if (unixTimestampInMilliseconds < -62135596800000 || unixTimestampInMilliseconds > 253402300799999)
        {
            Debug.LogError($"Unix timestamp is out of range for DateTimeOffset: {unixTimestampInMilliseconds}");
            return "Invalid Date";
        }

        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTimestampInMilliseconds);
        return dateTimeOffset.ToString("MMMM yyyy"); // Format as Month and Year
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Debug.LogError($"Unix timestamp is out of range for DateTimeOffset: {ex.Message}");
        return "Invalid Date";
    }
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
                SetText(playerData.AvatarID.ToString());  // Displaying avatar ID as text, update this as needed
                break;
            case PlayerProperty.Score:
                SetText("ErrorScore");
                break;
            case PlayerProperty.Emblem:
                Debug.Log("Error Emblem UIPTxtInfo");
                break;
            case PlayerProperty.Description:
                SetText(playerData.Description);
                break;
            case PlayerProperty.CurrentElo:
                SetText(playerData.Elo.ToString());
                break;
            case PlayerProperty.CurrentLeague:
                LeagueSO currentLeague = LeagueManager.Instance.GetCurrentLeague(playerData.Elo);
                if (currentLeague != null)
                {
                    SetText(currentLeague.leagueName);
                }
                break;
            case PlayerProperty.CurrentSubLeague:
                LeagueSO currentSubLeague = LeagueManager.Instance.GetCurrentLeague(playerData.Elo);
                if (currentSubLeague != null)
                {
                    SetText(currentSubLeague.subLeagueName);
                }
                break;
            case PlayerProperty.CurrentLeagueIcon:
                LeagueSO leagueWithIcon = LeagueManager.Instance.GetCurrentLeague(playerData.Elo);
                Image myImage = GetComponent<Image>();
                if (myImage != null && leagueWithIcon != null)
                {
                    myImage.sprite = leagueWithIcon.leagueSprite;
                }
                break;
            case PlayerProperty.Friends:
                SetText(playerData.Friends.Count.ToString());  // Displaying the number of friends as text
                break;
            case PlayerProperty.RegistrationDate:
                SetText(ConvertUnixTimestampToDateString(playerData.RegistrationDate));
                break;
            case PlayerProperty.Email:
                SetText(playerData.Email);
                break;
        }
    }
}
