using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Cosmicrafts.backend.Models
{
	public class OverallGamesWithFaction
	{
		[CandidName("factionID")]
		public UnboundedUInt FactionID { get; set; }

		[CandidName("gamesPlayed")]
		public UnboundedUInt GamesPlayed { get; set; }

		public OverallGamesWithFaction(UnboundedUInt factionID, UnboundedUInt gamesPlayed)
		{
			this.FactionID = factionID;
			this.GamesPlayed = gamesPlayed;
		}

		public OverallGamesWithFaction()
		{
		}
	}
}