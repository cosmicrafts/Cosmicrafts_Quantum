using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using CanisterPK.NFTsICHub.Models;
using TokenIdentifier = System.String;
using SubAccount = System.Collections.Generic.List<System.Byte>;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.NFTsICHub.Models
{
	public class ApproveRequest
	{
		[CandidName("allowance")]
		public Balance Allowance { get; set; }

		[CandidName("spender")]
		public Principal Spender { get; set; }

		[CandidName("subaccount")]
		public ApproveRequest.SubaccountInfo Subaccount { get; set; }

		[CandidName("token")]
		public TokenIdentifier Token { get; set; }

		public ApproveRequest(Balance allowance, Principal spender, ApproveRequest.SubaccountInfo subaccount, TokenIdentifier token)
		{
			this.Allowance = allowance;
			this.Spender = spender;
			this.Subaccount = subaccount;
			this.Token = token;
		}

		public ApproveRequest()
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