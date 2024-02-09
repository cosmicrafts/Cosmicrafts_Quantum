using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using UserId = EdjCase.ICP.Candid.Models.Principal;

namespace CanisterPK.CanisterMatchMaking.Models
{
	public class PlayerInfo
	{
		[CandidName("elo")]
		public UnboundedUInt Elo { get; set; }

		[CandidName("id")]
		public UserId Id { get; set; }

		[CandidName("lastPlayerActive")]
		public ulong LastPlayerActive { get; set; }

		[CandidName("matchAccepted")]
		public bool MatchAccepted { get; set; }

		[CandidName("playerGameData")]
		public string PlayerGameData { get; set; }

		public PlayerInfo(UnboundedUInt elo, UserId id, ulong lastPlayerActive, bool matchAccepted, string playerGameData)
		{
			this.Elo = elo;
			this.Id = id;
			this.LastPlayerActive = lastPlayerActive;
			this.MatchAccepted = matchAccepted;
			this.PlayerGameData = playerGameData;
		}

		public PlayerInfo()
		{
		}
	}
}