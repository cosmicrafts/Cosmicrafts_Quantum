using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Cosmicrafts.MainCanister.Models
{
	public class GamesWithCharacter
	{
		[CandidName("characterID")]
		public UnboundedUInt CharacterID { get; set; }

		[CandidName("gamesPlayed")]
		public UnboundedUInt GamesPlayed { get; set; }

		[CandidName("gamesWon")]
		public UnboundedUInt GamesWon { get; set; }

		public GamesWithCharacter(UnboundedUInt characterID, UnboundedUInt gamesPlayed, UnboundedUInt gamesWon)
		{
			this.CharacterID = characterID;
			this.GamesPlayed = gamesPlayed;
			this.GamesWon = gamesWon;
		}

		public GamesWithCharacter()
		{
		}
	}
}