using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using CanisterPK.testnft.Models;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.testnft.Models
{
	public class TokenMetadata
	{
		[CandidName("metadata")]
		public Dictionary<string, Metadata> Metadata { get; set; }

		[CandidName("owner")]
		public Account Owner { get; set; }

		[CandidName("tokenId")]
		public TokenId TokenId { get; set; }

		public TokenMetadata(Dictionary<string, Metadata> metadata, Account owner, TokenId tokenId)
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