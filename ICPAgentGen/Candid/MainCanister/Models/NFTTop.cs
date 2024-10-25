using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Username = System.String;
using PlayerId = EdjCase.ICP.Candid.Models.Principal;
using Level = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.MainCanister.Models
{
	public class NFTTop
	{
		[CandidName("avatar")]
		public UnboundedUInt Avatar { get; set; }

		[CandidName("level")]
		public UnboundedUInt Level { get; set; }

		[CandidName("nftCount")]
		public UnboundedUInt NftCount { get; set; }

		[CandidName("playerId")]
		public Principal PlayerId { get; set; }

		[CandidName("username")]
		public string Username { get; set; }

		public NFTTop(UnboundedUInt avatar, UnboundedUInt level, UnboundedUInt nftCount, Principal playerId, string username)
		{
			this.Avatar = avatar;
			this.Level = level;
			this.NftCount = nftCount;
			this.PlayerId = playerId;
			this.Username = username;
		}

		public NFTTop()
		{
		}
	}
}