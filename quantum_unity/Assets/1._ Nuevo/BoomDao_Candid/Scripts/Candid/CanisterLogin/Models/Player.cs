using EdjCase.ICP.Candid.Mapping;
using PlayerName = System.String;
using PlayerId = EdjCase.ICP.Candid.Models.Principal;
using Level = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.CanisterLogin.Models
{
	public class Player
	{
		[CandidName("id")]
		public PlayerId Id { get; set; }

		[CandidName("level")]
		public Level Level { get; set; }

		[CandidName("name")]
		public PlayerName Name { get; set; }

		public Player(PlayerId id, Level level, PlayerName name)
		{
			this.Id = id;
			this.Level = level;
			this.Name = name;
		}

		public Player()
		{
		}
	}
}