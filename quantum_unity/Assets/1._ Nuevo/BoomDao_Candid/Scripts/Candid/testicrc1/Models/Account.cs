using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using CanisterPK.testicrc1.Models;
using Subaccount = System.Collections.Generic.List<System.Byte>;

namespace CanisterPK.testicrc1.Models
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

		public class SubaccountInfo : OptionalValue<Subaccount>
		{
			public SubaccountInfo()
			{
			}

			public SubaccountInfo(Subaccount value) : base(value)
			{
			}
		}
	}
}