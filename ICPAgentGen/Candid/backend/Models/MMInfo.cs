using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.backend.Models;
using Username1 = System.String;
using Username = System.String;
using Playerid1 = EdjCase.ICP.Candid.Models.Principal;

namespace Cosmicrafts.backend.Models
{
	public class MMInfo
	{
		[CandidName("elo")]
		public double Elo { get; set; }

		[CandidName("id")]
		public Playerid1 Id { get; set; }

		[CandidName("lastPlayerActive")]
		public ulong LastPlayerActive { get; set; }

		[CandidName("matchAccepted")]
		public bool MatchAccepted { get; set; }

		[CandidName("playerGameData")]
		public PlayerGameData PlayerGameData { get; set; }

		[CandidName("username")]
		public Username1 Username { get; set; }

		public MMInfo(double elo, Playerid1 id, ulong lastPlayerActive, bool matchAccepted, PlayerGameData playerGameData, Username1 username)
		{
			this.Elo = elo;
			this.Id = id;
			this.LastPlayerActive = lastPlayerActive;
			this.MatchAccepted = matchAccepted;
			this.PlayerGameData = playerGameData;
			this.Username = username;
		}

		public MMInfo()
		{
		}
	}
}