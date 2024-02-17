using EdjCase.ICP.Candid.Mapping;
using CanisterPK.NFTsICHub.Models;
using TokenIdentifier = System.String;

namespace CanisterPK.NFTsICHub.Models
{
	public class BalanceRequest
	{
		[CandidName("token")]
		public TokenIdentifier Token { get; set; }

		[CandidName("user")]
		public User User { get; set; }

		public BalanceRequest(TokenIdentifier token, User user)
		{
			this.Token = token;
			this.User = user;
		}

		public BalanceRequest()
		{
		}
	}
}