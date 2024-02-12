using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using CanisterPK.CanisterLogin;
using EdjCase.ICP.Agent.Responses;
using EdjCase.ICP.Candid.Mapping;
using TokenID = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.CanisterLogin
{
	public class CanisterLoginApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public CanisterLoginApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> CreatePlayer(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createPlayer", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<CanisterLoginApiClient.GetICPBalanceReturnArg0> GetICPBalance()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getICPBalance", arg);
			return reply.ToObjects<CanisterLoginApiClient.GetICPBalanceReturnArg0>(this.Converter);
		}

		public async Task<OptionalValue<Models.Player>> GetMyPlayerData()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMyPlayerData", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Player>>(this.Converter);
		}

		public async Task<OptionalValue<Models.Player>> GetPlayer()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getPlayer", arg);
			return reply.ToObjects<OptionalValue<Models.Player>>(this.Converter);
		}

		public async Task<OptionalValue<Models.Player>> GetPlayerData(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getPlayerData", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Player>>(this.Converter);
		}

		public async Task<double> GetPlayerElo(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getPlayerElo", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<double>(this.Converter);
		}

		public async Task<OptionalValue<Models.PlayerPreferences>> GetPlayerPreferences()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getPlayerPreferences", arg);
			return reply.ToObjects<OptionalValue<Models.PlayerPreferences>>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> SavePlayerChar(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "savePlayerChar", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> SavePlayerLanguage(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "savePlayerLanguage", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> SavePlayerName(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "savePlayerName", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> UpdatePlayerElo(Principal arg0, double arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updatePlayerElo", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UpgradeNFT(TokenID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "upgradeNFT", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public class GetICPBalanceReturnArg0
		{
			[CandidName("e8s")]
			public ulong E8s { get; set; }

			public GetICPBalanceReturnArg0(ulong e8s)
			{
				this.E8s = e8s;
			}

			public GetICPBalanceReturnArg0()
			{
			}
		}
	}
}