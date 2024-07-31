using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using CanisterPK.testicrc1.Models;
using System.Collections.Generic;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.testicrc1.Models
{
	public class Burn
	{
		[CandidName("amount")]
		public Balance Amount { get; set; }

		[CandidName("created_at_time")]
		public OptionalValue<ulong> CreatedAtTime { get; set; }

		[CandidName("from")]
		public Account From { get; set; }

		[CandidName("memo")]
		public OptionalValue<List<byte>> Memo { get; set; }

		public Burn(Balance amount, OptionalValue<ulong> createdAtTime, Account from, OptionalValue<List<byte>> memo)
		{
			this.Amount = amount;
			this.CreatedAtTime = createdAtTime;
			this.From = from;
			this.Memo = memo;
		}

		public Burn()
		{
		}
	}
}