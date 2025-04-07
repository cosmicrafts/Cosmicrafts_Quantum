using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;

namespace Cosmicrafts.backend.Models
{
	public class PlayerGameData
	{
		[CandidName("deck")]
		public List<UnboundedUInt> Deck { get; set; }

		public PlayerGameData(List<UnboundedUInt> deck)
		{
			this.Deck = deck;
		}

		public PlayerGameData()
		{
		}
	}
}