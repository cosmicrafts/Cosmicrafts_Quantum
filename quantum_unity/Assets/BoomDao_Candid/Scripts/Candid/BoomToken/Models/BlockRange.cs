using EdjCase.ICP.Candid.Mapping;
using CanisterPK.BoomToken.Models;
using System.Collections.Generic;
using Block = CanisterPK.BoomToken.Models.Value;

namespace CanisterPK.BoomToken.Models
{
	public class BlockRange
	{
		[CandidName("blocks")]
		public BlockRange.BlocksInfo Blocks { get; set; }

		public BlockRange(BlockRange.BlocksInfo blocks)
		{
			this.Blocks = blocks;
		}

		public BlockRange()
		{
		}

		public class BlocksInfo : List<Block>
		{
			public BlocksInfo()
			{
			}
		}
	}
}