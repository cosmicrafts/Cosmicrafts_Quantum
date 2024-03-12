using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using Candid.IcrcLedger;
using EdjCase.ICP.Agent.Responses;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Mapping;
using AccountIdentifier = System.Collections.Generic.List<System.Byte>;
using BlockIndex = System.UInt64;
using Icrc1Tokens = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Candid.IcrcLedger
{
	public class IcrcLedgerApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public IcrcLedgerApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<Models.TransferResult> Transfer(Models.TransferArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "transfer", arg);
			return reply.ToObjects<Models.TransferResult>(this.Converter);
		}

		public async Task<Models.Tokens> AccountBalance(Models.AccountBalanceArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "account_balance", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Tokens>(this.Converter);
		}

		public async Task<AccountIdentifier> AccountIdentifier(Models.Account arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "account_identifier", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<AccountIdentifier>(this.Converter);
		}

		public async Task<Models.TransferFee> TransferFee(Models.TransferFeeArg arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "transfer_fee", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.TransferFee>(this.Converter);
		}

		public async Task<Models.QueryBlocksResponse> QueryBlocks(Models.GetBlocksArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "query_blocks", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.QueryBlocksResponse>(this.Converter);
		}

		public async Task<Models.QueryEncodedBlocksResponse> QueryEncodedBlocks(Models.GetBlocksArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "query_encoded_blocks", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.QueryEncodedBlocksResponse>(this.Converter);
		}

		public async Task<IcrcLedgerApiClient.SymbolReturnArg0> Symbol()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "symbol", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<IcrcLedgerApiClient.SymbolReturnArg0>(this.Converter);
		}

		public async Task<IcrcLedgerApiClient.NameReturnArg0> Name()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "name", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<IcrcLedgerApiClient.NameReturnArg0>(this.Converter);
		}

		public async Task<IcrcLedgerApiClient.DecimalsReturnArg0> Decimals()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "decimals", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<IcrcLedgerApiClient.DecimalsReturnArg0>(this.Converter);
		}

		public async Task<Models.Archives> Archives()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "archives", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Archives>(this.Converter);
		}

		public async Task<BlockIndex> SendDfx(Models.SendArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "send_dfx", arg);
			return reply.ToObjects<BlockIndex>(this.Converter);
		}

		public async Task<Models.Tokens> AccountBalanceDfx(Models.AccountBalanceArgsDfx arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "account_balance_dfx", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Tokens>(this.Converter);
		}

		public async Task<string> Icrc1Name()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_name", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task<string> Icrc1Symbol()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_symbol", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task<byte> Icrc1Decimals()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_decimals", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<byte>(this.Converter);
		}

		public async Task<Dictionary<string, Models.Value>> Icrc1Metadata()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_metadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Dictionary<string, Models.Value>>(this.Converter);
		}

		public async Task<Icrc1Tokens> Icrc1TotalSupply()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_total_supply", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Icrc1Tokens>(this.Converter);
		}

		public async Task<Icrc1Tokens> Icrc1Fee()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_fee", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Icrc1Tokens>(this.Converter);
		}

		public async Task<OptionalValue<Models.Account>> Icrc1MintingAccount()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_minting_account", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Account>>(this.Converter);
		}

		public async Task<Icrc1Tokens> Icrc1BalanceOf(Models.Account arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_balance_of", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Icrc1Tokens>(this.Converter);
		}

		public async Task<Models.Icrc1TransferResult> Icrc1Transfer(Models.TransferArg arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "icrc1_transfer", arg);
			return reply.ToObjects<Models.Icrc1TransferResult>(this.Converter);
		}

		public async Task<List<IcrcLedgerApiClient.Icrc1SupportedStandardsReturnArg0Item>> Icrc1SupportedStandards()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_supported_standards", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<IcrcLedgerApiClient.Icrc1SupportedStandardsReturnArg0Item>>(this.Converter);
		}

		public async Task<Models.ApproveResult> Icrc2Approve(Models.ApproveArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "icrc2_approve", arg);
			return reply.ToObjects<Models.ApproveResult>(this.Converter);
		}

		public async Task<Models.Allowance> Icrc2Allowance(Models.AllowanceArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc2_allowance", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Allowance>(this.Converter);
		}

		public async Task<Models.TransferFromResult> Icrc2TransferFrom(Models.TransferFromArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "icrc2_transfer_from", arg);
			return reply.ToObjects<Models.TransferFromResult>(this.Converter);
		}

		public class SymbolReturnArg0
		{
			[CandidName("symbol")]
			public string Symbol { get; set; }

			public SymbolReturnArg0(string symbol)
			{
				this.Symbol = symbol;
			}

			public SymbolReturnArg0()
			{
			}
		}

		public class NameReturnArg0
		{
			[CandidName("name")]
			public string Name { get; set; }

			public NameReturnArg0(string name)
			{
				this.Name = name;
			}

			public NameReturnArg0()
			{
			}
		}

		public class DecimalsReturnArg0
		{
			[CandidName("decimals")]
			public uint Decimals { get; set; }

			public DecimalsReturnArg0(uint decimals)
			{
				this.Decimals = decimals;
			}

			public DecimalsReturnArg0()
			{
			}
		}

		public class Icrc1SupportedStandardsReturnArg0Item
		{
			[CandidName("name")]
			public string Name { get; set; }

			[CandidName("url")]
			public string Url { get; set; }

			public Icrc1SupportedStandardsReturnArg0Item(string name, string url)
			{
				this.Name = name;
				this.Url = url;
			}

			public Icrc1SupportedStandardsReturnArg0Item()
			{
			}
		}
	}
}