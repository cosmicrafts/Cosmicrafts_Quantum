using EdjCase.ICP.Candid.Mapping;
using Candid.IcrcLedger.Models;
using System;

namespace Candid.IcrcLedger.Models
{
	[Variant]
	public class QueryArchiveResult
	{
		[VariantTagProperty]
		public QueryArchiveResultTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public QueryArchiveResult(QueryArchiveResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected QueryArchiveResult()
		{
		}

		public static QueryArchiveResult Ok(BlockRange info)
		{
			return new QueryArchiveResult(QueryArchiveResultTag.Ok, info);
		}

		public static QueryArchiveResult Err(QueryArchiveError info)
		{
			return new QueryArchiveResult(QueryArchiveResultTag.Err, info);
		}

		public BlockRange AsOk()
		{
			this.ValidateTag(QueryArchiveResultTag.Ok);
			return (BlockRange)this.Value!;
		}

		public QueryArchiveError AsErr()
		{
			this.ValidateTag(QueryArchiveResultTag.Err);
			return (QueryArchiveError)this.Value!;
		}

		private void ValidateTag(QueryArchiveResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum QueryArchiveResultTag
	{
		Ok,
		Err
	}
}