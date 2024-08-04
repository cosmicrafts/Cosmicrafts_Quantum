using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Cosmicrafts.Managers;

public class UIMainMenu : MonoBehaviour
{
    public TMP_Text walletIdText;
    public TMP_Text usernameText;
    public TMP_Text levelText;

    private bool getInfoFromCanister = false;
    List<UIPTxtInfo> UIPropertys;
    public UIMatchMaking uiMatchMaking;

    private void Awake()
    {
        UIPropertys = new List<UIPTxtInfo>();
        foreach (UIPTxtInfo prop in FindObjectsOfType<UIPTxtInfo>())
        {
            UIPropertys.Add(prop);
        }

        if (GameDataManager.Instance != null)
        {
            var playerData = GameDataManager.Instance.playerData;

            // Update UI elements with player data
            if (!string.IsNullOrEmpty(playerData.PrincipalId))
            {
                string walletId = playerData.PrincipalId;
                if (walletId.Length > 8) // Check if WalletID is long enough to shorten
                {
                    walletId = walletId.Substring(0, 5) + "..." + walletId.Substring(walletId.Length - 3);
                }
                walletIdText.text = walletId;
            }

            if (!string.IsNullOrEmpty(playerData.Username))
            {
                usernameText.text = playerData.Username;
            }

            levelText.text = playerData.Level.ToString();
        }
        else
        {
            Debug.LogError("[UIMainMenu] GameDataManager instance is null in Awake.");
        }

        if (getInfoFromCanister)
        {
            // GetInfoUserFromCanister();
        }
        else
        {
            LoadingPanel.Instance.DesactiveLoadingPanel();
        }
    }

    public void ChangeLang(int lang)
    {
        PlayerPrefs.SetInt("GameLanguage", lang);
        RefreshAllPropertys();
    }

    public void PlayCurrentMode()
    {
        if (GameDataManager.Instance != null)
        {
            var playerData = GameDataManager.Instance.playerData;
            string currentMatch = playerData.config.CurrentMatch.ToString();

            switch (currentMatch)
            {
                case "multi":
                    uiMatchMaking.StartSearch();
                    break;
                case "bots":
                    Debug.Log($"CURRENT MATCH: {currentMatch}");
                    break;
                default:
                    Debug.Log($"CURRENT MATCH: {currentMatch}");
                    break;
            }
        }
        else
        {
            Debug.LogError("[UIMainMenu] GameDataManager instance is null in PlayCurrentMode.");
        }
    }

    // Refresh a specific UI property of the player
    public void RefreshProperty(PlayerProperty property)
    {
        foreach (UIPTxtInfo prop in UIPropertys.Where(f => f.Property == property))
        {
            prop.LoadProperty();
        }
    }

    // Refresh all the UI references properties of the player
    public void RefreshAllPropertys()
    {
        foreach (UIPTxtInfo prop in UIPropertys)
        {
            prop.LoadProperty();
        }
    }
}
