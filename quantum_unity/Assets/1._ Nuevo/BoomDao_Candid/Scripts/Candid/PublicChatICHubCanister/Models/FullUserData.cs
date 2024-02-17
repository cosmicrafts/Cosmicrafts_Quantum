using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Username = System.String;
using Userid1 = EdjCase.ICP.Candid.Models.Principal;
using UserID = EdjCase.ICP.Candid.Models.Principal;

namespace CanisterPK.PublicChatICHubCanister.Models
{
	public class FullUserData
	{
		[CandidName("avatar")]
		public string Avatar { get; set; }

		[CandidName("banned")]
		public bool Banned { get; set; }

		[CandidName("role")]
		public UnboundedUInt Role { get; set; }

		[CandidName("userID")]
		public Userid1 UserID { get; set; }

		[CandidName("userSince")]
		public ulong UserSince { get; set; }

		[CandidName("username")]
		public Username Username { get; set; }

		public FullUserData(string avatar, bool banned, UnboundedUInt role, Userid1 userID, ulong userSince, Username username)
		{
			this.Avatar = avatar;
			this.Banned = banned;
			this.Role = role;
			this.UserID = userID;
			this.UserSince = userSince;
			this.Username = username;
		}

		public FullUserData()
		{
		}
	}
}