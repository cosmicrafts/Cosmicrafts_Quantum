using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Username = System.String;
using UserID = EdjCase.ICP.Candid.Models.Principal;
using RegistrationDate = EdjCase.ICP.Candid.Models.UnboundedInt;
using Description = System.String;
using AvatarID = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.database.Models
{
	public class UserRecord
	{
		[CandidName("avatar")]
		public AvatarID Avatar { get; set; }

		[CandidName("description")]
		public Description Description { get; set; }

		[CandidName("friends")]
		public List<UserID> Friends { get; set; }

		[CandidName("registrationDate")]
		public RegistrationDate RegistrationDate { get; set; }

		[CandidName("userId")]
		public UserID UserId { get; set; }

		[CandidName("username")]
		public Username Username { get; set; }

		public UserRecord(AvatarID avatar, Description description, List<UserID> friends, RegistrationDate registrationDate, UserID userId, Username username)
		{
			this.Avatar = avatar;
			this.Description = description;
			this.Friends = friends;
			this.RegistrationDate = registrationDate;
			this.UserId = userId;
			this.Username = username;
		}

		public UserRecord()
		{
		}
	}
}