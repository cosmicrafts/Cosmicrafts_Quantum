using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.backend.Models;

namespace Cosmicrafts.backend.Models
{
	public class AchievementReward
	{
		[CandidName("amount")]
		public UnboundedUInt Amount { get; set; }

		[CandidName("rewardType")]
		public AchievementRewardsType RewardType { get; set; }

		public AchievementReward(UnboundedUInt amount, AchievementRewardsType rewardType)
		{
			this.Amount = amount;
			this.RewardType = rewardType;
		}

		public AchievementReward()
		{
		}
	}
}