using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.backend.Models;

namespace Cosmicrafts.backend.Models
{
	public class GetInformationRequest
	{
		[CandidName("logs")]
		public OptionalValue<CanisterLogRequest> Logs { get; set; }

		[CandidName("metrics")]
		public OptionalValue<MetricsRequest> Metrics { get; set; }

		[CandidName("status")]
		public OptionalValue<StatusRequest> Status { get; set; }

		[CandidName("version")]
		public bool Version { get; set; }

		public GetInformationRequest(OptionalValue<CanisterLogRequest> logs, OptionalValue<MetricsRequest> metrics, OptionalValue<StatusRequest> status, bool version)
		{
			this.Logs = logs;
			this.Metrics = metrics;
			this.Status = status;
			this.Version = version;
		}

		public GetInformationRequest()
		{
		}
	}
}