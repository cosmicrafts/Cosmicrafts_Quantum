using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using CanisterPK.flux;
using EdjCase.ICP.Agent.Responses;
using System.Collections.Generic;
using Txindex1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using MetaDatum = System.ValueTuple<System.String, CanisterPK.flux.Models.Value>;
using Balance1 = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.flux
{
	public class FluxApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public FluxApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<Models.TransferResult> Burn(Models.BurnArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "burn", arg);
			return reply.ToObjects<Models.TransferResult>(this.Converter);
		}

		public async Task DepositCycles()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "deposit_cycles", arg);
		}

		public async Task<OptionalValue<Models.Transaction1>> GetTransaction(Txindex1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "get_transaction", arg);
			return reply.ToObjects<OptionalValue<Models.Transaction1>>(this.Converter);
		}

		public async Task<Models.GetTransactionsResponse> GetTransactions(Models.GetTransactionsRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get_transactions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.GetTransactionsResponse>(this.Converter);
		}

		public async Task<Balance1> Icrc1BalanceOf(Models.Account1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_balance_of", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Balance1>(this.Converter);
		}

		public async Task<byte> Icrc1Decimals()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_decimals", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<byte>(this.Converter);
		}

		public async Task<Balance1> Icrc1Fee()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_fee", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Balance1>(this.Converter);
		}

		public async Task<FluxApiClient.Icrc1MetadataReturnArg0> Icrc1Metadata()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_metadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<FluxApiClient.Icrc1MetadataReturnArg0>(this.Converter);
		}

		public async Task<OptionalValue<Models.Account1>> Icrc1MintingAccount()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_minting_account", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Account1>>(this.Converter);
		}

		public async Task<string> Icrc1Name()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_name", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task<Models.TransferResult> Icrc1PayForTransaction(Models.TransferArgs arg0, Principal arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "icrc1_pay_for_transaction", arg);
			return reply.ToObjects<Models.TransferResult>(this.Converter);
		}

		public async Task<List<Models.SupportedStandard>> Icrc1SupportedStandards()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_supported_standards", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.SupportedStandard>>(this.Converter);
		}

		public async Task<string> Icrc1Symbol()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_symbol", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task<Balance1> Icrc1TotalSupply()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_total_supply", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Balance1>(this.Converter);
		}

		public async Task<Models.TransferResult> Icrc1Transfer(Models.TransferArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "icrc1_transfer", arg);
			return reply.ToObjects<Models.TransferResult>(this.Converter);
		}

		public async Task<Models.TransferResult> Mint(Models.Mint arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "mint", arg);
			return reply.ToObjects<Models.TransferResult>(this.Converter);
		}

		public class Icrc1MetadataReturnArg0 : List<MetaDatum>
		{
			public Icrc1MetadataReturnArg0()
			{
			}
		}
	}
}