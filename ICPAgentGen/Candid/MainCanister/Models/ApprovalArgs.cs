using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.MainCanister.Models;
using System.Collections.Generic;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Subaccount1 = System.Collections.Generic.List<System.Byte>;

namespace Cosmicrafts.MainCanister.Models
{
	public class ApprovalArgs
	{
		[CandidName("created_at_time")]
		public OptionalValue<ulong> CreatedAtTime { get; set; }

		[CandidName("expires_at")]
		public OptionalValue<ulong> ExpiresAt { get; set; }

		[CandidName("from_subaccount")]
		public ApprovalArgs.FromSubaccountInfo FromSubaccount { get; set; }

		[CandidName("memo")]
		public OptionalValue<List<byte>> Memo { get; set; }

		[CandidName("spender")]
		public Account Spender { get; set; }

		[CandidName("token_ids")]
		public ApprovalArgs.TokenIdsInfo TokenIds { get; set; }

		public ApprovalArgs(OptionalValue<ulong> createdAtTime, OptionalValue<ulong> expiresAt, ApprovalArgs.FromSubaccountInfo fromSubaccount, OptionalValue<List<byte>> memo, Account spender, ApprovalArgs.TokenIdsInfo tokenIds)
		{
			this.CreatedAtTime = createdAtTime;
			this.ExpiresAt = expiresAt;
			this.FromSubaccount = fromSubaccount;
			this.Memo = memo;
			this.Spender = spender;
			this.TokenIds = tokenIds;
		}

		public ApprovalArgs()
		{
		}

		public class FromSubaccountInfo : OptionalValue<Subaccount1>
		{
			public FromSubaccountInfo()
			{
			}

			public FromSubaccountInfo(Subaccount1 value) : base(value)
			{
			}
		}

		public class TokenIdsInfo : OptionalValue<ApprovalArgs.TokenIdsInfo.TokenIdsInfoValue>
		{
			public TokenIdsInfo()
			{
			}

			public TokenIdsInfo(ApprovalArgs.TokenIdsInfo.TokenIdsInfoValue value) : base(value)
			{
			}

			public class TokenIdsInfoValue : List<TokenId>
			{
				public TokenIdsInfoValue()
				{
				}
			}
		}
	}
}