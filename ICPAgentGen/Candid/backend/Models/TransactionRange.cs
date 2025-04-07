using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Cosmicrafts.backend.Models;

namespace Cosmicrafts.backend.Models
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