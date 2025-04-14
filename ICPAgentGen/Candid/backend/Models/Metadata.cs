using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.backend.Models;

namespace Cosmicrafts.backend.Models
{
	public class Metadata
	{
		[CandidName("basic")]
		public OptionalValue<BasicMetadata> Basic { get; set; }

		[CandidName("category")]
		public Category Category { get; set; }

		[CandidName("general")]
		public GeneralMetadata General { get; set; }

		[CandidName("skills")]
		public OptionalValue<SkillMetadata> Skills { get; set; }

		[CandidName("skins")]
		public OptionalValue<SkinMetadata> Skins { get; set; }

		[CandidName("soul")]
		public OptionalValue<SoulMetadata> Soul { get; set; }

		public Metadata(OptionalValue<BasicMetadata> basic, Category category, GeneralMetadata general, OptionalValue<SkillMetadata> skills, OptionalValue<SkinMetadata> skins, OptionalValue<SoulMetadata> soul)
		{
			this.Basic = basic;
			this.Category = category;
			this.General = general;
			this.Skills = skills;
			this.Skins = skins;
			this.Soul = soul;
		}

		public Metadata()
		{
		}
	}
}