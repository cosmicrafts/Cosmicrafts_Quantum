using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EdjCase.ICP.Candid.Models;
using System.Numerics;
using Candid;

public class TournamentManager : MonoBehaviour
{
    public TMP_Dropdown tournamentDropdown; // Dropdown to list active tournaments

    // Start is called before the first frame update
    void Start()
    {
        // Query active tournaments when the script starts
        QueryActiveTournaments();
    }

    public async void QueryActiveTournaments()
    {
        Debug.Log("[TournamentManager] Querying active tournaments...");
        try
        {
            List<Cosmicrafts.MainCanister.Models.Tournament> activeTournaments = await CandidApiManager.Instance.MainCanister.GetActiveTournaments();
            UpdateTournamentDropdown(activeTournaments);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[TournamentManager] Error querying active tournaments: {e.Message}");
        }
    }

    private void UpdateTournamentDropdown(List<Cosmicrafts.MainCanister.Models.Tournament> tournaments)
    {
        // Clear existing options
        tournamentDropdown.ClearOptions();

        // Populate dropdown with active tournaments
        List<string> options = new List<string>();
        foreach (var tournament in tournaments)
        {
            options.Add($"{tournament.Id}: {tournament.Name}");
        }
        tournamentDropdown.AddOptions(options);

        Debug.Log("[TournamentManager] Tournament dropdown updated with active tournaments.");
    }

    public async void JoinSelectedTournament()
    {
        int selectedOptionIndex = tournamentDropdown.value;
        if (selectedOptionIndex >= 0)
        {
            string selectedOption = tournamentDropdown.options[selectedOptionIndex].text;
            string tournamentIdStr = selectedOption.Split(':')[0];
            try
            {
                BigInteger tournamentIdBigInt = BigInteger.Parse(tournamentIdStr);
                UnboundedUInt tournamentId = UnboundedUInt.FromBigInteger(tournamentIdBigInt);
                Debug.Log($"[TournamentManager] Attempting to join tournament ID: {tournamentId}");
                try
                {
                    bool success = await CandidApiManager.Instance.MainCanister.JoinTournament(tournamentId);
                    Debug.Log($"[TournamentManager] Join tournament response: {success}");
                    if (success)
                    {
                        Debug.Log("[TournamentManager] Successfully joined the tournament.");
                    }
                    else
                    {
                        Debug.LogWarning("[TournamentManager] Failed to join the tournament.");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[TournamentManager] Error joining tournament: {e.Message}");
                }
            }
            catch (System.FormatException e)
            {
                Debug.LogError($"[TournamentManager] Error parsing tournament ID: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning("[TournamentManager] No tournament selected.");
        }
    }
}
