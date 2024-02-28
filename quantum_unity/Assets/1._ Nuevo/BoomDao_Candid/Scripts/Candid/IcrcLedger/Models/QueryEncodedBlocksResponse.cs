using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using Candid.IcrcLedger.Models;

namespace Candid.IcrcLedger.Models
{
	public class QueryEncodedBlocksResponse
	{
		[CandidName("certificate")]
		public OptionalValue<List<byte>> Certificate { get; set; }

		[CandidName("blocks")]
		public List<List<byte>> Blocks { get; set; }

		[CandidName("chain_length")]
		public ulong ChainLength { get; set; }

		[CandidName("first_block_index")]
		public ulong FirstBlockIndex { get; set; }

		[CandidName("archived_blocks")]
		public List<ArchivedEncodedBlocksRange> ArchivedBlocks { get; set; }

		public QueryEncodedBlocksResponse(OptionalValue<List<byte>> certificate, List<List<byte>> blocks, ulong chainLength, ulong firstBlockIndex, List<ArchivedEncodedBlocksRange> archivedBlocks)
		{
			this.Certificate = certificate;
			this.Blocks = blocks;
			this.ChainLength = chainLength;
			this.FirstBlockIndex = firstBlockIndex;
			this.ArchivedBlocks = archivedBlocks;
		}

		public QueryEncodedBlocksResponse()
		{
		}
	}
}