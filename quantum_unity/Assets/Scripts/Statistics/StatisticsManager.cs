using UnityEngine;
using System.Threading.Tasks;
using EdjCase.ICP.Candid.Models;
using System.Threading.Tasks;
using CanisterPK.CanisterStats.Models;
using Candid;
using System;
using System.Numerics;

public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional based on your game's needs
        }
    }

    private async void Start()
    {
        await FetchPlayerStatistics();
        // Example call to send test statistics
        await SendTestGameStatistics();
    }
    
    public async Task<bool> SaveFinishedGameStatistics(BigInteger gameID, string characterID, BigInteger botDifficulty, 
        BigInteger botMode, double damageCritic, double damageDealt, double damageEvaded, double damageTaken, 
        double deploys, double energyChargeRate, double energyGenerated, double energyUsed, double energyWasted, 
        BigInteger faction, BigInteger gameMode, double kills, double secRemaining, bool wonGame, double xpEarned)
    {
        // Convert BigInteger to UnboundedUInt
        UnboundedUInt gameIDUnbounded = UnboundedUInt.FromBigInteger(gameID);
        UnboundedUInt botDifficultyUnbounded = UnboundedUInt.FromBigInteger(botDifficulty);
        UnboundedUInt botModeUnbounded = UnboundedUInt.FromBigInteger(botMode);
        UnboundedUInt factionUnbounded = UnboundedUInt.FromBigInteger(faction);
        UnboundedUInt gameModeUnbounded = UnboundedUInt.FromBigInteger(gameMode);

        BasicStats stats = new BasicStats
        {
            BotDifficulty = botDifficultyUnbounded,
            BotMode = botModeUnbounded,
            CharacterID = characterID,
            DamageCritic = damageCritic,
            DamageDealt = damageDealt,
            DamageEvaded = damageEvaded,
            DamageTaken = damageTaken,
            Deploys = deploys,
            EnergyChargeRate = energyChargeRate,
            EnergyGenerated = energyGenerated,
            EnergyUsed = energyUsed,
            EnergyWasted = energyWasted,
            Faction = factionUnbounded,
            GameMode = gameModeUnbounded,
            Kills = kills,
            SecRemaining = secRemaining,
            WonGame = wonGame,
            XpEarned = xpEarned
        };

        // Call the canister to save the game statistics
        var result = await CandidApiManager.Instance.CanisterStats.SaveFinishedGame(gameIDUnbounded, stats);

        Debug.Log($"Game stats saved for Game ID {gameID}: {result.ReturnArg0}");
        return result.ReturnArg0;
    }

    public async Task GetPlayerStats(string principalId)
    {
        try
        {
            Principal playerPrincipal = Principal.FromText(principalId);

            var playerStatsOpt = await CandidApiManager.Instance.CanisterStats.GetPlayerStats(playerPrincipal);

            // Use GetValueOrDefault() to safely access the value
            var playerStats = playerStatsOpt.GetValueOrDefault();
            if (playerStats != null) // Check if the playerStats is not the default value
            {
                Debug.Log($"Player Stats for {principalId}:");

                // Basic stats
                Debug.Log($"Energy Generated: {playerStats.EnergyGenerated}");
                Debug.Log($"Energy Used: {playerStats.EnergyUsed}");
                Debug.Log($"Energy Wasted: {playerStats.EnergyWasted}");
                Debug.Log($"Games Lost: {playerStats.GamesLost}");
                Debug.Log($"Games Played: {playerStats.GamesPlayed}");
                Debug.Log($"Games Won: {playerStats.GamesWon}");
                Debug.Log($"Total Damage Crit: {playerStats.TotalDamageCrit}");
                Debug.Log($"Total Damage Dealt: {playerStats.TotalDamageDealt}");
                Debug.Log($"Total Damage Evaded: {playerStats.TotalDamageEvaded}");
                Debug.Log($"Total Damage Taken: {playerStats.TotalDamageTaken}");
                Debug.Log($"Total XP Earned: {playerStats.TotalXpEarned}");

                // Game Mode specific stats
                foreach (var gameModeStats in playerStats.TotalGamesGameMode)
                {
                    Debug.Log($"Game Mode ID: {gameModeStats.GameModeID}, Games Played: {gameModeStats.GamesPlayed}, Games Won: {gameModeStats.GamesWon}");
                }

                // Character specific stats
                foreach (var charStats in playerStats.TotalGamesWithCharacter)
                {
                    Debug.Log($"Character ID: {charStats.CharacterID}, Games Played: {charStats.GamesPlayed}, Games Won: {charStats.GamesWon}");
                }

                // Faction specific stats
                foreach (var factionStats in playerStats.TotalGamesWithFaction)
                {
                    Debug.Log($"Faction ID: {factionStats.FactionID}, Games Played: {factionStats.GamesPlayed}, Games Won: {factionStats.GamesWon}");
                }
            }
            else
            {
                Debug.LogError($"No stats found for player with principal ID: {principalId}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to fetch player stats for {principalId} due to an error: {ex.Message}");
        }
    }

    private async Task FetchPlayerStatistics()
    {
        string userPrincipalId = GlobalGameData.Instance.GetUserData().WalletId;
        await GetPlayerStats(userPrincipalId);
    }

private async Task SendTestGameStatistics()
    {
        // Example usage of BigInteger for game statistics
        BigInteger gameID = new BigInteger(DateTime.Now.Ticks);
        string characterID = "exampleCharacterID";
        BigInteger botDifficulty = BigInteger.One;
        BigInteger botMode = BigInteger.One;
        double damageCritic = 200.5;
        double damageDealt = 5000;
        double damageEvaded = 300;
        double damageTaken = 400;
        double deploys = 5;
        double energyChargeRate = 1.2;
        double energyGenerated = 500;
        double energyUsed = 300;
        double energyWasted = 200;
        BigInteger faction = BigInteger.One;
        BigInteger gameMode = BigInteger.One;
        double kills = 15;
        double secRemaining = 120;
        bool wonGame = true;
        double xpEarned = 1000;

        await SaveFinishedGameStatistics(gameID, characterID, botDifficulty, botMode, damageCritic, damageDealt, 
            damageEvaded, damageTaken, deploys, energyChargeRate, energyGenerated, energyUsed, energyWasted, 
            faction, gameMode, kills, secRemaining, wonGame, xpEarned);
    }
}
