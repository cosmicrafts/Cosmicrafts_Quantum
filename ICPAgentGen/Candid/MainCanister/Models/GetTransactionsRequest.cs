using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using TxIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.MainCanister.Models
{
	public class GetTransactionsRequest
	{
		[CandidName("length")]
		public UnboundedUInt Length { get; set; }

		[CandidName("start")]
		public TxIndex Start { get; set; }

		public GetTransactionsRequest(UnboundedUInt length, TxIndex start)
		{
			this.Length = length;
			this.Start = start;
		}

		public GetTransactionsRequest()
		{
		}
	}
}