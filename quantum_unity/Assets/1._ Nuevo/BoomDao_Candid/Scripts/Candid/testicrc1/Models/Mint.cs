using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using CanisterPK.testicrc1.Models;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.testicrc1.Models
{
	public class Mint
	{
		[CandidName("amount")]
		public Balance Amount { get; set; }

		[CandidName("created_at_time")]
		public OptionalValue<ulong> CreatedAtTime { get; set; }

		[CandidName("memo")]
		public OptionalValue<List<byte>> Memo { get; set; }

		[CandidName("to")]
		public Account To { get; set; }

		public Mint(Balance amount, OptionalValue<ulong> createdAtTime, OptionalValue<List<byte>> memo, Account to)
		{
			this.Amount = amount;
			this.CreatedAtTime = createdAtTime;
			this.Memo = memo;
			this.To = to;
		}

		public Mint()
		{
		}
	}
}