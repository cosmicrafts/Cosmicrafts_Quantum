using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using CanisterPK.testnft.Models;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.testnft.Models
{
	public class MintArgs
	{
		[CandidName("metadata")]
		public Dictionary<string, Metadata> Metadata { get; set; }

		[CandidName("to")]
		public Account To { get; set; }

		[CandidName("token_id")]
		public TokenId TokenId { get; set; }

		public MintArgs(Dictionary<string, Metadata> metadata, Account to, TokenId tokenId)
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