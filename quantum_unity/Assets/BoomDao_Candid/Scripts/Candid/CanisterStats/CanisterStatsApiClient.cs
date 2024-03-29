using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using CanisterPK.CanisterStats;
using EdjCase.ICP.Agent.Responses;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Mapping;
using GameID = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.CanisterStats
{
	public class CanisterStatsApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public CanisterStatsApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<CanisterStatsApiClient.GetAllOnValidationReturnArg0> GetAllOnValidation()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllOnValidation", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<CanisterStatsApiClient.GetAllOnValidationReturnArg0>(this.Converter);
		}

		public async Task<Models.AverageStats> GetAverageStats()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAverageStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.AverageStats>(this.Converter);
		}

		public async Task<OptionalValue<Models.BasicStats>> GetBasicStats(GameID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getBasicStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.BasicStats>>(this.Converter);
		}

		public async Task<OptionalValue<Models.AverageStats>> GetMyAverageStats()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMyAverageStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.AverageStats>>(this.Converter);
		}

		public async Task<OptionalValue<Models.PlayerGamesStats>> GetMyStats()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMyStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.PlayerGamesStats>>(this.Converter);
		}

		public async Task<Models.OverallStats> GetOverallStats()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getOverallStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.OverallStats>(this.Converter);
		}

		public async Task<OptionalValue<Models.AverageStats>> GetPlayerAverageStats(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getPlayerAverageStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.AverageStats>>(this.Converter);
		}

		public async Task<OptionalValue<Models.PlayerGamesStats>> GetPlayerStats(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getPlayerStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.PlayerGamesStats>>(this.Converter);
		}

		public async Task<bool> SaveFinishedGame(GameID arg0, Models.BasicStats arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "saveFinishedGame", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> SetGameValid(GameID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "setGameValid", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public class GetAllOnValidationReturnArg0 : List<CanisterStatsApiClient.GetAllOnValidationReturnArg0.GetAllOnValidationReturnArg0Element>
		{
			public GetAllOnValidationReturnArg0()
			{
			}

			public class GetAllOnValidationReturnArg0Element
			{
				[CandidTag(0U)]
				public GameID F0 { get; set; }

				[CandidTag(1U)]
				public Models.BasicStats F1 { get; set; }

				public GetAllOnValidationReturnArg0Element(GameID f0, Models.BasicStats f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetAllOnValidationReturnArg0Element()
				{
				}
			}
		}
	}
}