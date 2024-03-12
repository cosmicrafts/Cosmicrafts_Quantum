using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using CanisterPK.CanisterMatchMaking.Models;

namespace CanisterPK.CanisterMatchMaking.Models
{
	public class MatchData
	{
		[CandidName("gameId")]
		public UnboundedUInt GameId { get; set; }

		[CandidName("player1")]
		public PlayerInfo Player1 { get; set; }

		[CandidName("player2")]
		public OptionalValue<PlayerInfo> Player2 { get; set; }

		[CandidName("status")]
		public MatchmakingStatus Status { get; set; }

		public MatchData(UnboundedUInt gameId, PlayerInfo player1, OptionalValue<PlayerInfo> player2, MatchmakingStatus status)
		{
			this.GameId = gameId;
			this.Player1 = player1;
			this.Player2 = player2;
			this.Status = status;
		}

		public MatchData()
		{
		}
	}
}