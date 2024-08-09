using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.MainCanister.Models;

namespace Cosmicrafts.MainCanister.Models
{
	public class SkinMetadata
	{
		[CandidName("general")]
		public GeneralMetadata General { get; set; }

		public SkinMetadata(GeneralMetadata general)
		{
			this.General = general;
		}

		public SkinMetadata()
		{
		}
	}
}