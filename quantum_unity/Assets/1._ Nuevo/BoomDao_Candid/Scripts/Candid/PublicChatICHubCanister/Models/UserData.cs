using EdjCase.ICP.Candid.Mapping;
using Username = System.String;
using Userid1 = EdjCase.ICP.Candid.Models.Principal;
using UserID = EdjCase.ICP.Candid.Models.Principal;

namespace CanisterPK.PublicChatICHubCanister.Models
{
	public class UserData
	{
		[CandidName("avatar")]
		public string Avatar { get; set; }

		[CandidName("banned")]
		public bool Banned { get; set; }

		[CandidName("description")]
		public string Description { get; set; }

		[CandidName("userID")]
		public Userid1 UserID { get; set; }

		[CandidName("userSince")]
		public ulong UserSince { get; set; }

		[CandidName("username")]
		public Username Username { get; set; }

		public UserData(string avatar, bool banned, string description, Userid1 userID, ulong userSince, Username username)
		{
			this.Avatar = avatar;
			this.Banned = banned;
			this.Description = description;
			this.UserID = userID;
			this.UserSince = userSince;
			this.Username = username;
		}

		public UserData()
		{
		}
	}
}