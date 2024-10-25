using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using ProxyServer.Cosmicrafts;
using System.Collections.Generic;
using EdjCase.ICP.Agent.Responses;
using VerificationBadgeBasicInfo = System.Boolean;
using UserNameBasicInfo = System.String;
using UserID = EdjCase.ICP.Candid.Models.Principal;
using TitleBasicInfo = System.String;
using LevelBasicInfo = EdjCase.ICP.Candid.Models.UnboundedUInt;
using DescriptionBasicInfo = System.String;
using CountryBasicInfo = System.String;
using AvatarIDBasicInfo = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace ProxyServer.Cosmicrafts
{
	public class CosmicraftsApiClient
	{
		public IAgent Agent { get; }
		public Principal CanisterId { get; }
		public CandidConverter? Converter { get; }

		public CosmicraftsApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> AcceptFriendReq(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "acceptFriendReq", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> AddFriend(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "addFriend", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> BlockUser(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "blockUser", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, UnboundedInt ReturnArg1, string ReturnArg2)> CreateComment(UnboundedUInt arg0, UserID arg1, UserID arg2, string arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createComment", arg);
			return reply.ToObjects<bool, UnboundedInt, string>(this.Converter);
		}

		public async Task<bool> CreateNotification(Models.Notification arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createNotification", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<UnboundedInt> CreatePost(OptionalValue<List<UnboundedUInt>> arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createPost", arg);
			return reply.ToObjects<UnboundedInt>(this.Converter);
		}

		public async Task<bool> DeleteFriend(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "deleteFriend", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> DeleteFriendReq(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "deleteFriendReq", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> DeletePost(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "deletePost", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> FollowUser(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "followUser", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<CosmicraftsApiClient.GetAllBlockedUsersReturnArg0> GetAllBlockedUsers()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllBlockedUsers", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<CosmicraftsApiClient.GetAllBlockedUsersReturnArg0>(this.Converter);
		}

		public async Task<OptionalValue<List<Models.Notification>>> GetAllNotificationsByID(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllNotificationsByID", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.Notification>>>(this.Converter);
		}

		public async Task<List<Models.UserBasicInfo>> GetAllUsersBasicInfo()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllUsersBasicInfo", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.UserBasicInfo>>(this.Converter);
		}

		public async Task<OptionalValue<List<Models.UserBasicInfo>>> GetBlockedUsersInfo(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getBlockedUsersInfo", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.UserBasicInfo>>>(this.Converter);
		}

		public async Task<OptionalValue<List<Models.UserBasicInfo>>> GetFollowers(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getFollowers", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.UserBasicInfo>>>(this.Converter);
		}

		public async Task<OptionalValue<List<Models.UserBasicInfo>>> GetFollowing(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getFollowing", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.UserBasicInfo>>>(this.Converter);
		}

		public async Task<OptionalValue<List<Models.Notification>>> GetFriendAddedNotifications(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getFriendAddedNotifications", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.Notification>>>(this.Converter);
		}

		public async Task<OptionalValue<List<Models.Notification>>> GetFriendAddedNotificationsByID(Principal arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getFriendAddedNotificationsByID", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.Notification>>>(this.Converter);
		}

		public async Task<OptionalValue<List<Models.Notification>>> GetFriendRequestNotifications(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getFriendRequestNotifications", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.Notification>>>(this.Converter);
		}

		public async Task<OptionalValue<List<Models.Notification>>> GetFriendRequestNotificationsByID(Principal arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getFriendRequestNotificationsByID", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.Notification>>>(this.Converter);
		}

		public async Task<OptionalValue<List<Models.UserBasicInfo>>> GetFriendRequests()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getFriendRequests", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.UserBasicInfo>>>(this.Converter);
		}

		public async Task<OptionalValue<List<Models.UserBasicInfo>>> GetFriends(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getFriends", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.UserBasicInfo>>>(this.Converter);
		}

		public async Task<OptionalValue<List<Models.Notification>>> GetMessageNotifications(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMessageNotifications", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.Notification>>>(this.Converter);
		}

		public async Task<OptionalValue<List<Models.Notification>>> GetNotifications(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getNotifications", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.Notification>>>(this.Converter);
		}

		public async Task<OptionalValue<Models.Post>> GetPost(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getPost", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Post>>(this.Converter);
		}

		public async Task<OptionalValue<Models.Settings>> GetSettings()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getSettings", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Settings>>(this.Converter);
		}

		public async Task<OptionalValue<Models.Status>> GetStatus()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getStatus", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Status>>(this.Converter);
		}

		public async Task<OptionalValue<Models.UserBasicInfo>> GetUserBasicInfo()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserBasicInfo", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.UserBasicInfo>>(this.Converter);
		}

		public async Task<OptionalValue<Models.UserBasicInfo>> GetUserBasicInfoByID(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserBasicInfoByID", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.UserBasicInfo>>(this.Converter);
		}

		public async Task<OptionalValue<Models.UserNetwork>> GetUserNetwork()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserNetwork", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.UserNetwork>>(this.Converter);
		}

		public async Task<OptionalValue<Models.UserNetwork>> GetUserNetworkByID(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getUserNetworkByID", arg);
			return reply.ToObjects<OptionalValue<Models.UserNetwork>>(this.Converter);
		}

		public async Task<OptionalValue<Models.UserProfile>> GetUserProfileByCaller()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserProfileByCaller", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.UserProfile>>(this.Converter);
		}

		public async Task<OptionalValue<Models.UserProfile>> GetUserProfileByID(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserProfileByID", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.UserProfile>>(this.Converter);
		}

		public async Task<bool> InitSettings()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "initSettings", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> IsFriend(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "isFriend", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> SendFriendRequest(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "sendFriendRequest", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> SetServerSettings(Models.Region arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "setServerSettings", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> SetStatus(Models.Status arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "setStatus", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> Signup(string arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "signup", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> UnblockUser(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "unblockUser", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> UnfollowUser(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "unfollowUser", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> UpdateAudioSettings(OptionalValue<UnboundedUInt> arg0, OptionalValue<UnboundedUInt> arg1, OptionalValue<UnboundedUInt> arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateAudioSettings", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> UpdateLanguageSettings(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateLanguageSettings", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> UpdatePingSettings(bool arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updatePingSettings", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> UpdatePost(UnboundedUInt arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updatePost", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UpdatePrivacy(string arg0, bool arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updatePrivacy", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UpdateUserBasicInfo(CosmicraftsApiClient.UpdateUserBasicInfoArg0 arg0, CosmicraftsApiClient.UpdateUserBasicInfoArg1 arg1, CosmicraftsApiClient.UpdateUserBasicInfoArg2 arg2, CosmicraftsApiClient.UpdateUserBasicInfoArg3 arg3, CosmicraftsApiClient.UpdateUserBasicInfoArg4 arg4, CosmicraftsApiClient.UpdateUserBasicInfoArg5 arg5, CosmicraftsApiClient.UpdateUserBasicInfoArg6 arg6)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter), CandidTypedValue.FromObject(arg4, this.Converter), CandidTypedValue.FromObject(arg5, this.Converter), CandidTypedValue.FromObject(arg6, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateUserBasicInfo", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> UserExists()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "userExists", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
		}

		public class GetAllBlockedUsersReturnArg0 : OptionalValue<CosmicraftsApiClient.GetAllBlockedUsersReturnArg0.GetAllBlockedUsersReturnArg0Value>
		{
			public GetAllBlockedUsersReturnArg0()
			{
			}

			public GetAllBlockedUsersReturnArg0(CosmicraftsApiClient.GetAllBlockedUsersReturnArg0.GetAllBlockedUsersReturnArg0Value value) : base(value)
			{
			}

			public class GetAllBlockedUsersReturnArg0Value : List<UserID>
			{
				public GetAllBlockedUsersReturnArg0Value()
				{
				}
			}
		}

		public class UpdateUserBasicInfoArg0 : OptionalValue<UserNameBasicInfo>
		{
			public UpdateUserBasicInfoArg0()
			{
			}

			public UpdateUserBasicInfoArg0(UserNameBasicInfo value) : base(value)
			{
			}
		}

		public class UpdateUserBasicInfoArg1 : OptionalValue<AvatarIDBasicInfo>
		{
			public UpdateUserBasicInfoArg1()
			{
			}

			public UpdateUserBasicInfoArg1(AvatarIDBasicInfo value) : base(value)
			{
			}
		}

		public class UpdateUserBasicInfoArg2 : OptionalValue<LevelBasicInfo>
		{
			public UpdateUserBasicInfoArg2()
			{
			}

			public UpdateUserBasicInfoArg2(LevelBasicInfo value) : base(value)
			{
			}
		}

		public class UpdateUserBasicInfoArg3 : OptionalValue<VerificationBadgeBasicInfo>
		{
			public UpdateUserBasicInfoArg3()
			{
			}

			public UpdateUserBasicInfoArg3(VerificationBadgeBasicInfo value) : base(value)
			{
			}
		}

		public class UpdateUserBasicInfoArg4 : OptionalValue<TitleBasicInfo>
		{
			public UpdateUserBasicInfoArg4()
			{
			}

			public UpdateUserBasicInfoArg4(TitleBasicInfo value) : base(value)
			{
			}
		}

		public class UpdateUserBasicInfoArg5 : OptionalValue<DescriptionBasicInfo>
		{
			public UpdateUserBasicInfoArg5()
			{
			}

			public UpdateUserBasicInfoArg5(DescriptionBasicInfo value) : base(value)
			{
			}
		}

		public class UpdateUserBasicInfoArg6 : OptionalValue<CountryBasicInfo>
		{
			public UpdateUserBasicInfoArg6()
			{
			}

			public UpdateUserBasicInfoArg6(CountryBasicInfo value) : base(value)
			{
			}
		}
	}
}