using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using TxIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.testicrc1.Models
{
	public class Gettransactionsrequest1
	{
		[CandidName("length")]
		public UnboundedUInt Length { get; set; }

		[CandidName("start")]
		public TxIndex Start { get; set; }

		public Gettransactionsrequest1(UnboundedUInt length, TxIndex start)
		{
			this.Length = length;
			this.Start = start;
		}

		public Gettransactionsrequest1()
		{
		}
	}
}