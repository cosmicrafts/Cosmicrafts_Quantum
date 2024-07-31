using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace CanisterPK.CanisterLogin.Models
{
	public class GamesWithFaction
	{
		[CandidName("factionID")]
		public UnboundedUInt FactionID { get; set; }

		[CandidName("gamesPlayed")]
		public UnboundedUInt GamesPlayed { get; set; }

		[CandidName("gamesWon")]
		public UnboundedUInt GamesWon { get; set; }

		public GamesWithFaction(UnboundedUInt factionID, UnboundedUInt gamesPlayed, UnboundedUInt gamesWon)
		{
			this.FactionID = factionID;
			this.GamesPlayed = gamesPlayed;
			this.GamesWon = gamesWon;
		}

		public GamesWithFaction()
		{
		}
	}
}