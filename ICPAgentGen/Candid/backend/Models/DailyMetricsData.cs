using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.backend.Models;
using EdjCase.ICP.Candid.Models;

namespace Cosmicrafts.backend.Models
{
	public class DailyMetricsData
	{
		[CandidName("canisterCycles")]
		public NumericEntity CanisterCycles { get; set; }

		[CandidName("canisterHeapMemorySize")]
		public NumericEntity CanisterHeapMemorySize { get; set; }

		[CandidName("canisterMemorySize")]
		public NumericEntity CanisterMemorySize { get; set; }

		[CandidName("timeMillis")]
		public UnboundedInt TimeMillis { get; set; }

		[CandidName("updateCalls")]
		public ulong UpdateCalls { get; set; }

		public DailyMetricsData(NumericEntity canisterCycles, NumericEntity canisterHeapMemorySize, NumericEntity canisterMemorySize, UnboundedInt timeMillis, ulong updateCalls)
		{
			this.CanisterCycles = canisterCycles;
			this.CanisterHeapMemorySize = canisterHeapMemorySize;
			this.CanisterMemorySize = canisterMemorySize;
			this.TimeMillis = timeMillis;
			this.UpdateCalls = updateCalls;
		}

		public DailyMetricsData()
		{
		}
	}
}