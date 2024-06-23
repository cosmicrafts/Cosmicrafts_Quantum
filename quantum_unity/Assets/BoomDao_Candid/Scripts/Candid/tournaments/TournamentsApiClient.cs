using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using System.Collections.Generic;
using CanisterPK.tournaments;
using EdjCase.ICP.Agent.Responses;
using EdjCase.ICP.Candid.Mapping;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;

namespace CanisterPK.tournaments
{
	public class TournamentsApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public TournamentsApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<bool> AdminUpdateMatchResult(UnboundedUInt arg0, Principal arg1, string arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "adminUpdateMatchResult", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> CloseRegistration(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "closeRegistration", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<UnboundedUInt> CreateTournament(string arg0, Time arg1, string arg2, Time arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createTournament", arg);
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async Task<bool> DeleteAllTournaments()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "deleteAllTournaments", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> DisputeMatch(UnboundedUInt arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "disputeMatch", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> ForfeitMatch(UnboundedUInt arg0, Principal arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "forfeitMatch", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<List<Models.Tournament>> GetActiveTournaments()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getActiveTournaments", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Tournament>>(this.Converter);
		}

		public async Task<List<Models.Tournament>> GetAllTournaments()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllTournaments", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Tournament>>(this.Converter);
		}

		public async Task<List<Models.Tournament>> GetInactiveTournaments()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getInactiveTournaments", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Tournament>>(this.Converter);
		}

		public async Task<List<TournamentsApiClient.GetRegisteredUsersReturnArg0Item>> GetRegisteredUsers(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getRegisteredUsers", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<TournamentsApiClient.GetRegisteredUsersReturnArg0Item>>(this.Converter);
		}

		public async Task<TournamentsApiClient.GetTournamentBracketReturnArg0> GetTournamentBracket(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTournamentBracket", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<TournamentsApiClient.GetTournamentBracketReturnArg0>(this.Converter);
		}

		public async Task<bool> JoinTournament(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "joinTournament", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> ManageDispute(UnboundedUInt arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "manageDispute", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> ResolveDispute(UnboundedUInt arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "resolveDispute", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> SubmitFeedback(UnboundedUInt arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "submitFeedback", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> SubmitMatchResult(UnboundedUInt arg0, Principal arg1, string arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "submitMatchResult", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> UpdateBracket(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateBracket", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public class GetRegisteredUsersReturnArg0Item
		{
			[CandidName("avatarId")]
			public UnboundedUInt AvatarId { get; set; }

			[CandidName("elo")]
			public UnboundedUInt Elo { get; set; }

			[CandidName("principal")]
			public Principal Principal { get; set; }

			[CandidName("username")]
			public string Username { get; set; }

			public GetRegisteredUsersReturnArg0Item(UnboundedUInt avatarId, UnboundedUInt elo, Principal principal, string username)
			{
				this.AvatarId = avatarId;
				this.Elo = elo;
				this.Principal = principal;
				this.Username = username;
			}

			public GetRegisteredUsersReturnArg0Item()
			{
			}
		}

		public class GetTournamentBracketReturnArg0
		{
			[CandidName("matches")]
			public List<Models.Match> Matches { get; set; }

			public GetTournamentBracketReturnArg0(List<Models.Match> matches)
			{
				this.Matches = matches;
			}

			public GetTournamentBracketReturnArg0()
			{
			}
		}
	}
}