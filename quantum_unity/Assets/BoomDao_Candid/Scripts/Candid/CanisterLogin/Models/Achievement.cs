using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using CanisterPK.CanisterLogin.Models;

namespace CanisterPK.CanisterLogin.Models
{
	public class Achievement
	{
		[CandidName("categoryId")]
		public UnboundedUInt CategoryId { get; set; }

		[CandidName("completed")]
		public bool Completed { get; set; }

		[CandidName("id")]
		public UnboundedUInt Id { get; set; }

		[CandidName("individualAchievements")]
		public List<UnboundedUInt> IndividualAchievements { get; set; }

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

		public Achievement(UnboundedUInt categoryId, bool completed, UnboundedUInt id, List<UnboundedUInt> individualAchievements, string name, UnboundedUInt progress, UnboundedUInt requiredProgress, List<AchievementReward> reward, AchievementTier tier)
		{
			this.CategoryId = categoryId;
			this.Completed = completed;
			this.Id = id;
			this.IndividualAchievements = individualAchievements;
			this.Name = name;
			this.Progress = progress;
			this.RequiredProgress = requiredProgress;
			this.Reward = reward;
			this.Tier = tier;
		}

		public Achievement()
		{
		}
	}
}