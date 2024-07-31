using EdjCase.ICP.Candid.Mapping;
using CanisterPK.chests.Models;
using System.Collections.Generic;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.chests.Models
{
	public class UpdateArgs
	{
		[CandidName("from")]
		public Account From { get; set; }

		[CandidName("metadata")]
		public Dictionary<string, Metadata> Metadata { get; set; }

		[CandidName("token_id")]
		public TokenId TokenId { get; set; }

		public UpdateArgs(Account from, Dictionary<string, Metadata> metadata, TokenId tokenId)
		{
			this.From = from;
			this.Metadata = metadata;
			this.TokenId = tokenId;
		}

		public UpdateArgs()
		{
		}
	}
}