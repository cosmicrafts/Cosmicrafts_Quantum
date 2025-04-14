using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.backend.Models;

namespace Cosmicrafts.backend.Models
{
	public class GetInformationResponse
	{
		[CandidName("logs")]
		public OptionalValue<CanisterLogResponse> Logs { get; set; }

		[CandidName("metrics")]
		public OptionalValue<MetricsResponse> Metrics { get; set; }

		[CandidName("status")]
		public OptionalValue<StatusResponse> Status { get; set; }

		[CandidName("version")]
		public OptionalValue<UnboundedUInt> Version { get; set; }

		public GetInformationResponse(OptionalValue<CanisterLogResponse> logs, OptionalValue<MetricsResponse> metrics, OptionalValue<StatusResponse> status, OptionalValue<UnboundedUInt> version)
		{
			this.Logs = logs;
			this.Metrics = metrics;
			this.Status = status;
			this.Version = version;
		}

		public GetInformationResponse()
		{
		}
	}
}