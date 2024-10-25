using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Username = System.String;
using PlayerId = EdjCase.ICP.Candid.Models.Principal;

namespace Cosmicrafts.MainCanister.Models
{
	public class AchievementsTop
	{
		[CandidName("avatar")]
		public UnboundedUInt Avatar { get; set; }

		[CandidName("playerId")]
		public Principal PlayerId { get; set; }

		[CandidName("totalAchievements")]
		public UnboundedUInt TotalAchievements { get; set; }

		[CandidName("username")]
		public string Username { get; set; }

		public AchievementsTop(UnboundedUInt avatar, Principal playerId, UnboundedUInt totalAchievements, string username)
		{
			this.Avatar = avatar;
			this.PlayerId = playerId;
			this.TotalAchievements = totalAchievements;
			this.Username = username;
		}

		public AchievementsTop()
		{
		}
	}
}