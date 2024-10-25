using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using Candid.IcrcLedger.Models;
using BlockIndex = System.UInt64;

namespace Candid.IcrcLedger.Models
{
	public class QueryBlocksResponse
	{
		[CandidName("chain_length")]
		public ulong ChainLength { get; set; }

		[CandidName("certificate")]
		public OptionalValue<List<byte>> Certificate { get; set; }

		[CandidName("blocks")]
		public List<Block> Blocks { get; set; }

		[CandidName("first_block_index")]
		public BlockIndex FirstBlockIndex { get; set; }

		[CandidName("archived_blocks")]
		public List<ArchivedBlocksRange> ArchivedBlocks { get; set; }

		public QueryBlocksResponse(ulong chainLength, OptionalValue<List<byte>> certificate, List<Block> blocks, BlockIndex firstBlockIndex, List<ArchivedBlocksRange> archivedBlocks)
		{
			this.ChainLength = chainLength;
			this.Certificate = certificate;
			this.Blocks = blocks;
			this.FirstBlockIndex = firstBlockIndex;
			this.ArchivedBlocks = archivedBlocks;
		}

		public QueryBlocksResponse()
		{
		}
	}
}