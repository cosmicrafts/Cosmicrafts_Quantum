using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using Candid.IcrcLedger.Models;
using Memo = System.UInt64;

namespace Candid.IcrcLedger.Models
{
	public class Transaction
	{
		[CandidName("memo")]
		public Memo Memo { get; set; }

		[CandidName("icrc1_memo")]
		public OptionalValue<List<byte>> Icrc1Memo { get; set; }

		[CandidName("operation")]
		public OptionalValue<Operation> Operation { get; set; }

		[CandidName("created_at_time")]
		public TimeStamp CreatedAtTime { get; set; }

		public Transaction(Memo memo, OptionalValue<List<byte>> icrc1Memo, OptionalValue<Operation> operation, TimeStamp createdAtTime)
		{
			this.Memo = memo;
			this.Icrc1Memo = icrc1Memo;
			this.Operation = operation;
			this.CreatedAtTime = createdAtTime;
		}

		public Transaction()
		{
		}
	}
}