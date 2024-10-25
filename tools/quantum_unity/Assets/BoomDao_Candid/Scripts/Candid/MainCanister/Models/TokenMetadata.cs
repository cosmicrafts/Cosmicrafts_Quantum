using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.MainCanister.Models;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.MainCanister.Models
{
	public class TokenMetadata
	{
		[CandidName("metadata")]
		public Metadata Metadata { get; set; }

		[CandidName("owner")]
		public Account Owner { get; set; }

		[CandidName("tokenId")]
		public TokenId TokenId { get; set; }

		public TokenMetadata(Metadata metadata, Account owner, TokenId tokenId)
		{
			this.Metadata = metadata;
			this.Owner = owner;
			this.TokenId = tokenId;
		}

		public TokenMetadata()
		{
		}
	}
}