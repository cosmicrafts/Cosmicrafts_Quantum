using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using EdjCase.ICP.Agent.Responses;
using CanisterPK.PublicChatICHubCanister;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Mapping;
using UserID = EdjCase.ICP.Candid.Models.Principal;
using MessageID = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.PublicChatICHubCanister
{
	public class PublicChatICHubCanisterApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public PublicChatICHubCanisterApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<bool> AddTextMessage(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "add_text_message", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> ApproveUserPending(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "approveUserPending", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> ExitChat(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "exit_chat", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<Principal> GetCaller()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getCaller", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Principal>(this.Converter);
		}

		public async Task<Models.UserRoles> GetUserRole()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserRole", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.UserRoles>(this.Converter);
		}

		public async Task<List<Models.RequestJoinData>> GetUsersPending()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUsersPending", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.RequestJoinData>>(this.Converter);
		}

		public async Task<List<Models.FullUserData>> GetGroupUsers()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get_group_users", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.FullUserData>>(this.Converter);
		}

		public async Task<PublicChatICHubCanisterApiClient.GetMessagesReturnArg0> GetMessages()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get_messages", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<PublicChatICHubCanisterApiClient.GetMessagesReturnArg0>(this.Converter);
		}

		public async Task<PublicChatICHubCanisterApiClient.GetMessagesPaginatedReturnArg0> GetMessagesPaginated(UnboundedUInt arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get_messages_paginated", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<PublicChatICHubCanisterApiClient.GetMessagesPaginatedReturnArg0>(this.Converter);
		}

		public async Task<MessageID> GetTotalMessages()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get_total_messages", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<MessageID>(this.Converter);
		}

		public async Task<bool> HasUserRequestedJoin(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "hasUserRequestedJoin", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> IsUserAdded(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "is_user_added", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> JoinChat(UserID arg0, Models.UserData arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "join_chat", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> RejectUserPending(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "rejectUserPending", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> TransferOwner(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "transferOwner", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UserRequestJoin(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "user_request_join", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public class GetMessagesReturnArg0 : List<PublicChatICHubCanisterApiClient.GetMessagesReturnArg0.GetMessagesReturnArg0Element>
		{
			public GetMessagesReturnArg0()
			{
			}

			public class GetMessagesReturnArg0Element
			{
				[CandidTag(0U)]
				public MessageID F0 { get; set; }

				[CandidTag(1U)]
				public Models.MessageData F1 { get; set; }

				public GetMessagesReturnArg0Element(MessageID f0, Models.MessageData f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetMessagesReturnArg0Element()
				{
				}
			}
		}

		public class GetMessagesPaginatedReturnArg0 : List<PublicChatICHubCanisterApiClient.GetMessagesPaginatedReturnArg0.GetMessagesPaginatedReturnArg0Element>
		{
			public GetMessagesPaginatedReturnArg0()
			{
			}

			public class GetMessagesPaginatedReturnArg0Element
			{
				[CandidTag(0U)]
				public MessageID F0 { get; set; }

				[CandidTag(1U)]
				public Models.MessageData F1 { get; set; }

				public GetMessagesPaginatedReturnArg0Element(MessageID f0, Models.MessageData f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetMessagesPaginatedReturnArg0Element()
				{
				}
			}
		}
	}
}