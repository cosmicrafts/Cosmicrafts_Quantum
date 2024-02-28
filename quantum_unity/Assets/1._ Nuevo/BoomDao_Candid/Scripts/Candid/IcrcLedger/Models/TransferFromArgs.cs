using EdjCase.ICP.Candid.Mapping;
using Candid.IcrcLedger.Models;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using Memo = System.UInt64;
using Icrc1Timestamp = System.UInt64;
using Icrc1Tokens = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Candid.IcrcLedger.Models
{
	public class TransferFromArgs
	{
		[CandidName("spender_subaccount")]
		public TransferFromArgs.SpenderSubaccountInfo SpenderSubaccount { get; set; }

		[CandidName("from")]
		public Account From { get; set; }

		[CandidName("to")]
		public Account To { get; set; }

		[CandidName("amount")]
		public Icrc1Tokens Amount { get; set; }

		[CandidName("fee")]
		public TransferFromArgs.FeeInfo Fee { get; set; }

		[CandidName("memo")]
		public OptionalValue<List<byte>> Memo { get; set; }

		[CandidName("created_at_time")]
		public TransferFromArgs.CreatedAtTimeInfo CreatedAtTime { get; set; }

		public TransferFromArgs(TransferFromArgs.SpenderSubaccountInfo spenderSubaccount, Account from, Account to, Icrc1Tokens amount, TransferFromArgs.FeeInfo fee, OptionalValue<List<byte>> memo, TransferFromArgs.CreatedAtTimeInfo createdAtTime)
		{
			this.SpenderSubaccount = spenderSubaccount;
			this.From = from;
			this.To = to;
			this.Amount = amount;
			this.Fee = fee;
			this.Memo = memo;
			this.CreatedAtTime = createdAtTime;
		}

		public TransferFromArgs()
		{
		}

		public class SpenderSubaccountInfo : OptionalValue<SubAccount>
		{
			public SpenderSubaccountInfo()
			{
			}

			public SpenderSubaccountInfo(SubAccount value) : base(value)
			{
			}
		}

		public class FeeInfo : OptionalValue<Icrc1Tokens>
		{
			public FeeInfo()
			{
			}

			public FeeInfo(Icrc1Tokens value) : base(value)
			{
			}
		}

		public class CreatedAtTimeInfo : OptionalValue<Icrc1Timestamp>
		{
			public CreatedAtTimeInfo()
			{
			}

			public CreatedAtTimeInfo(Icrc1Timestamp value) : base(value)
			{
			}
		}
	}
}