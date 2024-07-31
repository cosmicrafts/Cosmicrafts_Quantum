using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using CanisterPK.flux.Models;
using System.Collections.Generic;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.flux.Models
{
	public class Transfer
	{
		[CandidName("amount")]
		public Balance Amount { get; set; }

		[CandidName("created_at_time")]
		public OptionalValue<ulong> CreatedAtTime { get; set; }

		[CandidName("fee")]
		public OptionalValue<Balance> Fee { get; set; }

		[CandidName("from")]
		public Account From { get; set; }

		[CandidName("memo")]
		public OptionalValue<List<byte>> Memo { get; set; }

		[CandidName("to")]
		public Account To { get; set; }

		public Transfer(Balance amount, OptionalValue<ulong> createdAtTime, OptionalValue<Balance> fee, Account from, OptionalValue<List<byte>> memo, Account to)
		{
			this.Amount = amount;
			this.CreatedAtTime = createdAtTime;
			this.Fee = fee;
			this.From = from;
			this.Memo = memo;
			this.To = to;
		}

		public Transfer()
		{
		}
	}
}