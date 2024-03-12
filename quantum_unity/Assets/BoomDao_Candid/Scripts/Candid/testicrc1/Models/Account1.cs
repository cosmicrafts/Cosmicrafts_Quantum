using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using CanisterPK.testicrc1.Models;
using Subaccount = System.Collections.Generic.List<System.Byte>;

namespace CanisterPK.testicrc1.Models
{
	public class Account1
	{
		[CandidName("owner")]
		public Principal Owner { get; set; }

		[CandidName("subaccount")]
		public Account1.SubaccountInfo Subaccount { get; set; }

		public Account1(Principal owner, Account1.SubaccountInfo subaccount)
		{
			this.Owner = owner;
			this.Subaccount = subaccount;
		}

		public Account1()
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