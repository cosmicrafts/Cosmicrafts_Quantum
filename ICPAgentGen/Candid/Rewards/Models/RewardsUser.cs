using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using CanisterPK.Rewards.Models;

namespace CanisterPK.Rewards.Models
{
	public class RewardsUser
	{
		[CandidName("expiration")]
		public ulong Expiration { get; set; }

		[CandidName("finish_date")]
		public ulong FinishDate { get; set; }

		[CandidName("finished")]
		public bool Finished { get; set; }

		[CandidName("id_reward")]
		public UnboundedUInt IdReward { get; set; }

		[CandidName("prize_amount")]
		public UnboundedUInt PrizeAmount { get; set; }

		[CandidName("prize_type")]
		public PrizeType PrizeType { get; set; }

		[CandidName("progress")]
		public double Progress { get; set; }

		[CandidName("rewardType")]
		public RewardType RewardType { get; set; }

		[CandidName("start_date")]
		public ulong StartDate { get; set; }

		[CandidName("total")]
		public double Total { get; set; }

		public RewardsUser(ulong expiration, ulong finishDate, bool finished, UnboundedUInt idReward, UnboundedUInt prizeAmount, PrizeType prizeType, double progress, RewardType rewardType, ulong startDate, double total)
		{
			this.Expiration = expiration;
			this.FinishDate = finishDate;
			this.Finished = finished;
			this.IdReward = idReward;
			this.PrizeAmount = prizeAmount;
			this.PrizeType = prizeType;
			this.Progress = progress;
			this.RewardType = rewardType;
			this.StartDate = startDate;
			this.Total = total;
		}

		public RewardsUser()
		{
		}
	}
}