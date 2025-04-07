using EdjCase.ICP.Candid.Mapping;

namespace Cosmicrafts.backend.Models
{
	public enum CanisterLogFeature
	{
		[CandidName("filterMessageByContains")]
		FilterMessageByContains,
		[CandidName("filterMessageByRegex")]
		FilterMessageByRegex
	}
}