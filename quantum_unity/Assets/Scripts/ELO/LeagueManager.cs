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
    public TMP_Text subLeagueNameText;

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
        StartCoroutine(SubscribeToEventWithDelay());
    }

    void OnDisable()
    {
        Debug.Log("Unsubscribing from OnLeagueChanged event.");
        if (EloManagement.Instance != null)
        {
            EloManagement.Instance.OnLeagueChanged -= UpdateLeagueUI;
        }
    }

    private IEnumerator SubscribeToEventWithDelay()
    {
        yield return new WaitForSeconds(1f);

        Debug.Log("Subscribing to OnLeagueChanged event.");
        if (EloManagement.Instance != null)
        {
            EloManagement.Instance.OnLeagueChanged += UpdateLeagueUI;
        }
        else
        {
            Debug.LogError("EloManagement.Instance is null.");
        }
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
            subLeagueNameText.text = newLeague.subLeagueName;
        }
    }
}
