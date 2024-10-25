using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using ProxyServer.Cosmicrafts.Models;
using UserID = EdjCase.ICP.Candid.Models.Principal;

namespace ProxyServer.Cosmicrafts.Models
{
	public class UserNetwork
	{
		[CandidName("blockedUsers")]
		public OptionalValue<List<UserID>> BlockedUsers { get; set; }

		[CandidName("connections")]
		public List<SocialConnection> Connections { get; set; }

		[CandidName("followers")]
		public OptionalValue<List<UserID>> Followers { get; set; }

		[CandidName("following")]
		public OptionalValue<List<UserID>> Following { get; set; }

		[CandidName("friendRequests")]
		public OptionalValue<List<UserID>> FriendRequests { get; set; }

		[CandidName("friends")]
		public OptionalValue<List<UserID>> Friends { get; set; }

		[CandidName("posts")]
		public OptionalValue<List<Post>> Posts { get; set; }

		[CandidName("status")]
		public Status Status { get; set; }

		public UserNetwork(OptionalValue<List<UserID>> blockedUsers, List<SocialConnection> connections, OptionalValue<List<UserID>> followers, OptionalValue<List<UserID>> following, OptionalValue<List<UserID>> friendRequests, OptionalValue<List<UserID>> friends, OptionalValue<List<Post>> posts, Status status)
		{
			this.BlockedUsers = blockedUsers;
			this.Connections = connections;
			this.Followers = followers;
			this.Following = following;
			this.FriendRequests = friendRequests;
			this.Friends = friends;
			this.Posts = posts;
			this.Status = status;
		}

		public UserNetwork()
		{
		}
	}
}