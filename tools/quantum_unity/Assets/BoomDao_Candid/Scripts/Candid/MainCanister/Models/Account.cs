using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using Subaccount = System.Collections.Generic.List<System.Byte>;

namespace Cosmicrafts.MainCanister.Models
{
	public class Account
	{
		[CandidName("owner")]
		public Principal Owner { get; set; }

		[CandidName("subaccount")]
		public OptionalValue<List<byte>> Subaccount { get; set; }

		public Account(Principal owner, OptionalValue<List<byte>> subaccount)
		{
			this.Owner = owner;
			this.Subaccount = subaccount;
		}

		public Account()
		{
		}
	}
}