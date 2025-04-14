using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using Cosmicrafts.backend.Models;

namespace Cosmicrafts.backend.Models
{
	public class PlayerGamesStats
	{
		[CandidName("energyGenerated")]
		public UnboundedUInt EnergyGenerated { get; set; }

		[CandidName("energyUsed")]
		public UnboundedUInt EnergyUsed { get; set; }

		[CandidName("energyWasted")]
		public UnboundedUInt EnergyWasted { get; set; }

		[CandidName("gamesLost")]
		public UnboundedUInt GamesLost { get; set; }

		[CandidName("gamesPlayed")]
		public UnboundedUInt GamesPlayed { get; set; }

		[CandidName("gamesWon")]
		public UnboundedUInt GamesWon { get; set; }

		[CandidName("totalDamageCrit")]
		public UnboundedUInt TotalDamageCrit { get; set; }

		[CandidName("totalDamageDealt")]
		public UnboundedUInt TotalDamageDealt { get; set; }

		[CandidName("totalDamageEvaded")]
		public UnboundedUInt TotalDamageEvaded { get; set; }

		[CandidName("totalDamageTaken")]
		public UnboundedUInt TotalDamageTaken { get; set; }

		[CandidName("totalGamesGameMode")]
		public List<GamesWithGameMode> TotalGamesGameMode { get; set; }

		[CandidName("totalGamesWithCharacter")]
		public List<GamesWithCharacter> TotalGamesWithCharacter { get; set; }

		[CandidName("totalGamesWithFaction")]
		public List<GamesWithFaction> TotalGamesWithFaction { get; set; }

		[CandidName("totalKills")]
		public UnboundedUInt TotalKills { get; set; }

		[CandidName("totalXpEarned")]
		public UnboundedUInt TotalXpEarned { get; set; }

		public PlayerGamesStats(UnboundedUInt energyGenerated, UnboundedUInt energyUsed, UnboundedUInt energyWasted, UnboundedUInt gamesLost, UnboundedUInt gamesPlayed, UnboundedUInt gamesWon, UnboundedUInt totalDamageCrit, UnboundedUInt totalDamageDealt, UnboundedUInt totalDamageEvaded, UnboundedUInt totalDamageTaken, List<GamesWithGameMode> totalGamesGameMode, List<GamesWithCharacter> totalGamesWithCharacter, List<GamesWithFaction> totalGamesWithFaction, UnboundedUInt totalKills, UnboundedUInt totalXpEarned)
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
			this.TotalKills = totalKills;
			this.TotalXpEarned = totalXpEarned;
		}

		public PlayerGamesStats()
		{
		}
	}
}