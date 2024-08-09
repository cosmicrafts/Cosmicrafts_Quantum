using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.MainCanister.Models;

namespace Cosmicrafts.MainCanister.Models
{
	public class TrophyMetadata
	{
		[CandidName("general")]
		public GeneralMetadata General { get; set; }

		public TrophyMetadata(GeneralMetadata general)
		{
			this.General = general;
		}

		public TrophyMetadata()
		{
		}
	}
}