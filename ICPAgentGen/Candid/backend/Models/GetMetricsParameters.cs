using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.backend.Models;

namespace Cosmicrafts.backend.Models
{
	public class GetMetricsParameters
	{
		[CandidName("dateFromMillis")]
		public UnboundedUInt DateFromMillis { get; set; }

		[CandidName("dateToMillis")]
		public UnboundedUInt DateToMillis { get; set; }

		[CandidName("granularity")]
		public MetricsGranularity Granularity { get; set; }

		public GetMetricsParameters(UnboundedUInt dateFromMillis, UnboundedUInt dateToMillis, MetricsGranularity granularity)
		{
			this.DateFromMillis = dateFromMillis;
			this.DateToMillis = dateToMillis;
			this.Granularity = granularity;
		}

		public GetMetricsParameters()
		{
		}
	}
}