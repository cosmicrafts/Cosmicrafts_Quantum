using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using Cosmicrafts.MainCanister.Models;

namespace Cosmicrafts.MainCanister.Models
{
	public class Achievementline1
	{
		[CandidName("categoryId")]
		public UnboundedUInt CategoryId { get; set; }

		[CandidName("claimed")]
		public bool Claimed { get; set; }

		[CandidName("completed")]
		public bool Completed { get; set; }

		[CandidName("id")]
		public UnboundedUInt Id { get; set; }

		[CandidName("individualAchievements")]
		public List<IndividualAchievement> IndividualAchievements { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		[CandidName("progress")]
		public UnboundedUInt Progress { get; set; }

		[CandidName("requiredProgress")]
		public UnboundedUInt RequiredProgress { get; set; }

		[CandidName("reward")]
		public List<AchievementReward> Reward { get; set; }

		public Achievementline1(UnboundedUInt categoryId, bool claimed, bool completed, UnboundedUInt id, List<IndividualAchievement> individualAchievements, string name, UnboundedUInt progress, UnboundedUInt requiredProgress, List<AchievementReward> reward)
		{
			this.CategoryId = categoryId;
			this.Claimed = claimed;
			this.Completed = completed;
			this.Id = id;
			this.IndividualAchievements = individualAchievements;
			this.Name = name;
			this.Progress = progress;
			this.RequiredProgress = requiredProgress;
			this.Reward = reward;
		}

		public Achievementline1()
		{
		}
	}
}