using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.MainCanister.Models;

namespace Cosmicrafts.MainCanister.Models
{
	public class MissionsUser
	{
		[CandidName("expiration")]
		public ulong Expiration { get; set; }

		[CandidName("finish_date")]
		public ulong FinishDate { get; set; }

		[CandidName("finished")]
		public bool Finished { get; set; }

		[CandidName("id_mission")]
		public UnboundedUInt IdMission { get; set; }

		[CandidName("missionType")]
		public MissionType MissionType { get; set; }

		[CandidName("progress")]
		public UnboundedUInt Progress { get; set; }

		[CandidName("reward_amount")]
		public UnboundedUInt RewardAmount { get; set; }

		[CandidName("reward_type")]
		public MissionRewardType RewardType { get; set; }

		[CandidName("start_date")]
		public ulong StartDate { get; set; }

		[CandidName("total")]
		public UnboundedUInt Total { get; set; }

		public MissionsUser(ulong expiration, ulong finishDate, bool finished, UnboundedUInt idMission, MissionType missionType, UnboundedUInt progress, UnboundedUInt rewardAmount, MissionRewardType rewardType, ulong startDate, UnboundedUInt total)
		{
			this.Expiration = expiration;
			this.FinishDate = finishDate;
			this.Finished = finished;
			this.IdMission = idMission;
			this.MissionType = missionType;
			this.Progress = progress;
			this.RewardAmount = rewardAmount;
			this.RewardType = rewardType;
			this.StartDate = startDate;
			this.Total = total;
		}

		public MissionsUser()
		{
		}
	}
}