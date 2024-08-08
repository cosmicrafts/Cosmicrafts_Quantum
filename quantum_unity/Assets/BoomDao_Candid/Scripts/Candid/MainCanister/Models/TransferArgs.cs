using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.MainCanister.Models;
using System.Collections.Generic;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Subaccount1 = System.Collections.Generic.List<System.Byte>;

namespace Cosmicrafts.MainCanister.Models
{
	public class TransferArgs
	{
		[CandidName("created_at_time")]
		public OptionalValue<ulong> CreatedAtTime { get; set; }

		[CandidName("from")]
		public OptionalValue<Account> From { get; set; }

		[CandidName("is_atomic")]
		public OptionalValue<bool> IsAtomic { get; set; }

		[CandidName("memo")]
		public OptionalValue<List<byte>> Memo { get; set; }

		[CandidName("spender_subaccount")]
		public OptionalValue<Subaccount1> SpenderSubaccount { get; set; }

		[CandidName("to")]
		public Account To { get; set; }

		[CandidName("token_ids")]
		public List<TokenId> TokenIds { get; set; }

		public TransferArgs(OptionalValue<ulong> createdAtTime, OptionalValue<Account> from, OptionalValue<bool> isAtomic, OptionalValue<List<byte>> memo, OptionalValue<Subaccount1> spenderSubaccount, Account to, List<TokenId> tokenIds)
		{
			this.CreatedAtTime = createdAtTime;
			this.From = from;
			this.IsAtomic = isAtomic;
			this.Memo = memo;
			this.SpenderSubaccount = spenderSubaccount;
			this.To = to;
			this.TokenIds = tokenIds;
		}

		public TransferArgs()
		{
		}
	}
}