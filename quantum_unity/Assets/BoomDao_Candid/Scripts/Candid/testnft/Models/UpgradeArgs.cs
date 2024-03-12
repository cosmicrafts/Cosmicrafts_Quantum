using EdjCase.ICP.Candid.Mapping;
using CanisterPK.testnft.Models;
using System.Collections.Generic;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.testnft.Models
{
	public class UpgradeArgs
	{
		[CandidName("from")]
		public Account From { get; set; }

		[CandidName("metadata")]
		public Dictionary<string, Metadata> Metadata { get; set; }

		[CandidName("token_id")]
		public TokenId TokenId { get; set; }

		public UpgradeArgs(Account from, Dictionary<string, Metadata> metadata, TokenId tokenId)
		{
			this.From = from;
			this.Metadata = metadata;
			this.TokenId = tokenId;
		}

		public UpgradeArgs()
		{
		}
	}
}