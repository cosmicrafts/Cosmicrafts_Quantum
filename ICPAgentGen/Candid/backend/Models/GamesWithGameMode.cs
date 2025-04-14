using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Cosmicrafts.backend.Models
{
	public class GamesWithGameMode
	{
		[CandidName("gameModeID")]
		public UnboundedUInt GameModeID { get; set; }

		[CandidName("gamesPlayed")]
		public UnboundedUInt GamesPlayed { get; set; }

		[CandidName("gamesWon")]
		public UnboundedUInt GamesWon { get; set; }

		public GamesWithGameMode(UnboundedUInt gameModeID, UnboundedUInt gamesPlayed, UnboundedUInt gamesWon)
		{
			this.GameModeID = gameModeID;
			this.GamesPlayed = gamesPlayed;
			this.GamesWon = gamesWon;
		}

		public GamesWithGameMode()
		{
		}
	}
}