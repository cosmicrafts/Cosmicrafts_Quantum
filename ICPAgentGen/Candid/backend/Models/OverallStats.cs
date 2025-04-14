using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using Cosmicrafts.backend.Models;

namespace Cosmicrafts.backend.Models
{
	public class OverallStats
	{
		[CandidName("totalDamageDealt")]
		public UnboundedUInt TotalDamageDealt { get; set; }

		[CandidName("totalEnergyGenerated")]
		public UnboundedUInt TotalEnergyGenerated { get; set; }

		[CandidName("totalEnergyUsed")]
		public UnboundedUInt TotalEnergyUsed { get; set; }

		[CandidName("totalEnergyWasted")]
		public UnboundedUInt TotalEnergyWasted { get; set; }

		[CandidName("totalGamesGameMode")]
		public List<OverallGamesWithGameMode> TotalGamesGameMode { get; set; }

		[CandidName("totalGamesMP")]
		public UnboundedUInt TotalGamesMP { get; set; }

		[CandidName("totalGamesPlayed")]
		public UnboundedUInt TotalGamesPlayed { get; set; }

		[CandidName("totalGamesSP")]
		public UnboundedUInt TotalGamesSP { get; set; }

		[CandidName("totalGamesWithCharacter")]
		public List<OverallGamesWithCharacter> TotalGamesWithCharacter { get; set; }

		[CandidName("totalGamesWithFaction")]
		public List<OverallGamesWithFaction> TotalGamesWithFaction { get; set; }

		[CandidName("totalKills")]
		public UnboundedUInt TotalKills { get; set; }

		[CandidName("totalTimePlayed")]
		public UnboundedUInt TotalTimePlayed { get; set; }

		[CandidName("totalXpEarned")]
		public UnboundedUInt TotalXpEarned { get; set; }

		public OverallStats(UnboundedUInt totalDamageDealt, UnboundedUInt totalEnergyGenerated, UnboundedUInt totalEnergyUsed, UnboundedUInt totalEnergyWasted, List<OverallGamesWithGameMode> totalGamesGameMode, UnboundedUInt totalGamesMP, UnboundedUInt totalGamesPlayed, UnboundedUInt totalGamesSP, List<OverallGamesWithCharacter> totalGamesWithCharacter, List<OverallGamesWithFaction> totalGamesWithFaction, UnboundedUInt totalKills, UnboundedUInt totalTimePlayed, UnboundedUInt totalXpEarned)
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