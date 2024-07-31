using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using CanisterPK.CanisterLogin.Models;

namespace CanisterPK.CanisterLogin.Models
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