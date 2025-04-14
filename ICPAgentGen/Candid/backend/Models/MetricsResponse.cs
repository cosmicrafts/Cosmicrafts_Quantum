using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.backend.Models;

namespace Cosmicrafts.backend.Models
{
	public class MetricsResponse
	{
		[CandidName("metrics")]
		public OptionalValue<CanisterMetrics> Metrics { get; set; }

		public MetricsResponse(OptionalValue<CanisterMetrics> metrics)
		{
			this.Metrics = metrics;
		}

		public MetricsResponse()
		{
		}
	}
}