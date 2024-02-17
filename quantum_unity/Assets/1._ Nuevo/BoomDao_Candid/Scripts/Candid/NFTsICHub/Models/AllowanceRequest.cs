using EdjCase.ICP.Candid.Mapping;
using CanisterPK.NFTsICHub.Models;
using EdjCase.ICP.Candid.Models;
using TokenIdentifier = System.String;

namespace CanisterPK.NFTsICHub.Models
{
	public class AllowanceRequest
	{
		[CandidName("owner")]
		public User Owner { get; set; }

		[CandidName("spender")]
		public Principal Spender { get; set; }

		[CandidName("token")]
		public TokenIdentifier Token { get; set; }

		public AllowanceRequest(User owner, Principal spender, TokenIdentifier token)
		{
			this.Owner = owner;
			this.Spender = spender;
			this.Token = token;
		}

		public AllowanceRequest()
		{
		}
	}
}