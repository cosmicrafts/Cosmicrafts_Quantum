using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.MainCanister.Models;
using EdjCase.ICP.Candid.Models;

namespace Cosmicrafts.MainCanister.Models
{
	public class SkinMetadata
	{
		[CandidName("general")]
		public GeneralMetadata General { get; set; }

		[CandidName("soul")]
		public OptionalValue<SoulMetadata> Soul { get; set; }

		public SkinMetadata(GeneralMetadata general, OptionalValue<SoulMetadata> soul)
		{
			this.General = general;
			this.Soul = soul;
		}

		public SkinMetadata()
		{
		}
	}
}