using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.backend.Models;
using System.Collections.Generic;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.backend.Models
{
	public class Burn
	{
		[CandidName("amount")]
		public Balance Amount { get; set; }

		[CandidName("created_at_time")]
		public OptionalValue<ulong> CreatedAtTime { get; set; }

		[CandidName("from")]
		public Account1 From { get; set; }

		[CandidName("memo")]
		public OptionalValue<List<byte>> Memo { get; set; }

		public Burn(Balance amount, OptionalValue<ulong> createdAtTime, Account1 from, OptionalValue<List<byte>> memo)
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