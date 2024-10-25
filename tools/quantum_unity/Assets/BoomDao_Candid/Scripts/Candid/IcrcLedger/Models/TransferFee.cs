using EdjCase.ICP.Candid.Mapping;
using Candid.IcrcLedger.Models;

namespace Candid.IcrcLedger.Models
{
	public class TransferFee
	{
		[CandidName("transfer_fee")]
		public Tokens TransferFee_ { get; set; }

		public TransferFee(Tokens transferfee)
		{
			this.TransferFee_ = transferfee;
		}

		public TransferFee()
		{
		}
	}
}