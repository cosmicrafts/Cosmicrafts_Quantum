using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.MainCanister.Models;
using System.Collections.Generic;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.MainCanister.Models
{
	public class PlayerGameData
	{
		[CandidName("deck")]
		public PlayerGameData.DeckInfo Deck { get; set; }

		public PlayerGameData(PlayerGameData.DeckInfo deck)
		{
			this.Deck = deck;
		}

		public PlayerGameData()
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