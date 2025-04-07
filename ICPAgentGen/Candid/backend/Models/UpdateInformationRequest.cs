using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.backend.Models;

namespace Cosmicrafts.backend.Models
{
	public class UpdateInformationRequest
	{
		[CandidName("metrics")]
		public OptionalValue<CollectMetricsRequestType> Metrics { get; set; }

		public UpdateInformationRequest(OptionalValue<CollectMetricsRequestType> metrics)
		{
			this.Metrics = metrics;
		}

		public UpdateInformationRequest()
		{
		}
	}
}