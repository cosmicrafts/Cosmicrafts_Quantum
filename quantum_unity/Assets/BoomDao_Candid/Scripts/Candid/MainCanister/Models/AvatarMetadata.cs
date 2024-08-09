using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.MainCanister.Models;

namespace Cosmicrafts.MainCanister.Models
{
	public class AvatarMetadata
	{
		[CandidName("general")]
		public GeneralMetadata General { get; set; }

		public AvatarMetadata(GeneralMetadata general)
		{
			this.General = general;
		}

		public AvatarMetadata()
		{
		}
	}
}