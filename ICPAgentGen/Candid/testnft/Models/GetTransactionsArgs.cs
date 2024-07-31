using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using CanisterPK.testnft.Models;

namespace CanisterPK.testnft.Models
{
	public class GetTransactionsArgs
	{
		[CandidName("account")]
		public OptionalValue<Account> Account { get; set; }

		[CandidName("limit")]
		public UnboundedUInt Limit { get; set; }

		[CandidName("offset")]
		public UnboundedUInt Offset { get; set; }

		public GetTransactionsArgs(OptionalValue<Account> account, UnboundedUInt limit, UnboundedUInt offset)
		{
			this.Account = account;
			this.Limit = limit;
			this.Offset = offset;
		}

		public GetTransactionsArgs()
		{
		}
	}
}