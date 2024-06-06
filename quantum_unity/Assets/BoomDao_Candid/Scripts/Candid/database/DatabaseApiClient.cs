using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using CanisterPK.database;
using EdjCase.ICP.Agent.Responses;
using System.Collections.Generic;
using Username = System.String;
using UserID = EdjCase.ICP.Candid.Models.Principal;
using Description = System.String;
using AvatarID = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.database
{
	public class DatabaseApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public DatabaseApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> AddFriend(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "addFriend", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<DatabaseApiClient.GetFriendsListReturnArg0> GetFriendsList()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getFriendsList", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<DatabaseApiClient.GetFriendsListReturnArg0>(this.Converter);
		}

		public async Task<OptionalValue<Models.UserDetails>> GetUserDetails(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserDetails", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.UserDetails>>(this.Converter);
		}

		public async Task<(bool ReturnArg0, UserID ReturnArg1)> RegisterUser(Username arg0, AvatarID arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "registerUser", arg);
			return reply.ToObjects<bool, UserID>(this.Converter);
		}

		public async Task<OptionalValue<Models.UserRecord>> SearchUserByPrincipal(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "searchUserByPrincipal", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.UserRecord>>(this.Converter);
		}

		public async Task<List<Models.UserRecord>> SearchUserByUsername(Username arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "searchUserByUsername", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.UserRecord>>(this.Converter);
		}

		public async Task<(bool ReturnArg0, UserID ReturnArg1)> UpdateAvatar(AvatarID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateAvatar", arg);
			return reply.ToObjects<bool, UserID>(this.Converter);
		}

		public async Task<(bool ReturnArg0, UserID ReturnArg1)> UpdateDescription(Description arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateDescription", arg);
			return reply.ToObjects<bool, UserID>(this.Converter);
		}

		public async Task<(bool ReturnArg0, UserID ReturnArg1)> UpdateUsername(Username arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateUsername", arg);
			return reply.ToObjects<bool, UserID>(this.Converter);
		}

		public class GetFriendsListReturnArg0 : OptionalValue<DatabaseApiClient.GetFriendsListReturnArg0.GetFriendsListReturnArg0Value>
		{
			public GetFriendsListReturnArg0()
			{
			}

			public GetFriendsListReturnArg0(DatabaseApiClient.GetFriendsListReturnArg0.GetFriendsListReturnArg0Value value) : base(value)
			{
			}

			public class GetFriendsListReturnArg0Value : List<UserID>
			{
				public GetFriendsListReturnArg0Value()
				{
				}
			}
		}
	}
}