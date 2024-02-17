using EdjCase.ICP.Candid.Mapping;
using Username = System.String;
using UserID = EdjCase.ICP.Candid.Models.Principal;

namespace CanisterPK.CoreICHubCanister.Models
{
	public class UserFriendData
	{
		[CandidName("avatar")]
		public string Avatar { get; set; }

		[CandidName("status")]
		public string Status { get; set; }

		[CandidName("userID")]
		public UserID UserID { get; set; }

		[CandidName("username")]
		public Username Username { get; set; }

		public UserFriendData(string avatar, string status, UserID userID, Username username)
		{
			this.Avatar = avatar;
			this.Status = status;
			this.UserID = userID;
			this.Username = username;
		}

		public UserFriendData()
		{
		}
	}
}