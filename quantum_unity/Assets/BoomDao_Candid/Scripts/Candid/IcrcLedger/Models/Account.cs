using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Candid.IcrcLedger.Models;
using SubAccount = System.Collections.Generic.List<System.Byte>;

namespace Candid.IcrcLedger.Models
{
	public class Account
	{
		[CandidName("owner")]
		public Principal Owner { get; set; }

		[CandidName("subaccount")]
		public Account.SubaccountInfo Subaccount { get; set; }

		public Account(Principal owner, Account.SubaccountInfo subaccount)
		{
			this.Owner = owner;
			this.Subaccount = subaccount;
		}

		public Account()
		{
		}

		public class SubaccountInfo : OptionalValue<SubAccount>
		{
			public SubaccountInfo()
			{
			}

			public SubaccountInfo(SubAccount value) : base(value)
			{
			}
		}
	}
}