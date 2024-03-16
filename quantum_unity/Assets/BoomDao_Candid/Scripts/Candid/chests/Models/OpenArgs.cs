using EdjCase.ICP.Candid.Mapping;
using CanisterPK.chests.Models;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.chests.Models
{
	public class OpenArgs
	{
		[CandidName("from")]
		public Account From { get; set; }

		[CandidName("token_id")]
		public TokenId TokenId { get; set; }

		public OpenArgs(Account from, TokenId tokenId)
		{
			this.From = from;
			this.TokenId = tokenId;
		}

		public OpenArgs()
		{
		}
	}
}