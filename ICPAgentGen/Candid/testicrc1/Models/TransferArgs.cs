using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using CanisterPK.testicrc1.Models;
using Subaccount = System.Collections.Generic.List<System.Byte>;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.testicrc1.Models
{
	public class TransferArgs
	{
		[CandidName("amount")]
		public Balance Amount { get; set; }

		[CandidName("created_at_time")]
		public OptionalValue<ulong> CreatedAtTime { get; set; }

		[CandidName("fee")]
		public OptionalValue<Balance> Fee { get; set; }

		[CandidName("from_subaccount")]
		public OptionalValue<Subaccount> FromSubaccount { get; set; }

		[CandidName("memo")]
		public OptionalValue<List<byte>> Memo { get; set; }

		[CandidName("to")]
		public Account To { get; set; }

		public TransferArgs(Balance amount, OptionalValue<ulong> createdAtTime, OptionalValue<Balance> fee, OptionalValue<Subaccount> fromSubaccount, OptionalValue<List<byte>> memo, Account to)
		{
			this.Amount = amount;
			this.CreatedAtTime = createdAtTime;
			this.Fee = fee;
			this.FromSubaccount = fromSubaccount;
			this.Memo = memo;
			this.To = to;
		}

		public TransferArgs()
		{
		}
	}
}