using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using CanisterPK.Rewards.Models;

namespace CanisterPK.Rewards.Models
{
	public class Reward
	{
		[CandidName("end_date")]
		public ulong EndDate { get; set; }

		[CandidName("id")]
		public UnboundedUInt Id { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		[CandidName("prize_amount")]
		public UnboundedUInt PrizeAmount { get; set; }

		[CandidName("prize_type")]
		public PrizeType PrizeType { get; set; }

		[CandidName("rewardType")]
		public RewardType RewardType { get; set; }

		[CandidName("start_date")]
		public ulong StartDate { get; set; }

		[CandidName("total")]
		public double Total { get; set; }

		public Reward(ulong endDate, UnboundedUInt id, string name, UnboundedUInt prizeAmount, PrizeType prizeType, RewardType rewardType, ulong startDate, double total)
		{
			this.EndDate = endDate;
			this.Id = id;
			this.Name = name;
			this.PrizeAmount = prizeAmount;
			this.PrizeType = prizeType;
			this.RewardType = rewardType;
			this.StartDate = startDate;
			this.Total = total;
		}

		public Reward()
		{
		}
	}
}