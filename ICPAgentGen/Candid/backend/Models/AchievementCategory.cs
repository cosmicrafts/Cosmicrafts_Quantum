using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Cosmicrafts.backend.Models;
using EdjCase.ICP.Candid.Models;

namespace Cosmicrafts.backend.Models
{
	public class AchievementCategory
	{
		[CandidName("achievements")]
		public List<AchievementLine> Achievements { get; set; }

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

		public AchievementCategory(List<AchievementLine> achievements, bool claimed, bool completed, UnboundedUInt id, string name, UnboundedUInt progress, UnboundedUInt requiredProgress, List<Achievementreward1> reward)
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