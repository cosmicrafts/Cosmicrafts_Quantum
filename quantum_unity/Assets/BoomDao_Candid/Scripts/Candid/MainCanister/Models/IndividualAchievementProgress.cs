using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.MainCanister.Models;
using EdjCase.ICP.Candid.Models;

namespace Cosmicrafts.MainCanister.Models
{
	public class IndividualAchievementProgress
	{
		[CandidName("completed")]
		public bool Completed { get; set; }

		[CandidName("individualAchievement")]
		public IndividualAchievement IndividualAchievement { get; set; }

		[CandidName("progress")]
		public UnboundedUInt Progress { get; set; }

		public IndividualAchievementProgress(bool completed, IndividualAchievement individualAchievement, UnboundedUInt progress)
		{
			this.Completed = completed;
			this.IndividualAchievement = individualAchievement;
			this.Progress = progress;
		}

		public IndividualAchievementProgress()
		{
		}
	}
}