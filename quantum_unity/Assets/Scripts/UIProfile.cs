using Candid;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;
using Cosmicrafts.Managers;

public class UIProfile : MonoBehaviour
{
    [Header("Overview")] 
    public TMP_Text since;
    public TMP_Text timePlayed;
    public TMP_Text blockChain;
    public TMP_Text gamesWins;
    public TMP_Text gamesPlayed;
    [Header("Statistics")]
    public TMP_Text damageDealt;
    public TMP_Text damageEvaded;
    public TMP_Text criticalDamage;
    public TMP_Text damageTaken;
    public TMP_Text xpEarned;
    
    public TMP_Text energyGenerated;
    public TMP_Text energyUsed;
    public TMP_Text energyWasted;

    private async void Start()
    {
        await GetInfoToProfile();
    }

    public async void OpenProfile()
    {
        Debug.Log("Profile opened");
        await GetInfoToProfile();
    }

    public async Task GetInfoToProfile()
    {
        Debug.Log("Fetching profile info");
        var playerGameStats = await CandidApiManager.Instance.CanisterLogin.GetMyStats();
        
        if (playerGameStats.HasValue)
        {
            var playerGameStatsValue = playerGameStats.ValueOrDefault;

            since.text = "";
            timePlayed.text = "";
            blockChain.text = "";
            gamesPlayed.text = playerGameStatsValue.GamesPlayed.ToString();
            gamesWins.text = playerGameStatsValue.GamesWon.ToString();
            
            damageDealt.text = playerGameStatsValue.TotalDamageDealt.ToString();
            damageEvaded.text = playerGameStatsValue.TotalDamageEvaded.ToString();
            criticalDamage.text = playerGameStatsValue.TotalDamageCrit.ToString();
            damageTaken.text = playerGameStatsValue.TotalDamageTaken.ToString();
            xpEarned.text = playerGameStatsValue.TotalXpEarned.ToString();
            
            energyGenerated.text = playerGameStatsValue.EnergyGenerated.ToString();
            energyUsed.text = playerGameStatsValue.EnergyUsed.ToString();
            energyWasted.text = playerGameStatsValue.EnergyWasted.ToString();
        }
        else
        {
            Debug.Log("No stats information available");
        }
    }
}
