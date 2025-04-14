using EdjCase.ICP.Candid.Mapping;

namespace Cosmicrafts.backend.Models
{
	public enum MetricsGranularity
	{
		[CandidName("daily")]
		Daily,
		[CandidName("hourly")]
		Hourly
	}
}