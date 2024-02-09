using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using CanisterPK.CanisterMatchMaking;
using EdjCase.ICP.Agent.Responses;

namespace CanisterPK.CanisterMatchMaking
{
	public class CanisterMatchMakingApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public CanisterMatchMakingApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> CancelMatchmaking()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "cancelMatchmaking", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<OptionalValue<Models.MatchData>> GetMatchData(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMatchData", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.MatchData>>(this.Converter);
		}

		public async Task<(Models.SearchStatus ReturnArg0, UnboundedUInt ReturnArg1, string ReturnArg2)> GetMatchSearching(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getMatchSearching", arg);
			return reply.ToObjects<Models.SearchStatus, UnboundedUInt, string>(this.Converter);
		}

		public async Task<(OptionalValue<Models.FullMatchData> ReturnArg0, UnboundedUInt ReturnArg1)> GetMyMatchData()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMyMatchData", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.FullMatchData>, UnboundedUInt>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> IsGameMatched()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "isGameMatched", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> SetGameOver(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "setGameOver", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> SetPlayerActive()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "setPlayerActive", arg);
			return reply.ToObjects<bool>(this.Converter);
		}
	}
}