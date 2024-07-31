using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using CanisterPK.testnft;
using EdjCase.ICP.Agent.Responses;
using System.Collections.Generic;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.testnft
{
	public class TestnftApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public TestnftApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<Models.Account> GetCollectionOwner()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get_collection_owner", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Account>(this.Converter);
		}

		public async Task<Models.GetTransactionsResult> GetTransactions(Models.GetTransactionsArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "get_transactions", arg);
			return reply.ToObjects<Models.GetTransactionsResult>(this.Converter);
		}

		public async Task<Models.ApprovalReceipt> Icrc7Approve(Models.ApprovalArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "icrc7_approve", arg);
			return reply.ToObjects<Models.ApprovalReceipt>(this.Converter);
		}

		public async Task<Models.BalanceResult> Icrc7BalanceOf(Models.Account arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_balance_of", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.BalanceResult>(this.Converter);
		}

		public async Task<Models.CollectionMetadata> Icrc7CollectionMetadata()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_collection_metadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.CollectionMetadata>(this.Converter);
		}

		public async Task<OptionalValue<string>> Icrc7Description()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_description", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<string>>(this.Converter);
		}

		public async Task<OptionalValue<List<byte>>> Icrc7Image()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_image", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<byte>>>(this.Converter);
		}

		public async Task<Models.MetadataResult> Icrc7Metadata(TokenId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_metadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.MetadataResult>(this.Converter);
		}

		public async Task<string> Icrc7Name()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_name", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task<Models.OwnerResult> Icrc7OwnerOf(TokenId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_owner_of", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.OwnerResult>(this.Converter);
		}

		public async Task<OptionalValue<ushort>> Icrc7Royalties()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_royalties", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<ushort>>(this.Converter);
		}

		public async Task<OptionalValue<Models.Account>> Icrc7RoyaltyRecipient()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_royalty_recipient", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Account>>(this.Converter);
		}

		public async Task<OptionalValue<UnboundedUInt>> Icrc7SupplyCap()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_supply_cap", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<UnboundedUInt>>(this.Converter);
		}

		public async Task<List<Models.SupportedStandard>> Icrc7SupportedStandards()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_supported_standards", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.SupportedStandard>>(this.Converter);
		}

		public async Task<string> Icrc7Symbol()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_symbol", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task<Models.TokensOfResult> Icrc7TokensOf(Models.Account arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_tokens_of", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.TokensOfResult>(this.Converter);
		}

		public async Task<UnboundedUInt> Icrc7TotalSupply()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_total_supply", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async Task<Models.TransferReceipt> Icrc7Transfer(Models.TransferArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "icrc7_transfer", arg);
			return reply.ToObjects<Models.TransferReceipt>(this.Converter);
		}

		public async Task<Models.MintReceipt> Mint(Models.MintArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "mint", arg);
			return reply.ToObjects<Models.MintReceipt>(this.Converter);
		}

		public async Task<Models.MintReceipt> MintDeck(List<Models.MintArgs> arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "mintDeck", arg);
			return reply.ToObjects<Models.MintReceipt>(this.Converter);
		}

		public async Task<Models.UpgradeReceipt> UpgradeNFT(Models.UpgradeArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "upgradeNFT", arg);
			return reply.ToObjects<Models.UpgradeReceipt>(this.Converter);
		}
	}
}