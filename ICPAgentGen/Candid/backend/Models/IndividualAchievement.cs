using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.backend.Models;
using System.Collections.Generic;

namespace Cosmicrafts.backend.Models
{
	public class IndividualAchievement
	{
		[CandidName("achievementId")]
		public UnboundedUInt AchievementId { get; set; }

		[CandidName("achievementType")]
		public AchievementType AchievementType { get; set; }

		[CandidName("claimed")]
		public bool Claimed { get; set; }

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
		public List<Achievementreward1> Reward { get; set; }

		public IndividualAchievement(UnboundedUInt achievementId, AchievementType achievementType, bool claimed, bool completed, UnboundedUInt id, string name, UnboundedUInt progress, UnboundedUInt requiredProgress, List<Achievementreward1> reward)
		{
			this.AchievementId = achievementId;
			this.AchievementType = achievementType;
			this.Claimed = claimed;
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