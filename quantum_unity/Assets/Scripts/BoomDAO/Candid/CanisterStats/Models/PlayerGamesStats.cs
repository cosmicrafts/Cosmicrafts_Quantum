using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using CanisterPK.CanisterStats.Models;

namespace CanisterPK.CanisterStats.Models
{
	public class PlayerGamesStats
	{
		[CandidName("energyGenerated")]
		public double EnergyGenerated { get; set; }

		[CandidName("energyUsed")]
		public double EnergyUsed { get; set; }

		[CandidName("energyWasted")]
		public double EnergyWasted { get; set; }

		[CandidName("gamesLost")]
		public UnboundedUInt GamesLost { get; set; }

		[CandidName("gamesPlayed")]
		public UnboundedUInt GamesPlayed { get; set; }

		[CandidName("gamesWon")]
		public UnboundedUInt GamesWon { get; set; }

		[CandidName("totalDamageCrit")]
		public double TotalDamageCrit { get; set; }

		[CandidName("totalDamageDealt")]
		public double TotalDamageDealt { get; set; }

		[CandidName("totalDamageEvaded")]
		public double TotalDamageEvaded { get; set; }

		[CandidName("totalDamageTaken")]
		public double TotalDamageTaken { get; set; }

		[CandidName("totalGamesGameMode")]
		public List<GamesWithGameMode> TotalGamesGameMode { get; set; }

		[CandidName("totalGamesWithCharacter")]
		public List<GamesWithCharacter> TotalGamesWithCharacter { get; set; }

		[CandidName("totalGamesWithFaction")]
		public List<GamesWithFaction> TotalGamesWithFaction { get; set; }

		[CandidName("totalXpEarned")]
		public double TotalXpEarned { get; set; }

		public PlayerGamesStats(double energyGenerated, double energyUsed, double energyWasted, UnboundedUInt gamesLost, UnboundedUInt gamesPlayed, UnboundedUInt gamesWon, double totalDamageCrit, double totalDamageDealt, double totalDamageEvaded, double totalDamageTaken, List<GamesWithGameMode> totalGamesGameMode, List<GamesWithCharacter> totalGamesWithCharacter, List<GamesWithFaction> totalGamesWithFaction, double totalXpEarned)
		{
			this.EnergyGenerated = energyGenerated;
			this.EnergyUsed = energyUsed;
			this.EnergyWasted = energyWasted;
			this.GamesLost = gamesLost;
			this.GamesPlayed = gamesPlayed;
			this.GamesWon = gamesWon;
			this.TotalDamageCrit = totalDamageCrit;
			this.TotalDamageDealt = totalDamageDealt;
			this.TotalDamageEvaded = totalDamageEvaded;
			this.TotalDamageTaken = totalDamageTaken;
			this.TotalGamesGameMode = totalGamesGameMode;
			this.TotalGamesWithCharacter = totalGamesWithCharacter;
			this.TotalGamesWithFaction = totalGamesWithFaction;
			this.TotalXpEarned = totalXpEarned;
		}

		public PlayerGamesStats()
		{
		}
	}
}