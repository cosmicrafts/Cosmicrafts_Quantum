using EdjCase.ICP.Candid.Mapping;
using Candid.IcrcLedger.Models;
using EdjCase.ICP.Candid.Models;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using Memo = System.UInt64;
using TextAccountIdentifier = System.String;

namespace Candid.IcrcLedger.Models
{
	public class SendArgs
	{
		[CandidName("memo")]
		public Memo Memo { get; set; }

		[CandidName("amount")]
		public Tokens Amount { get; set; }

		[CandidName("fee")]
		public Tokens Fee { get; set; }

		[CandidName("from_subaccount")]
		public SendArgs.FromSubaccountInfo FromSubaccount { get; set; }

		[CandidName("to")]
		public TextAccountIdentifier To { get; set; }

		[CandidName("created_at_time")]
		public OptionalValue<TimeStamp> CreatedAtTime { get; set; }

		public SendArgs(Memo memo, Tokens amount, Tokens fee, SendArgs.FromSubaccountInfo fromSubaccount, TextAccountIdentifier to, OptionalValue<TimeStamp> createdAtTime)
		{
			this.Memo = memo;
			this.Amount = amount;
			this.Fee = fee;
			this.FromSubaccount = fromSubaccount;
			this.To = to;
			this.CreatedAtTime = createdAtTime;
		}

		public SendArgs()
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