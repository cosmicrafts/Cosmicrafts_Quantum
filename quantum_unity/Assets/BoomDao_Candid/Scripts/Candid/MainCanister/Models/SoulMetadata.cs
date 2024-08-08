using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;

namespace Cosmicrafts.MainCanister.Models
{
	public class SoulMetadata
	{
		[CandidName("birth")]
		public Time Birth { get; set; }

		[CandidName("gamesPlayed")]
		public OptionalValue<UnboundedUInt> GamesPlayed { get; set; }

		[CandidName("totalDamageDealt")]
		public OptionalValue<UnboundedUInt> TotalDamageDealt { get; set; }

		[CandidName("totalKills")]
		public OptionalValue<UnboundedUInt> TotalKills { get; set; }

		public SoulMetadata(Time birth, OptionalValue<UnboundedUInt> gamesPlayed, OptionalValue<UnboundedUInt> totalDamageDealt, OptionalValue<UnboundedUInt> totalKills)
		{
			this.Birth = birth;
			this.GamesPlayed = gamesPlayed;
			this.TotalDamageDealt = totalDamageDealt;
			this.TotalKills = totalKills;
		}

		public SoulMetadata()
		{
		}
	}
}