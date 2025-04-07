using EdjCase.ICP.Candid.Mapping;

namespace Cosmicrafts.backend.Models
{
	public class StatusRequest
	{
		[CandidName("cycles")]
		public bool Cycles { get; set; }

		[CandidName("heap_memory_size")]
		public bool HeapMemorySize { get; set; }

		[CandidName("memory_size")]
		public bool MemorySize { get; set; }

		public StatusRequest(bool cycles, bool heapMemorySize, bool memorySize)
		{
			this.Cycles = cycles;
			this.HeapMemorySize = heapMemorySize;
			this.MemorySize = memorySize;
		}

		public StatusRequest()
		{
		}
	}
}