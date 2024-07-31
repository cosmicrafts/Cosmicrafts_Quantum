using EdjCase.ICP.Candid.Mapping;
using Username1 = System.String;
using Username = System.String;
using Playerid2 = EdjCase.ICP.Candid.Models.Principal;

namespace CanisterPK.CanisterLogin.Models
{
	public class MMInfo
	{
		[CandidName("elo")]
		public double Elo { get; set; }

		[CandidName("id")]
		public Playerid2 Id { get; set; }

		[CandidName("lastPlayerActive")]
		public ulong LastPlayerActive { get; set; }

		[CandidName("matchAccepted")]
		public bool MatchAccepted { get; set; }

		[CandidName("playerGameData")]
		public string PlayerGameData { get; set; }

		[CandidName("username")]
		public Username1 Username { get; set; }

		public MMInfo(double elo, Playerid2 id, ulong lastPlayerActive, bool matchAccepted, string playerGameData, Username1 username)
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