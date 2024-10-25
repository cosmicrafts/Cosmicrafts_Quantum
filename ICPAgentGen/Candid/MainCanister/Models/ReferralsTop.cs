using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Username = System.String;
using PlayerId = EdjCase.ICP.Candid.Models.Principal;

namespace Cosmicrafts.MainCanister.Models
{
	public class ReferralsTop
	{
		[CandidName("avatar")]
		public UnboundedUInt Avatar { get; set; }

		[CandidName("multiplier")]
		public double Multiplier { get; set; }

		[CandidName("playerId")]
		public Principal PlayerId { get; set; }

		[CandidName("referrer")]
		public OptionalValue<Principal> Referrer { get; set; }

		[CandidName("totalReferrals")]
		public UnboundedUInt TotalReferrals { get; set; }

		[CandidName("username")]
		public string Username { get; set; }

		public ReferralsTop(UnboundedUInt avatar, double multiplier, Principal playerId, OptionalValue<Principal> referrer, UnboundedUInt totalReferrals, string username)
		{
			this.Avatar = avatar;
			this.Multiplier = multiplier;
			this.PlayerId = playerId;
			this.Referrer = referrer;
			this.TotalReferrals = totalReferrals;
			this.Username = username;
		}

		public ReferralsTop()
		{
		}
	}
}