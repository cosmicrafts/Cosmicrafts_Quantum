using EdjCase.ICP.Candid.Mapping;
using CanisterPK.UserGeekICHubCanister.Models;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using ChunkId = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Key = System.String;

namespace CanisterPK.UserGeekICHubCanister.Models
{
	public class SetAssetContentArguments
	{
		[CandidName("key")]
		public Key Key { get; set; }

		[CandidName("content_encoding")]
		public string ContentEncoding { get; set; }

		[CandidName("chunk_ids")]
		public SetAssetContentArguments.ChunkIdsInfo ChunkIds { get; set; }

		[CandidName("sha256")]
		public OptionalValue<List<byte>> Sha256 { get; set; }

		public SetAssetContentArguments(Key key, string contentEncoding, SetAssetContentArguments.ChunkIdsInfo chunkIds, OptionalValue<List<byte>> sha256)
		{
			this.Key = key;
			this.ContentEncoding = contentEncoding;
			this.ChunkIds = chunkIds;
			this.Sha256 = sha256;
		}

		public SetAssetContentArguments()
		{
		}

		public class ChunkIdsInfo : List<ChunkId>
		{
			public ChunkIdsInfo()
			{
			}
		}
	}
}