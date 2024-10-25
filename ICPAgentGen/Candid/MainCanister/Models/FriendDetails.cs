using EdjCase.ICP.Candid.Mapping;
using Username1 = System.String;
using Username = System.String;
using Playerid1 = EdjCase.ICP.Candid.Models.Principal;
using PlayerId = EdjCase.ICP.Candid.Models.Principal;
using AvatarID = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.MainCanister.Models
{
	public class FriendDetails
	{
		[CandidName("avatar")]
		public AvatarID Avatar { get; set; }

		[CandidName("playerId")]
		public Playerid1 PlayerId { get; set; }

		[CandidName("username")]
		public Username1 Username { get; set; }

		public FriendDetails(AvatarID avatar, Playerid1 playerId, Username1 username)
		{
			this.Avatar = avatar;
			this.PlayerId = playerId;
			this.Username = username;
		}

		public FriendDetails()
		{
		}
	}
}