using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.MainCanister.Models;

namespace Cosmicrafts.MainCanister.Models
{
	public class CharacterMetadata
	{
		[CandidName("basic")]
		public OptionalValue<BasicMetadata> Basic { get; set; }

		[CandidName("general")]
		public GeneralMetadata General { get; set; }

		[CandidName("skills")]
		public OptionalValue<SkillMetadata> Skills { get; set; }

		[CandidName("skins")]
		public OptionalValue<SkinMetadata> Skins { get; set; }

		[CandidName("soul")]
		public OptionalValue<SoulMetadata> Soul { get; set; }

		public CharacterMetadata(OptionalValue<BasicMetadata> basic, GeneralMetadata general, OptionalValue<SkillMetadata> skills, OptionalValue<SkinMetadata> skins, OptionalValue<SoulMetadata> soul)
		{
			this.Basic = basic;
			this.General = general;
			this.Skills = skills;
			this.Skins = skins;
			this.Soul = soul;
		}

		public CharacterMetadata()
		{
		}
	}
}