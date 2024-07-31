using EdjCase.ICP.Candid.Mapping;
using CanisterPK.BoomToken.Models;

namespace CanisterPK.BoomToken.Models
{
	public class AllowanceArgs
	{
		[CandidName("account")]
		public Account Account { get; set; }

		[CandidName("spender")]
		public Account Spender { get; set; }

		public AllowanceArgs(Account account, Account spender)
		{
			this.Account = account;
			this.Spender = spender;
		}

		public AllowanceArgs()
		{
		}
	}
}