using EdjCase.ICP.Candid.Mapping;
using Username1 = System.String;
using Username = System.String;
using Playerid1 = EdjCase.ICP.Candid.Models.Principal;
using PlayerId = EdjCase.ICP.Candid.Models.Principal;
using Avatarid1 = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.backend.Models
{
	public class FriendDetails
	{
		[CandidName("avatar")]
		public Avatarid1 Avatar { get; set; }

		[CandidName("playerId")]
		public Playerid1 PlayerId { get; set; }

		[CandidName("username")]
		public Username1 Username { get; set; }

		public FriendDetails(Avatarid1 avatar, Playerid1 playerId, Username1 username)
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