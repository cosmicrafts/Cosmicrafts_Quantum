using EdjCase.ICP.Candid.Mapping;
using Candid.IcrcLedger.Models;
using System;
using BlockIndex = System.UInt64;

namespace Candid.IcrcLedger.Models
{
	[Variant]
	public class QueryArchiveError
	{
		[VariantTagProperty]
		public QueryArchiveErrorTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public QueryArchiveError(QueryArchiveErrorTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected QueryArchiveError()
		{
		}

		public static QueryArchiveError BadFirstBlockIndex(QueryArchiveError.BadFirstBlockIndexInfo info)
		{
			return new QueryArchiveError(QueryArchiveErrorTag.BadFirstBlockIndex, info);
		}

		public static QueryArchiveError Other(QueryArchiveError.OtherInfo info)
		{
			return new QueryArchiveError(QueryArchiveErrorTag.Other, info);
		}

		public QueryArchiveError.BadFirstBlockIndexInfo AsBadFirstBlockIndex()
		{
			this.ValidateTag(QueryArchiveErrorTag.BadFirstBlockIndex);
			return (QueryArchiveError.BadFirstBlockIndexInfo)this.Value!;
		}

		public QueryArchiveError.OtherInfo AsOther()
		{
			this.ValidateTag(QueryArchiveErrorTag.Other);
			return (QueryArchiveError.OtherInfo)this.Value!;
		}

		private void ValidateTag(QueryArchiveErrorTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class BadFirstBlockIndexInfo
		{
			[CandidName("requested_index")]
			public BlockIndex RequestedIndex { get; set; }

			[CandidName("first_valid_index")]
			public BlockIndex FirstValidIndex { get; set; }

			public BadFirstBlockIndexInfo(BlockIndex requestedIndex, BlockIndex firstValidIndex)
			{
				this.RequestedIndex = requestedIndex;
				this.FirstValidIndex = firstValidIndex;
			}

			public BadFirstBlockIndexInfo()
			{
			}
		}

		public class OtherInfo
		{
			[CandidName("error_code")]
			public ulong ErrorCode { get; set; }

			[CandidName("error_message")]
			public string ErrorMessage { get; set; }

			public OtherInfo(ulong errorCode, string errorMessage)
			{
				this.ErrorCode = errorCode;
				this.ErrorMessage = errorMessage;
			}

			public OtherInfo()
			{
			}
		}
	}

	public enum QueryArchiveErrorTag
	{
		BadFirstBlockIndex,
		Other
	}
}