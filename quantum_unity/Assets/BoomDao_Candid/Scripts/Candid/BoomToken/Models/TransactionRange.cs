using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Boom.BoomToken.Models;

namespace Boom.BoomToken.Models
{
	public class TransactionRange
	{
		[CandidName("transactions")]
		public List<Transaction> Transactions { get; set; }

		public TransactionRange(List<Transaction> transactions)
		{
			this.Transactions = transactions;
		}

		public TransactionRange()
		{
		}
	}
}