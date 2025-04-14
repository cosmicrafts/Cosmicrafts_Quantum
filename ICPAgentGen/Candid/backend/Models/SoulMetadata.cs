using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;

namespace Cosmicrafts.backend.Models
{
	public class SoulMetadata
	{
		[CandidName("birth")]
		public Time Birth { get; set; }

		[CandidName("combatExperience")]
		public UnboundedUInt CombatExperience { get; set; }

		[CandidName("gamesPlayed")]
		public OptionalValue<UnboundedUInt> GamesPlayed { get; set; }

		[CandidName("totalDamageDealt")]
		public OptionalValue<UnboundedUInt> TotalDamageDealt { get; set; }

		[CandidName("totalKills")]
		public OptionalValue<UnboundedUInt> TotalKills { get; set; }

		public SoulMetadata(Time birth, UnboundedUInt combatExperience, OptionalValue<UnboundedUInt> gamesPlayed, OptionalValue<UnboundedUInt> totalDamageDealt, OptionalValue<UnboundedUInt> totalKills)
		{
			this.Birth = birth;
			this.CombatExperience = combatExperience;
			this.GamesPlayed = gamesPlayed;
			this.TotalDamageDealt = totalDamageDealt;
			this.TotalKills = totalKills;
		}

		public SoulMetadata()
		{
		}
	}
}