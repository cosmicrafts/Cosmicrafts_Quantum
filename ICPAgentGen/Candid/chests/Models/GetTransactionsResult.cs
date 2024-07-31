using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using CanisterPK.chests.Models;

namespace CanisterPK.chests.Models
{
	public class GetTransactionsResult
	{
		[CandidName("total")]
		public UnboundedUInt Total { get; set; }

		[CandidName("transactions")]
		public List<Transaction> Transactions { get; set; }

		public GetTransactionsResult(UnboundedUInt total, List<Transaction> transactions)
		{
			this.Total = total;
			this.Transactions = transactions;
		}

		public GetTransactionsResult()
		{
		}
	}
}