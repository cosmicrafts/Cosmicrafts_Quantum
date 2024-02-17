using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Username = System.String;
using UserID = EdjCase.ICP.Candid.Models.Principal;

namespace CanisterPK.CoreICHubCanister.Models
{
	public class UserSearchData
	{
		[CandidName("avatar")]
		public string Avatar { get; set; }

		[CandidName("commonFriends")]
		public UnboundedUInt CommonFriends { get; set; }

		[CandidName("commonGroups")]
		public UnboundedUInt CommonGroups { get; set; }

		[CandidName("status")]
		public string Status { get; set; }

		[CandidName("userID")]
		public UserID UserID { get; set; }

		[CandidName("username")]
		public Username Username { get; set; }

		public UserSearchData(string avatar, UnboundedUInt commonFriends, UnboundedUInt commonGroups, string status, UserID userID, Username username)
		{
			this.Avatar = avatar;
			this.CommonFriends = commonFriends;
			this.CommonGroups = commonGroups;
			this.Status = status;
			this.UserID = userID;
			this.Username = username;
		}

		public UserSearchData()
		{
		}
	}
}