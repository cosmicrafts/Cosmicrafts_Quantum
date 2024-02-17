using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using CanisterPK.NFTsICHub;
using EdjCase.ICP.Agent.Responses;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Mapping;
using TokenIndex = System.UInt32;
using Tokenidentifier1 = System.String;
using Extension = System.String;
using Accountidentifier1 = System.String;

namespace CanisterPK.NFTsICHub
{
	public class NFTsICHubApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public NFTsICHubApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task AcceptCycles()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "acceptCycles", arg);
		}

		public async Task<Models.Result> Allowance(Models.AllowanceRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "allowance", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async Task Approve(Models.ApproveRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "approve", arg);
		}

		public async Task<UnboundedUInt> AvailableCycles()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "availableCycles", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async Task<Models.BalanceResponse> Balance(Models.BalanceRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "balance", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.BalanceResponse>(this.Converter);
		}

		public async Task<Models.Result3> Bearer(Tokenidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "bearer", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result3>(this.Converter);
		}

		public async Task<string> ComputeExtTokenIdentifier(Principal arg0, uint arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "computeExtTokenIdentifier", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task Distribute(Models.User1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "distribute", arg);
		}

		public async Task<NFTsICHubApiClient.ExtensionsReturnArg0> Extensions()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "extensions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<NFTsICHubApiClient.ExtensionsReturnArg0>(this.Converter);
		}

		public async Task<NFTsICHubApiClient.FreeGiftReturnArg0> FreeGift(Accountidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "freeGift", arg);
			return reply.ToObjects<NFTsICHubApiClient.FreeGiftReturnArg0>(this.Converter);
		}

		public async Task<NFTsICHubApiClient.GetAllTokensReturnArg0> GetAllTokens()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllTokens", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<NFTsICHubApiClient.GetAllTokensReturnArg0>(this.Converter);
		}

		public async Task<NFTsICHubApiClient.GetAllowancesReturnArg0> GetAllowances()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllowances", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<NFTsICHubApiClient.GetAllowancesReturnArg0>(this.Converter);
		}

		public async Task<NFTsICHubApiClient.GetBuyersReturnArg0> GetBuyers()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getBuyers", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<NFTsICHubApiClient.GetBuyersReturnArg0>(this.Converter);
		}

		public async Task<TokenIndex> GetMinted()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMinted", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<TokenIndex>(this.Converter);
		}

		public async Task<Principal> GetMinter()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMinter", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Principal>(this.Converter);
		}

		public async Task<NFTsICHubApiClient.GetRegistryReturnArg0> GetRegistry()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getRegistry", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<NFTsICHubApiClient.GetRegistryReturnArg0>(this.Converter);
		}

		public async Task<TokenIndex> GetSold()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getSold", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<TokenIndex>(this.Converter);
		}

		public async Task<NFTsICHubApiClient.GetTokensReturnArg0> GetTokens()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTokens", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<NFTsICHubApiClient.GetTokensReturnArg0>(this.Converter);
		}

		public async Task<NFTsICHubApiClient.GetUserBalanceReturnArg0> GetUserBalance(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserBalance", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<NFTsICHubApiClient.GetUserBalanceReturnArg0>(this.Converter);
		}

		public async Task<Models.HttpResponse> HttpRequest(Models.HttpRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "http_request", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.HttpResponse>(this.Converter);
		}

		public async Task<Models.Result2> Index(Tokenidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "index", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result2>(this.Converter);
		}

		public async Task<Models.Result1> Metadata(Tokenidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "metadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result1>(this.Converter);
		}

		public async Task<TokenIndex> MintNFT(Models.MintRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "mintNFT", arg);
			return reply.ToObjects<TokenIndex>(this.Converter);
		}

		public async Task SetMinter(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "setMinter", arg);
		}

		public async Task<Models.Result> Supply(Tokenidentifier1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "supply", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async Task<Models.TransferResponse> Transfer(Models.TransferRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "transfer", arg);
			return reply.ToObjects<Models.TransferResponse>(this.Converter);
		}

		public class ExtensionsReturnArg0 : List<Extension>
		{
			public ExtensionsReturnArg0()
			{
			}
		}

		public class FreeGiftReturnArg0 : OptionalValue<TokenIndex>
		{
			public FreeGiftReturnArg0()
			{
			}

			public FreeGiftReturnArg0(TokenIndex value) : base(value)
			{
			}
		}

		public class GetAllTokensReturnArg0 : List<NFTsICHubApiClient.GetAllTokensReturnArg0.GetAllTokensReturnArg0Element>
		{
			public GetAllTokensReturnArg0()
			{
			}

			public class GetAllTokensReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenIndex F0 { get; set; }

				[CandidTag(1U)]
				public Models.Metadata F1 { get; set; }

				public GetAllTokensReturnArg0Element(TokenIndex f0, Models.Metadata f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetAllTokensReturnArg0Element()
				{
				}
			}
		}

		public class GetAllowancesReturnArg0 : List<NFTsICHubApiClient.GetAllowancesReturnArg0.GetAllowancesReturnArg0Element>
		{
			public GetAllowancesReturnArg0()
			{
			}

			public class GetAllowancesReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenIndex F0 { get; set; }

				[CandidTag(1U)]
				public Principal F1 { get; set; }

				public GetAllowancesReturnArg0Element(TokenIndex f0, Principal f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetAllowancesReturnArg0Element()
				{
				}
			}
		}

		public class GetBuyersReturnArg0 : List<NFTsICHubApiClient.GetBuyersReturnArg0.GetBuyersReturnArg0Element>
		{
			public GetBuyersReturnArg0()
			{
			}

			public class GetBuyersReturnArg0Element
			{
				[CandidTag(0U)]
				public Accountidentifier1 F0 { get; set; }

				[CandidTag(1U)]
				public NFTsICHubApiClient.GetBuyersReturnArg0.GetBuyersReturnArg0Element.F1Info F1 { get; set; }

				public GetBuyersReturnArg0Element(Accountidentifier1 f0, NFTsICHubApiClient.GetBuyersReturnArg0.GetBuyersReturnArg0Element.F1Info f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetBuyersReturnArg0Element()
				{
				}

				public class F1Info : List<TokenIndex>
				{
					public F1Info()
					{
					}
				}
			}
		}

		public class GetRegistryReturnArg0 : List<NFTsICHubApiClient.GetRegistryReturnArg0.GetRegistryReturnArg0Element>
		{
			public GetRegistryReturnArg0()
			{
			}

			public class GetRegistryReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenIndex F0 { get; set; }

				[CandidTag(1U)]
				public Accountidentifier1 F1 { get; set; }

				public GetRegistryReturnArg0Element(TokenIndex f0, Accountidentifier1 f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetRegistryReturnArg0Element()
				{
				}
			}
		}

		public class GetTokensReturnArg0 : List<NFTsICHubApiClient.GetTokensReturnArg0.GetTokensReturnArg0Element>
		{
			public GetTokensReturnArg0()
			{
			}

			public class GetTokensReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenIndex F0 { get; set; }

				[CandidTag(1U)]
				public Models.Metadata F1 { get; set; }

				public GetTokensReturnArg0Element(TokenIndex f0, Models.Metadata f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetTokensReturnArg0Element()
				{
				}
			}
		}

		public class GetUserBalanceReturnArg0 : List<NFTsICHubApiClient.GetUserBalanceReturnArg0.GetUserBalanceReturnArg0Element>
		{
			public GetUserBalanceReturnArg0()
			{
			}

			public class GetUserBalanceReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenIndex F0 { get; set; }

				[CandidTag(1U)]
				public Models.Metadata F1 { get; set; }

				public GetUserBalanceReturnArg0Element(TokenIndex f0, Models.Metadata f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetUserBalanceReturnArg0Element()
				{
				}
			}
		}
	}
}