using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class LeagueManager : MonoBehaviour
{
    public static LeagueManager Instance { get; private set; }
    public List<LeagueSO> leagues;
    public Image leagueSpriteImage;
    public TMP_Text leagueNameText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnEnable()
    {
        Debug.Log("Subscribing to OnLeagueChanged event.");
        EloManagement.Instance.OnLeagueChanged += UpdateLeagueUI;
    }

    void OnDisable()
    {
        Debug.Log("Unsubscribing from OnLeagueChanged event.");
        EloManagement.Instance.OnLeagueChanged -= UpdateLeagueUI;
    }

    private void HandleLeagueChange(LeagueSO newLeague)
    {
        // Show UI panel or update existing UI with newLeague information
    }

    public LeagueSO GetCurrentLeague(double elo)
    {
        return leagues.FirstOrDefault(l => elo >= l.minElo && elo < l.maxElo);
    }

    private void UpdateLeagueUI(LeagueSO newLeague)
    {
        if (newLeague != null)
        {
            leagueNameText.text = newLeague.leagueName;
            leagueSpriteImage.sprite = newLeague.leagueSprite;
        }
    }
}
