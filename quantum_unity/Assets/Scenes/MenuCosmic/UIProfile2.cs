 using System;
 using System.Collections;
using System.Collections.Generic;
 using System.Globalization;
 using Candid;
 using TMPro;
 using UnityEngine;

public class UIProfile2 : MonoBehaviour
{

    [Header("GameObjects")] 
    public GameObject loading;
    public GameObject error;
    public GameObject content;

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
    public TMP_Text xpEarned ;
    
    public TMP_Text energyGenerated;
    public TMP_Text energyUsed;
    public TMP_Text energyWasted;


    private async void Start()
    {
        GetInfoToProfile();
    }

    public void OpenProfile()
    {
        Debug.Log("awake");
        loading.SetActive(true);
        error.SetActive(false);
        content.SetActive(false);
        GetInfoToProfile();
    }

    public async void GetInfoToProfile()
    {
        Debug.Log("GetInfoToProfile");
        var playerGameStats = await CandidApiManager.Instance.CanisterStats.GetMyStats();
        
        if (playerGameStats.HasValue)
        {
            var playerGameStatsValue = playerGameStats.ValueOrDefault;

            since.text = "";
            timePlayed.text = "";
            blockChain.text = "";
            gamesPlayed.text = playerGameStatsValue.GamesPlayed.ToString();
            gamesWins.text = playerGameStatsValue.GamesWon.ToString();
            
            damageDealt.text = playerGameStatsValue.TotalDamageDealt.ToString(CultureInfo.InvariantCulture);
            damageEvaded.text = playerGameStatsValue.TotalDamageEvaded.ToString(CultureInfo.InvariantCulture);
            criticalDamage.text = playerGameStatsValue.TotalDamageCrit.ToString(CultureInfo.InvariantCulture);
            damageTaken.text = playerGameStatsValue.TotalDamageTaken.ToString(CultureInfo.InvariantCulture);
            xpEarned.text = playerGameStatsValue.TotalXpEarned.ToString(CultureInfo.InvariantCulture);
            
            energyGenerated.text = playerGameStatsValue.EnergyGenerated.ToString(CultureInfo.InvariantCulture);
            energyUsed.text = playerGameStatsValue.EnergyUsed.ToString(CultureInfo.InvariantCulture);
            energyWasted.text = playerGameStatsValue.EnergyWasted.ToString(CultureInfo.InvariantCulture);
            
            //loading.SetActive(false);
            //content.SetActive(true);
        }
        else
        {
            //loading.SetActive(false);
            //error.SetActive(true);
            Debug.Log("No hay info de Stats");
        }
        
            
            
            
    }

    
}
