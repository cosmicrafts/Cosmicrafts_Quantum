using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Cosmicrafts.MainCanister.Models;

namespace Cosmicrafts.MainCanister.Models
{
	public class BasicStats
	{
		[CandidName("playerStats")]
		public List<PlayerStats> PlayerStats { get; set; }

		public BasicStats(List<PlayerStats> playerStats)
		{
			this.PlayerStats = playerStats;
		}

		public BasicStats()
		{
		}
	}
}