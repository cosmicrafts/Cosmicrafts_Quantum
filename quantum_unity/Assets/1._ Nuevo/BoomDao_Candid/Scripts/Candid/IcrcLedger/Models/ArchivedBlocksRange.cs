using EdjCase.ICP.Candid.Mapping;
using BlockIndex = System.UInt64;
using QueryArchiveFn = EdjCase.ICP.Candid.Models.Values.CandidFunc;

namespace Candid.IcrcLedger.Models
{
	public class ArchivedBlocksRange
	{
		[CandidName("start")]
		public BlockIndex Start { get; set; }

		[CandidName("length")]
		public ulong Length { get; set; }

		[CandidName("callback")]
		public QueryArchiveFn Callback { get; set; }

		public ArchivedBlocksRange(BlockIndex start, ulong length, QueryArchiveFn callback)
		{
			this.Start = start;
			this.Length = length;
			this.Callback = callback;
		}

		public ArchivedBlocksRange()
		{
		}
	}
}