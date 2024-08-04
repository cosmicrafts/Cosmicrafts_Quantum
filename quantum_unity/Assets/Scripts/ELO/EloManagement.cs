using System.Threading.Tasks;
using System;
using UnityEngine;
using Candid;
using EdjCase.ICP.Candid.Models;
using CanisterPK.CanisterLogin;
using CanisterPK.CanisterLogin.Models;
using TMPro;
using Cosmicrafts.Data;

public class EloManagement : MonoBehaviour
{
    public static EloManagement Instance { get; private set; }
    public double CurrentEloPoints { get; private set; }
    public TMP_Text eloText;
    public event Action<LeagueSO> OnLeagueChanged;
    private LeagueSO currentLeague;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("EloManagement instance already exists. Destroying this instance.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private async void Start()
    {
        await UpdatePlayerElo();
    }

    public async Task UpdatePlayerElo()
    {
        var userData = await AsyncDataManager.LoadPlayerDataAsync();
        if (userData == null)
        {
            Debug.LogError("Failed to load player data.");
            return;
        }

        Principal playerPrincipal = Principal.FromText(userData.PrincipalId);

        try
        {
            double eloPoints = await CandidApiManager.Instance.CanisterLogin.GetPlayerElo(playerPrincipal);
            // Round down the Elo points
            int roundedEloPoints = (int)Math.Floor(eloPoints);
            CurrentEloPoints = roundedEloPoints;
            Debug.Log($"Updated Elo Points: {roundedEloPoints}");

            // Update UI element with new rounded down Elo points
            if (eloText != null) eloText.text = $"{roundedEloPoints}";
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to update player Elo: {ex.Message}");
        }

        // Determine if the league has changed
        LeagueSO newLeague = LeagueManager.Instance.GetCurrentLeague(CurrentEloPoints);
        if (newLeague != currentLeague)
        {
            currentLeague = newLeague;
            // Fire the event with the new LeagueSO
            OnLeagueChanged?.Invoke(newLeague);
            Debug.Log($"Welcome to {newLeague.leagueName} League!");
        }
    }
}
