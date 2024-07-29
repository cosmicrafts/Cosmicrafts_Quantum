using EdjCase.ICP.Candid.Mapping;
using CanisterPK.CanisterLogin.Models;
using EdjCase.ICP.Candid.Models;
using Username1 = System.String;
using Username = System.String;
using Playerid2 = EdjCase.ICP.Candid.Models.Principal;
using Matchid1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using MatchID = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Level1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Level = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Avatarid1 = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.CanisterLogin.Models
{
	public class FullMatchData
	{
		[CandidName("matchID")]
		public Matchid1 MatchID { get; set; }

		[CandidName("player1")]
		public FullMatchData.Player1Info Player1 { get; set; }

		[CandidName("player2")]
		public FullMatchData.Player2Info Player2 { get; set; }

		[CandidName("status")]
		public MMStatus Status { get; set; }

		public FullMatchData(Matchid1 matchID, FullMatchData.Player1Info player1, FullMatchData.Player2Info player2, MMStatus status)
		{
			this.MatchID = matchID;
			this.Player1 = player1;
			this.Player2 = player2;
			this.Status = status;
		}

		public FullMatchData()
		{
		}

		public class Player1Info
		{
			[CandidName("avatar")]
			public Avatarid1 Avatar { get; set; }

			[CandidName("elo")]
			public double Elo { get; set; }

			[CandidName("id")]
			public Playerid2 Id { get; set; }

			[CandidName("level")]
			public Level1 Level { get; set; }

			[CandidName("matchAccepted")]
			public bool MatchAccepted { get; set; }

			[CandidName("playerGameData")]
			public string PlayerGameData { get; set; }

			[CandidName("username")]
			public Username1 Username { get; set; }

			public Player1Info(Avatarid1 avatar, double elo, Playerid2 id, Level1 level, bool matchAccepted, string playerGameData, Username1 username)
			{
				this.Avatar = avatar;
				this.Elo = elo;
				this.Id = id;
				this.Level = level;
				this.MatchAccepted = matchAccepted;
				this.PlayerGameData = playerGameData;
				this.Username = username;
			}

			public Player1Info()
			{
			}
		}

		public class Player2Info : OptionalValue<FullMatchData.Player2Info.Player2InfoValue>
		{
			public Player2Info()
			{
			}

			public Player2Info(FullMatchData.Player2Info.Player2InfoValue value) : base(value)
			{
			}

			public class Player2InfoValue
			{
				[CandidName("avatar")]
				public Avatarid1 Avatar { get; set; }

				[CandidName("elo")]
				public double Elo { get; set; }

				[CandidName("id")]
				public Playerid2 Id { get; set; }

				[CandidName("level")]
				public Level1 Level { get; set; }

				[CandidName("matchAccepted")]
				public bool MatchAccepted { get; set; }

				[CandidName("playerGameData")]
				public string PlayerGameData { get; set; }

				[CandidName("username")]
				public Username1 Username { get; set; }

				public Player2InfoValue(Avatarid1 avatar, double elo, Playerid2 id, Level1 level, bool matchAccepted, string playerGameData, Username1 username)
				{
					this.Avatar = avatar;
					this.Elo = elo;
					this.Id = id;
					this.Level = level;
					this.MatchAccepted = matchAccepted;
					this.PlayerGameData = playerGameData;
					this.Username = username;
				}

				public Player2InfoValue()
				{
				}
			}
		}
	}
}