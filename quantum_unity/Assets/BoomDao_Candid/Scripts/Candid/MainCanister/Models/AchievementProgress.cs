using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Playerid1 = EdjCase.ICP.Candid.Models.Principal;
using PlayerId = EdjCase.ICP.Candid.Models.Principal;

namespace Cosmicrafts.MainCanister.Models
{
	public class AchievementProgress
	{
		[CandidName("achievementId")]
		public UnboundedUInt AchievementId { get; set; }

		[CandidName("completed")]
		public bool Completed { get; set; }

		[CandidName("playerId")]
		public Playerid1 PlayerId { get; set; }

		[CandidName("progress")]
		public UnboundedUInt Progress { get; set; }

		public AchievementProgress(UnboundedUInt achievementId, bool completed, Playerid1 playerId, UnboundedUInt progress)
		{
			this.AchievementId = achievementId;
			this.Completed = completed;
			this.PlayerId = playerId;
			this.Progress = progress;
		}

		public AchievementProgress()
		{
		}
	}
}