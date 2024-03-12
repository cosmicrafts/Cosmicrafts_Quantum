using EdjCase.ICP.Candid.Mapping;
using BlockIndex = System.UInt64;

namespace Candid.IcrcLedger.Models
{
	public class GetBlocksArgs
	{
		[CandidName("start")]
		public BlockIndex Start { get; set; }

		[CandidName("length")]
		public ulong Length { get; set; }

		public GetBlocksArgs(BlockIndex start, ulong length)
		{
			this.Start = start;
			this.Length = length;
		}

		public GetBlocksArgs()
		{
		}
	}
}