using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using TxIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;
using QueryArchiveFn = EdjCase.ICP.Candid.Models.Values.CandidFunc;

namespace CanisterPK.testicrc1.Models
{
	public class ArchivedTransaction
	{
		[CandidName("callback")]
		public QueryArchiveFn Callback { get; set; }

		[CandidName("length")]
		public UnboundedUInt Length { get; set; }

		[CandidName("start")]
		public TxIndex Start { get; set; }

		public ArchivedTransaction(QueryArchiveFn callback, UnboundedUInt length, TxIndex start)
		{
			this.Callback = callback;
			this.Length = length;
			this.Start = start;
		}

		public ArchivedTransaction()
		{
		}
	}
}