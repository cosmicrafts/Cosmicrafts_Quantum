using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.MainCanister.Models;
using System.Collections.Generic;

namespace Cosmicrafts.MainCanister.Models
{
	public class IndividualAchievement
	{
		[CandidName("achievementId")]
		public UnboundedUInt AchievementId { get; set; }

		[CandidName("achievementType")]
		public AchievementType AchievementType { get; set; }

		[CandidName("completed")]
		public bool Completed { get; set; }

		[CandidName("id")]
		public UnboundedUInt Id { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		[CandidName("progress")]
		public UnboundedUInt Progress { get; set; }

		[CandidName("requiredProgress")]
		public UnboundedUInt RequiredProgress { get; set; }

		[CandidName("reward")]
		public List<AchievementReward> Reward { get; set; }

		public IndividualAchievement(UnboundedUInt achievementId, AchievementType achievementType, bool completed, UnboundedUInt id, string name, UnboundedUInt progress, UnboundedUInt requiredProgress, List<AchievementReward> reward)
		{
			this.AchievementId = achievementId;
			this.AchievementType = achievementType;
			this.Completed = completed;
			this.Id = id;
			this.Name = name;
			this.Progress = progress;
			this.RequiredProgress = requiredProgress;
			this.Reward = reward;
		}

		public IndividualAchievement()
		{
		}
	}
}