using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Candid.IcrcLedger.Models
{
	public class ArchiveOptions
	{
		[CandidName("trigger_threshold")]
		public ulong TriggerThreshold { get; set; }

		[CandidName("num_blocks_to_archive")]
		public ulong NumBlocksToArchive { get; set; }

		[CandidName("node_max_memory_size_bytes")]
		public OptionalValue<ulong> NodeMaxMemorySizeBytes { get; set; }

		[CandidName("max_message_size_bytes")]
		public OptionalValue<ulong> MaxMessageSizeBytes { get; set; }

		[CandidName("controller_id")]
		public Principal ControllerId { get; set; }

		[CandidName("cycles_for_archive_creation")]
		public OptionalValue<ulong> CyclesForArchiveCreation { get; set; }

		public ArchiveOptions(ulong triggerThreshold, ulong numBlocksToArchive, OptionalValue<ulong> nodeMaxMemorySizeBytes, OptionalValue<ulong> maxMessageSizeBytes, Principal controllerId, OptionalValue<ulong> cyclesForArchiveCreation)
		{
			this.TriggerThreshold = triggerThreshold;
			this.NumBlocksToArchive = numBlocksToArchive;
			this.NodeMaxMemorySizeBytes = nodeMaxMemorySizeBytes;
			this.MaxMessageSizeBytes = maxMessageSizeBytes;
			this.ControllerId = controllerId;
			this.CyclesForArchiveCreation = cyclesForArchiveCreation;
		}

		public ArchiveOptions()
		{
		}
	}
}