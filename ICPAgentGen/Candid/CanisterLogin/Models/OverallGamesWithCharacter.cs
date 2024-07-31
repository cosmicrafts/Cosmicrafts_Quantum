using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace CanisterPK.CanisterLogin.Models
{
	public class OverallGamesWithCharacter
	{
		[CandidName("characterID")]
		public UnboundedUInt CharacterID { get; set; }

		[CandidName("gamesPlayed")]
		public UnboundedUInt GamesPlayed { get; set; }

		public OverallGamesWithCharacter(UnboundedUInt characterID, UnboundedUInt gamesPlayed)
		{
			this.CharacterID = characterID;
			this.GamesPlayed = gamesPlayed;
		}

		public OverallGamesWithCharacter()
		{
		}
	}
}