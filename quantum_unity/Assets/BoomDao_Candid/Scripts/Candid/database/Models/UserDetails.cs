using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using CanisterPK.database.Models;

namespace CanisterPK.database.Models
{
	public class UserDetails
	{
		[CandidName("friends")]
		public List<FriendDetails> Friends { get; set; }

		[CandidName("user")]
		public UserRecord User { get; set; }

		public UserDetails(List<FriendDetails> friends, UserRecord user)
		{
			this.Friends = friends;
			this.User = user;
		}

		public UserDetails()
		{
		}
	}
}