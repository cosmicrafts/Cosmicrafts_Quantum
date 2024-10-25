using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.MainCanister.Models;

namespace Cosmicrafts.MainCanister.Models
{
	public class Achievementreward1
	{
		[CandidName("amount")]
		public UnboundedUInt Amount { get; set; }

		[CandidName("rewardType")]
		public AchievementRewardsType RewardType { get; set; }

		public Achievementreward1(UnboundedUInt amount, AchievementRewardsType rewardType)
		{
			this.Amount = amount;
			this.RewardType = rewardType;
		}

		public Achievementreward1()
		{
		}
	}
}