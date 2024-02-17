using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using CanisterPK.CoreICHubCanister;
using EdjCase.ICP.Agent.Responses;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Mapping;
using Username1 = System.String;
using Userid1 = EdjCase.ICP.Candid.Models.Principal;
using Groupid1 = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.CoreICHubCanister
{
	public class CoreICHubCanisterApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public CoreICHubCanisterApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<bool> AddUserFavApp(Userid1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "addUserFavApp", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> AddUserToGroup(Groupid1 arg0, Userid1 arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "add_user_to_group", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> ApproveUserPendingGroup(Groupid1 arg0, Userid1 arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "approveUserPendingGroup", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> BanUser(Userid1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ban_user", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> ChangeGroupAvatar(Groupid1 arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "changeGroupAvatar", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> ChangeGroupDescription(Groupid1 arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "changeGroupDescription", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> ChangeGroupName(Groupid1 arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "changeGroupName", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> ChangeGroupPrivacy(Groupid1 arg0, bool arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "changeGroupPrivacy", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> ChangeUserDescription(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "changeUserDescription", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> CreateGroup(string arg0, bool arg1, bool arg2, string arg3, string arg4)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter), CandidTypedValue.FromObject(arg4, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "create_group", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1, UnboundedUInt ReturnArg2)> CreatePrivateChat(Userid1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "create_private_chat", arg);
			return reply.ToObjects<bool, string, UnboundedUInt>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> CreateUserProfile(Username1 arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "create_user_profile", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> FriendRequest(Userid1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "friendRequest", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<CoreICHubCanisterApiClient.GetAllGroupsReturnArg0> GetAllGroups()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllGroups", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<CoreICHubCanisterApiClient.GetAllGroupsReturnArg0>(this.Converter);
		}

		public async Task<CoreICHubCanisterApiClient.GetAllUsersReturnArg0> GetAllUsers()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllUsers", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<CoreICHubCanisterApiClient.GetAllUsersReturnArg0>(this.Converter);
		}

		public async Task<OptionalValue<Models.Friends>> GetFriendListData()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getFriendListData", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Friends>>(this.Converter);
		}

		public async Task<UnboundedUInt> GetIsFriend(Userid1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getIsFriend", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async Task<List<Models.UserFavorite>> GetMyFavorites()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMyFavorites", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.UserFavorite>>(this.Converter);
		}

		public async Task<List<Models.UserFriendData>> GetMyFriendRequests()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMyFriendRequests", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.UserFriendData>>(this.Converter);
		}

		public async Task<List<Models.UserFriendData>> GetMyFriends()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMyFriends", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.UserFriendData>>(this.Converter);
		}

		public async Task<(UnboundedUInt ReturnArg0, bool ReturnArg1)> GetPrivateChat(Userid1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getPrivateChat", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt, bool>(this.Converter);
		}

		public async Task<string> GetUserAvatar(Userid1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserAvatar", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task<OptionalValue<Models.UserGroups>> GetUserGroupsAdmin(Userid1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserGroupsAdmin", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.UserGroups>>(this.Converter);
		}

		public async Task<Principal> GetUserID()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getUserID", arg);
			return reply.ToObjects<Principal>(this.Converter);
		}

		public async Task<Username1> GetUsername(Userid1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUsername", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Username1>(this.Converter);
		}

		public async Task<string> GetUsersActivity(Userid1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUsersActivity", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task<CoreICHubCanisterApiClient.GetUsersAvatarReturnArg0> GetUsersAvatar(CoreICHubCanisterApiClient.GetUsersAvatarArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUsersAvatar", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<CoreICHubCanisterApiClient.GetUsersAvatarReturnArg0>(this.Converter);
		}

		public async Task<OptionalValue<Models.UserData>> GetUser(Userid1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get_user", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.UserData>>(this.Converter);
		}

		public async Task<List<Models.GroupData>> GetUserGroups()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get_user_groups", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.GroupData>>(this.Converter);
		}

		public async Task<bool> HasUserRequestedJoin(Groupid1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "hasUserRequestedJoin", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> Initialize()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "initialize", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> IsUsedAdded(Groupid1 arg0, Userid1 arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "is_used_added", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> LogUserActivity(string arg0, bool arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "logUserActivity", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> RejectFriendRequest(Userid1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "rejectFriendRequest", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> RejectUserPendingGroup(Groupid1 arg0, Userid1 arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "rejectUserPendingGroup", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> RemoveUserFromGroup(Userid1 arg0, Groupid1 arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "remove_user_from_group", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<OptionalValue<List<Models.GroupData>>> SearchGroupByName(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "search_group_by_name", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.GroupData>>>(this.Converter);
		}

		public async Task<OptionalValue<List<Models.UserSearchData>>> SearchUserByName(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "search_user_by_name", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.UserSearchData>>>(this.Converter);
		}

		public async Task<bool> SetImageToUser(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "setImageToUser", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public class GetAllGroupsReturnArg0 : List<CoreICHubCanisterApiClient.GetAllGroupsReturnArg0.GetAllGroupsReturnArg0Element>
		{
			public GetAllGroupsReturnArg0()
			{
			}

			public class GetAllGroupsReturnArg0Element
			{
				[CandidTag(0U)]
				public Groupid1 F0 { get; set; }

				[CandidTag(1U)]
				public Models.GroupData F1 { get; set; }

				public GetAllGroupsReturnArg0Element(Groupid1 f0, Models.GroupData f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetAllGroupsReturnArg0Element()
				{
				}
			}
		}

		public class GetAllUsersReturnArg0 : List<CoreICHubCanisterApiClient.GetAllUsersReturnArg0.GetAllUsersReturnArg0Element>
		{
			public GetAllUsersReturnArg0()
			{
			}

			public class GetAllUsersReturnArg0Element
			{
				[CandidTag(0U)]
				public Userid1 F0 { get; set; }

				[CandidTag(1U)]
				public Models.UserData F1 { get; set; }

				public GetAllUsersReturnArg0Element(Userid1 f0, Models.UserData f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetAllUsersReturnArg0Element()
				{
				}
			}
		}

		public class GetUsersAvatarArg0 : List<Userid1>
		{
			public GetUsersAvatarArg0()
			{
			}
		}

		public class GetUsersAvatarReturnArg0 : List<CoreICHubCanisterApiClient.GetUsersAvatarReturnArg0.GetUsersAvatarReturnArg0Element>
		{
			public GetUsersAvatarReturnArg0()
			{
			}

			public class GetUsersAvatarReturnArg0Element
			{
				[CandidTag(0U)]
				public Userid1 F0 { get; set; }

				[CandidTag(1U)]
				public string F1 { get; set; }

				public GetUsersAvatarReturnArg0Element(Userid1 f0, string f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetUsersAvatarReturnArg0Element()
				{
				}
			}
		}
	}
}