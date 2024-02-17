using EdjCase.ICP.Candid.Mapping;
using Username = System.String;
using UserID = EdjCase.ICP.Candid.Models.Principal;

namespace CanisterPK.CoreICHubCanister.Models
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
		public UserID UserID { get; set; }

		[CandidName("userSince")]
		public ulong UserSince { get; set; }

		[CandidName("username")]
		public Username Username { get; set; }

		public UserData(string avatar, bool banned, string description, UserID userID, ulong userSince, Username username)
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