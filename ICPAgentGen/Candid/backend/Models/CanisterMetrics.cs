using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.backend.Models;

namespace Cosmicrafts.backend.Models
{
	public class CanisterMetrics
	{
		[CandidName("data")]
		public CanisterMetricsData Data { get; set; }

		public CanisterMetrics(CanisterMetricsData data)
		{
			this.Data = data;
		}

		public CanisterMetrics()
		{
		}
	}
}