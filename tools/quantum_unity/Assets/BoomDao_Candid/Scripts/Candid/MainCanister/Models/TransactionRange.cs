using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Cosmicrafts.MainCanister.Models;

namespace Cosmicrafts.MainCanister.Models
{
	public class TransactionRange
	{
		[CandidName("transactions")]
		public List<Transaction1> Transactions { get; set; }

		public TransactionRange(List<Transaction1> transactions)
		{
			this.Transactions = transactions;
		}

		public TransactionRange()
		{
		}
	}
}