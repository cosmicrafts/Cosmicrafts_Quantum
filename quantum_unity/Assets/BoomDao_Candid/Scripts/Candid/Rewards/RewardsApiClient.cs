using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using System.Collections.Generic;
using CanisterPK.Rewards;
using EdjCase.ICP.Agent.Responses;
using PlayerID = EdjCase.ICP.Candid.Models.Principal;

namespace CanisterPK.Rewards
{
	public class RewardsApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public RewardsApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> AddProgressToRewards(Principal arg0, List<Models.RewardProgress> arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "addProgressToRewards", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1, UnboundedUInt ReturnArg2)> AddReward(Models.Reward arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "addReward", arg);
			return reply.ToObjects<bool, string, UnboundedUInt>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> ClaimedReward(Principal arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "claimedReward", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> CreateReward(string arg0, Models.RewardType arg1, Models.PrizeType arg2, UnboundedUInt arg3, double arg4, ulong arg5)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter), CandidTypedValue.FromObject(arg4, this.Converter), CandidTypedValue.FromObject(arg5, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createReward", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(UnboundedUInt ReturnArg0, List<Models.Reward> ReturnArg1)> GetAllActiveRewards()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllActiveRewards", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt, List<Models.Reward>>(this.Converter);
		}

		public async Task<Dictionary<Principal, List<Models.RewardsUser>>> GetAllUsersRewards()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllUsersRewards", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Dictionary<Principal, List<Models.RewardsUser>>>(this.Converter);
		}

		public async Task<OptionalValue<Models.Reward>> GetReward(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getReward", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Reward>>(this.Converter);
		}

		public async Task<OptionalValue<Models.RewardsUser>> GetUserReward(PlayerID arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserReward", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.RewardsUser>>(this.Converter);
		}

		public async Task<List<Models.RewardsUser>> GetUserRewards(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getUserRewards", arg);
			return reply.ToObjects<List<Models.RewardsUser>>(this.Converter);
		}
	}
}