using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;
using CanisterPK.CanisterLogin.Models;

namespace CanisterPK.CanisterLogin.Models
{
	public class AchievementCategory
	{
		[CandidName("achievements")]
		public List<UnboundedUInt> Achievements { get; set; }

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

		[CandidName("tier")]
		public AchievementTier Tier { get; set; }

		public AchievementCategory(List<UnboundedUInt> achievements, bool completed, UnboundedUInt id, string name, UnboundedUInt progress, UnboundedUInt requiredProgress, List<AchievementReward> reward, AchievementTier tier)
		{
			this.Achievements = achievements;
			this.Completed = completed;
			this.Id = id;
			this.Name = name;
			this.Progress = progress;
			this.RequiredProgress = requiredProgress;
			this.Reward = reward;
			this.Tier = tier;
		}

		public AchievementCategory()
		{
		}
	}
}