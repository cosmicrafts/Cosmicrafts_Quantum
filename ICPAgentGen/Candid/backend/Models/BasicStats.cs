using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Cosmicrafts.backend.Models;

namespace Cosmicrafts.backend.Models
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