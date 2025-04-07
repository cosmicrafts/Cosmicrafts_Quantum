using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.backend.Models;
using System.Collections.Generic;
using Subaccount = System.Collections.Generic.List<System.Byte>;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.backend.Models
{
	public class BurnArgs
	{
		[CandidName("amount")]
		public Balance Amount { get; set; }

		[CandidName("created_at_time")]
		public OptionalValue<ulong> CreatedAtTime { get; set; }

		[CandidName("from_subaccount")]
		public BurnArgs.FromSubaccountInfo FromSubaccount { get; set; }

		[CandidName("memo")]
		public OptionalValue<List<byte>> Memo { get; set; }

		public BurnArgs(Balance amount, OptionalValue<ulong> createdAtTime, BurnArgs.FromSubaccountInfo fromSubaccount, OptionalValue<List<byte>> memo)
		{
			this.Amount = amount;
			this.CreatedAtTime = createdAtTime;
			this.FromSubaccount = fromSubaccount;
			this.Memo = memo;
		}

		public BurnArgs()
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
	}
}