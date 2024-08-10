using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.MainCanister.Models;
using System.Collections.Generic;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.MainCanister.Models
{
	public class Playergamedata1
	{
		[CandidName("deck")]
		public Playergamedata1.DeckInfo Deck { get; set; }

		public Playergamedata1(Playergamedata1.DeckInfo deck)
		{
			this.Deck = deck;
		}

		public Playergamedata1()
		{
		}

		public class DeckInfo : List<TokenId>
		{
			public DeckInfo()
			{
			}
		}
	}
}