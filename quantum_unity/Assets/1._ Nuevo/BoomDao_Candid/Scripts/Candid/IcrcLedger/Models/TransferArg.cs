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
	public class TransferArg
	{
		[CandidName("from_subaccount")]
		public TransferArg.FromSubaccountInfo FromSubaccount { get; set; }

		[CandidName("to")]
		public Account To { get; set; }

		[CandidName("amount")]
		public Icrc1Tokens Amount { get; set; }

		[CandidName("fee")]
		public TransferArg.FeeInfo Fee { get; set; }

		[CandidName("memo")]
		public OptionalValue<List<byte>> Memo { get; set; }

		[CandidName("created_at_time")]
		public TransferArg.CreatedAtTimeInfo CreatedAtTime { get; set; }

		public TransferArg(TransferArg.FromSubaccountInfo fromSubaccount, Account to, Icrc1Tokens amount, TransferArg.FeeInfo fee, OptionalValue<List<byte>> memo, TransferArg.CreatedAtTimeInfo createdAtTime)
		{
			this.FromSubaccount = fromSubaccount;
			this.To = to;
			this.Amount = amount;
			this.Fee = fee;
			this.Memo = memo;
			this.CreatedAtTime = createdAtTime;
		}

		public TransferArg()
		{
		}

		public class FromSubaccountInfo : OptionalValue<SubAccount>
		{
			public FromSubaccountInfo()
			{
			}

			public FromSubaccountInfo(SubAccount value) : base(value)
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