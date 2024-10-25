using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Cosmicrafts.MainCanister.Models;

namespace Cosmicrafts.MainCanister.Models
{
	public class BasicStats
	{
		[CandidName("playerStats")]
		public List<Playerstats1> PlayerStats { get; set; }

		public BasicStats(List<Playerstats1> playerStats)
		{
			this.PlayerStats = playerStats;
		}

		public BasicStats()
		{
		}
	}
}