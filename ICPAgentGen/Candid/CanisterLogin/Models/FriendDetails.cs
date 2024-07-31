using EdjCase.ICP.Candid.Mapping;
using Username1 = System.String;
using Username = System.String;
using Playerid2 = EdjCase.ICP.Candid.Models.Principal;
using PlayerId = EdjCase.ICP.Candid.Models.Principal;
using Avatarid1 = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.CanisterLogin.Models
{
	public class FriendDetails
	{
		[CandidName("avatar")]
		public Avatarid1 Avatar { get; set; }

		[CandidName("playerId")]
		public Playerid2 PlayerId { get; set; }

		[CandidName("username")]
		public Username1 Username { get; set; }

		public FriendDetails(Avatarid1 avatar, Playerid2 playerId, Username1 username)
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