using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using CanisterPK.flux.Models;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.flux.Models
{
	public class Mint1
	{
		[CandidName("amount")]
		public Balance Amount { get; set; }

		[CandidName("created_at_time")]
		public OptionalValue<ulong> CreatedAtTime { get; set; }

		[CandidName("memo")]
		public OptionalValue<List<byte>> Memo { get; set; }

		[CandidName("to")]
		public Account To { get; set; }

		public Mint1(Balance amount, OptionalValue<ulong> createdAtTime, OptionalValue<List<byte>> memo, Account to)
		{
			this.Amount = amount;
			this.CreatedAtTime = createdAtTime;
			this.Memo = memo;
			this.To = to;
		}

		public Mint1()
		{
		}
	}
}