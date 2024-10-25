using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Cosmicrafts.MainCanister.Models;
using EdjCase.ICP.Candid.Models;

namespace Cosmicrafts.MainCanister.Models
{
	public class AchievementCategory
	{
		[CandidName("achievements")]
		public List<Achievementline1> Achievements { get; set; }

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
		public List<AchievementReward> Reward { get; set; }

		public AchievementCategory(List<Achievementline1> achievements, bool claimed, bool completed, UnboundedUInt id, string name, UnboundedUInt progress, UnboundedUInt requiredProgress, List<AchievementReward> reward)
		{
			this.Achievements = achievements;
			this.Claimed = claimed;
			this.Completed = completed;
			this.Id = id;
			this.Name = name;
			this.Progress = progress;
			this.RequiredProgress = requiredProgress;
			this.Reward = reward;
		}

		public AchievementCategory()
		{
		}
	}
}