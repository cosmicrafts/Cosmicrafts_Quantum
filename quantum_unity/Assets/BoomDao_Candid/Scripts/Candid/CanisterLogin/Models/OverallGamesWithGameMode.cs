using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace CanisterPK.CanisterLogin.Models
{
	public class OverallGamesWithGameMode
	{
		[CandidName("gameModeID")]
		public UnboundedUInt GameModeID { get; set; }

		[CandidName("gamesPlayed")]
		public UnboundedUInt GamesPlayed { get; set; }

		public OverallGamesWithGameMode(UnboundedUInt gameModeID, UnboundedUInt gamesPlayed)
		{
			this.GameModeID = gameModeID;
			this.GamesPlayed = gamesPlayed;
		}

		public OverallGamesWithGameMode()
		{
		}
	}
}