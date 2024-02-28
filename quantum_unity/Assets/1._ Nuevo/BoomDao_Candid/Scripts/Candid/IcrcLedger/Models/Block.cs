using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using Candid.IcrcLedger.Models;

namespace Candid.IcrcLedger.Models
{
	public class Block
	{
		[CandidName("parent_hash")]
		public OptionalValue<List<byte>> ParentHash { get; set; }

		[CandidName("transaction")]
		public Transaction Transaction { get; set; }

		[CandidName("timestamp")]
		public TimeStamp Timestamp { get; set; }

		public Block(OptionalValue<List<byte>> parentHash, Transaction transaction, TimeStamp timestamp)
		{
			this.ParentHash = parentHash;
			this.Transaction = transaction;
			this.Timestamp = timestamp;
		}

		public Block()
		{
		}
	}
}