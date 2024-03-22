using EdjCase.ICP.Candid.Mapping;
using Boom.BoomToken.Models;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using Subaccount = System.Collections.Generic.List<System.Byte>;
using Timestamp = System.UInt64;
using Tokens = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Boom.BoomToken.Models
{
	public class ApproveArgs
	{
		[CandidName("from_subaccount")]
		public ApproveArgs.FromSubaccountInfo FromSubaccount { get; set; }

		[CandidName("spender")]
		public Account Spender { get; set; }

		[CandidName("amount")]
		public Tokens Amount { get; set; }

		[CandidName("expected_allowance")]
		public ApproveArgs.ExpectedAllowanceInfo ExpectedAllowance { get; set; }

		[CandidName("expires_at")]
		public ApproveArgs.ExpiresAtInfo ExpiresAt { get; set; }

		[CandidName("fee")]
		public ApproveArgs.FeeInfo Fee { get; set; }

		[CandidName("memo")]
		public OptionalValue<List<byte>> Memo { get; set; }

		[CandidName("created_at_time")]
		public ApproveArgs.CreatedAtTimeInfo CreatedAtTime { get; set; }

		public ApproveArgs(ApproveArgs.FromSubaccountInfo fromSubaccount, Account spender, Tokens amount, ApproveArgs.ExpectedAllowanceInfo expectedAllowance, ApproveArgs.ExpiresAtInfo expiresAt, ApproveArgs.FeeInfo fee, OptionalValue<List<byte>> memo, ApproveArgs.CreatedAtTimeInfo createdAtTime)
		{
			this.FromSubaccount = fromSubaccount;
			this.Spender = spender;
			this.Amount = amount;
			this.ExpectedAllowance = expectedAllowance;
			this.ExpiresAt = expiresAt;
			this.Fee = fee;
			this.Memo = memo;
			this.CreatedAtTime = createdAtTime;
		}

		public ApproveArgs()
		{
		}

		public class FromSubaccountInfo : OptionalValue<Subaccount>
		{
			public FromSubaccountInfo()
			{
			}

			public FromSubaccountInfo(Subaccount value) : base(value)
			{
			}
		}

		public class ExpectedAllowanceInfo : OptionalValue<Tokens>
		{
			public ExpectedAllowanceInfo()
			{
			}

			public ExpectedAllowanceInfo(Tokens value) : base(value)
			{
			}
		}

		public class ExpiresAtInfo : OptionalValue<Timestamp>
		{
			public ExpiresAtInfo()
			{
			}

			public ExpiresAtInfo(Timestamp value) : base(value)
			{
			}
		}

		public class FeeInfo : OptionalValue<Tokens>
		{
			public FeeInfo()
			{
			}

			public FeeInfo(Tokens value) : base(value)
			{
			}
		}

		public class CreatedAtTimeInfo : OptionalValue<Timestamp>
		{
			public CreatedAtTimeInfo()
			{
			}

			public CreatedAtTimeInfo(Timestamp value) : base(value)
			{
			}
		}
	}
}