using EdjCase.ICP.Candid.Mapping;
using Username = System.String;
using UserID = EdjCase.ICP.Candid.Models.Principal;
using AvatarID = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.database.Models
{
	public class FriendDetails
	{
		[CandidName("avatar")]
		public AvatarID Avatar { get; set; }

		[CandidName("userId")]
		public UserID UserId { get; set; }

		[CandidName("username")]
		public Username Username { get; set; }

		public FriendDetails(AvatarID avatar, UserID userId, Username username)
		{
			this.Avatar = avatar;
			this.UserId = userId;
			this.Username = username;
		}

		public FriendDetails()
		{
		}
	}
}