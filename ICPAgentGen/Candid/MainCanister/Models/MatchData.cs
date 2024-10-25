using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.MainCanister.Models;
using EdjCase.ICP.Candid.Models;
using Matchid1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using MatchID = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.MainCanister.Models
{
	public class MatchData
	{
		[CandidName("matchID")]
		public Matchid1 MatchID { get; set; }

		[CandidName("player1")]
		public MMInfo Player1 { get; set; }

		[CandidName("player2")]
		public OptionalValue<MMInfo> Player2 { get; set; }

		[CandidName("status")]
		public MMStatus Status { get; set; }

		public MatchData(Matchid1 matchID, MMInfo player1, OptionalValue<MMInfo> player2, MMStatus status)
		{
			this.MatchID = matchID;
			this.Player1 = player1;
			this.Player2 = player2;
			this.Status = status;
		}

		public MatchData()
		{
		}
	}
}