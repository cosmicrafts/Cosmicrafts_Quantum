using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using BlockIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.BoomToken.Models
{
	public class ArchiveInfo
	{
		[CandidName("canister_id")]
		public Principal CanisterId { get; set; }

		[CandidName("block_range_start")]
		public BlockIndex BlockRangeStart { get; set; }

		[CandidName("block_range_end")]
		public BlockIndex BlockRangeEnd { get; set; }

		public ArchiveInfo(Principal canisterId, BlockIndex blockRangeStart, BlockIndex blockRangeEnd)
		{
			this.CanisterId = canisterId;
			this.BlockRangeStart = blockRangeStart;
			this.BlockRangeEnd = blockRangeEnd;
		}

		public ArchiveInfo()
		{
		}
	}
}