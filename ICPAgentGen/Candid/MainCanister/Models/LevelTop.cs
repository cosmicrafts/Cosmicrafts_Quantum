using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Username = System.String;
using PlayerId = EdjCase.ICP.Candid.Models.Principal;
using Level = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.MainCanister.Models
{
	public class LevelTop
	{
		[CandidName("avatar")]
		public UnboundedUInt Avatar { get; set; }

		[CandidName("level")]
		public UnboundedUInt Level { get; set; }

		[CandidName("playerId")]
		public Principal PlayerId { get; set; }

		[CandidName("username")]
		public string Username { get; set; }

		public LevelTop(UnboundedUInt avatar, UnboundedUInt level, Principal playerId, string username)
		{
			this.Avatar = avatar;
			this.Level = level;
			this.PlayerId = playerId;
			this.Username = username;
		}

		public LevelTop()
		{
		}
	}
}