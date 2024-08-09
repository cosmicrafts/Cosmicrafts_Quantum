using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.MainCanister.Models;

namespace Cosmicrafts.MainCanister.Models
{
	public class ChestMetadata
	{
		[CandidName("general")]
		public GeneralMetadata General { get; set; }

		public ChestMetadata(GeneralMetadata general)
		{
			this.General = general;
		}

		public ChestMetadata()
		{
		}
	}
}