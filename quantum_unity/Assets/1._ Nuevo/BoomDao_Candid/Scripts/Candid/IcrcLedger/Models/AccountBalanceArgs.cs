using EdjCase.ICP.Candid.Mapping;
using AccountIdentifier = System.Collections.Generic.List<System.Byte>;

namespace Candid.IcrcLedger.Models
{
	public class AccountBalanceArgs
	{
		[CandidName("account")]
		public AccountIdentifier Account { get; set; }

		public AccountBalanceArgs(AccountIdentifier account)
		{
			this.Account = account;
		}

		public AccountBalanceArgs()
		{
		}
	}
}