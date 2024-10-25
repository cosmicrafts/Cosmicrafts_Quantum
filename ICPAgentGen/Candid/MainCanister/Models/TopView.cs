using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Cosmicrafts.MainCanister.Models;

namespace Cosmicrafts.MainCanister.Models
{
	public class TopView
	{
		[CandidName("achTop")]
		public List<AchievementsTop> AchTop { get; set; }

		[CandidName("eloTop")]
		public List<ELOTop> EloTop { get; set; }

		[CandidName("levelTop")]
		public List<LevelTop> LevelTop { get; set; }

		[CandidName("nftTop")]
		public List<NFTTop> NftTop { get; set; }

		[CandidName("referralsTop")]
		public List<ReferralsTop> ReferralsTop { get; set; }

		public TopView(List<AchievementsTop> achTop, List<ELOTop> eloTop, List<LevelTop> levelTop, List<NFTTop> nftTop, List<ReferralsTop> referralsTop)
		{
			this.AchTop = achTop;
			this.EloTop = eloTop;
			this.LevelTop = levelTop;
			this.NftTop = nftTop;
			this.ReferralsTop = referralsTop;
		}

		public TopView()
		{
		}
	}
}