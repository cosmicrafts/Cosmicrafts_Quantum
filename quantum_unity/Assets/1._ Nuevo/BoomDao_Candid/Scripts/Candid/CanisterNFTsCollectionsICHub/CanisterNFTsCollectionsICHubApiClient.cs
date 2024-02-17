using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using CanisterPK.CanisterNFTsCollectionsICHub;
using System.Collections.Generic;
using EdjCase.ICP.Agent.Responses;

namespace CanisterPK.CanisterNFTsCollectionsICHub
{
	public class CanisterNFTsCollectionsICHubApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public CanisterNFTsCollectionsICHubApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> AddNFTCollection(Models.NFTMetadata arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "addNFTCollection", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<Dictionary<Principal, Models.NFTMetadata>> GetMyCollections()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMyCollections", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Dictionary<Principal, Models.NFTMetadata>>(this.Converter);
		}

		public async Task<Dictionary<Principal, Models.NFTMetadata>> GetNftsCanisters()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getNftsCanisters", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Dictionary<Principal, Models.NFTMetadata>>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> RemoveCollection(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "removeCollection", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UpdateAvatarNFTCollection(Principal arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateAvatarNFTCollection", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}
	}
}