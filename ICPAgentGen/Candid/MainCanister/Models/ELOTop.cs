using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Username = System.String;
using PlayerId = EdjCase.ICP.Candid.Models.Principal;

namespace Cosmicrafts.MainCanister.Models
{
	public class ELOTop
	{
		[CandidName("avatar")]
		public UnboundedUInt Avatar { get; set; }

		[CandidName("elo")]
		public double Elo { get; set; }

		[CandidName("playerId")]
		public Principal PlayerId { get; set; }

		[CandidName("username")]
		public string Username { get; set; }

		public ELOTop(UnboundedUInt avatar, double elo, Principal playerId, string username)
		{
			this.Avatar = avatar;
			this.Elo = elo;
			this.PlayerId = playerId;
			this.Username = username;
		}

		public ELOTop()
		{
		}
	}
}