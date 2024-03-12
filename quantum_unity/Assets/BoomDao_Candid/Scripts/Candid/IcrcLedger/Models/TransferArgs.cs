using EdjCase.ICP.Candid.Mapping;
using Candid.IcrcLedger.Models;
using EdjCase.ICP.Candid.Models;
using AccountIdentifier = System.Collections.Generic.List<System.Byte>;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using Memo = System.UInt64;

namespace Candid.IcrcLedger.Models
{
	public class TransferArgs
	{
		[CandidName("memo")]
		public Memo Memo { get; set; }

		[CandidName("amount")]
		public Tokens Amount { get; set; }

		[CandidName("fee")]
		public Tokens Fee { get; set; }

		[CandidName("from_subaccount")]
		public TransferArgs.FromSubaccountInfo FromSubaccount { get; set; }

		[CandidName("to")]
		public AccountIdentifier To { get; set; }

		[CandidName("created_at_time")]
		public OptionalValue<TimeStamp> CreatedAtTime { get; set; }

		public TransferArgs(Memo memo, Tokens amount, Tokens fee, TransferArgs.FromSubaccountInfo fromSubaccount, AccountIdentifier to, OptionalValue<TimeStamp> createdAtTime)
		{
			this.Memo = memo;
			this.Amount = amount;
			this.Fee = fee;
			this.FromSubaccount = fromSubaccount;
			this.To = to;
			this.CreatedAtTime = createdAtTime;
		}

		public TransferArgs()
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
	}
}