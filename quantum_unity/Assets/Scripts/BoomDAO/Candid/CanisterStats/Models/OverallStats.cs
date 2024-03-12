using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using CanisterPK.CanisterStats.Models;
using EdjCase.ICP.Candid.Models;

namespace CanisterPK.CanisterStats.Models
{
	public class OverallStats
	{
		[CandidName("totalDamageDealt")]
		public double TotalDamageDealt { get; set; }

		[CandidName("totalEnergyGenerated")]
		public double TotalEnergyGenerated { get; set; }

		[CandidName("totalEnergyUsed")]
		public double TotalEnergyUsed { get; set; }

		[CandidName("totalEnergyWasted")]
		public double TotalEnergyWasted { get; set; }

		[CandidName("totalGamesGameMode")]
		public List<GamesWithGameMode> TotalGamesGameMode { get; set; }

		[CandidName("totalGamesMP")]
		public UnboundedUInt TotalGamesMP { get; set; }

		[CandidName("totalGamesPlayed")]
		public UnboundedUInt TotalGamesPlayed { get; set; }

		[CandidName("totalGamesSP")]
		public UnboundedUInt TotalGamesSP { get; set; }

		[CandidName("totalGamesWithCharacter")]
		public List<GamesWithCharacter> TotalGamesWithCharacter { get; set; }

		[CandidName("totalGamesWithFaction")]
		public List<GamesWithFaction> TotalGamesWithFaction { get; set; }

		[CandidName("totalKills")]
		public double TotalKills { get; set; }

		[CandidName("totalTimePlayed")]
		public double TotalTimePlayed { get; set; }

		[CandidName("totalXpEarned")]
		public double TotalXpEarned { get; set; }

		public OverallStats(double totalDamageDealt, double totalEnergyGenerated, double totalEnergyUsed, double totalEnergyWasted, List<GamesWithGameMode> totalGamesGameMode, UnboundedUInt totalGamesMP, UnboundedUInt totalGamesPlayed, UnboundedUInt totalGamesSP, List<GamesWithCharacter> totalGamesWithCharacter, List<GamesWithFaction> totalGamesWithFaction, double totalKills, double totalTimePlayed, double totalXpEarned)
		{
			this.TotalDamageDealt = totalDamageDealt;
			this.TotalEnergyGenerated = totalEnergyGenerated;
			this.TotalEnergyUsed = totalEnergyUsed;
			this.TotalEnergyWasted = totalEnergyWasted;
			this.TotalGamesGameMode = totalGamesGameMode;
			this.TotalGamesMP = totalGamesMP;
			this.TotalGamesPlayed = totalGamesPlayed;
			this.TotalGamesSP = totalGamesSP;
			this.TotalGamesWithCharacter = totalGamesWithCharacter;
			this.TotalGamesWithFaction = totalGamesWithFaction;
			this.TotalKills = totalKills;
			this.TotalTimePlayed = totalTimePlayed;
			this.TotalXpEarned = totalXpEarned;
		}

		public OverallStats()
		{
		}
	}
}