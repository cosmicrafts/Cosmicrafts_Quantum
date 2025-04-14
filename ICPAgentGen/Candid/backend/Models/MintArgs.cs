using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.backend.Models;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.backend.Models
{
	public class MintArgs
	{
		[CandidName("metadata")]
		public Metadata Metadata { get; set; }

		[CandidName("to")]
		public Account To { get; set; }

		[CandidName("token_id")]
		public TokenId TokenId { get; set; }

		public MintArgs(Metadata metadata, Account to, TokenId tokenId)
		{
			this.Metadata = metadata;
			this.To = to;
			this.TokenId = tokenId;
		}

		public MintArgs()
		{
		}
	}
}