using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using CanisterPK.flux.Models;
using EdjCase.ICP.Candid.Models;
using TxIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.flux.Models
{
	public class GetTransactionsResponse
	{
		[CandidName("archived_transactions")]
		public List<ArchivedTransaction> ArchivedTransactions { get; set; }

		[CandidName("first_index")]
		public TxIndex FirstIndex { get; set; }

		[CandidName("log_length")]
		public UnboundedUInt LogLength { get; set; }

		[CandidName("transactions")]
		public List<Transaction> Transactions { get; set; }

		public GetTransactionsResponse(List<ArchivedTransaction> archivedTransactions, TxIndex firstIndex, UnboundedUInt logLength, List<Transaction> transactions)
		{
			this.ArchivedTransactions = archivedTransactions;
			this.FirstIndex = firstIndex;
			this.LogLength = logLength;
			this.Transactions = transactions;
		}

		public GetTransactionsResponse()
		{
		}
	}
}