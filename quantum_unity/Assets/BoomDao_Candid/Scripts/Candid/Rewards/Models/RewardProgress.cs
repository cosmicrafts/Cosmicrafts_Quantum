using EdjCase.ICP.Candid.Mapping;
using CanisterPK.Rewards.Models;

namespace CanisterPK.Rewards.Models
{
	public class RewardProgress
	{
		[CandidName("progress")]
		public double Progress { get; set; }

		[CandidName("rewardType")]
		public RewardType RewardType { get; set; }

		public RewardProgress(double progress, RewardType rewardType)
		{
			this.Progress = progress;
			this.RewardType = rewardType;
		}

		public RewardProgress()
		{
		}
	}
}