using EdjCase.ICP.Candid.Mapping;
using CanisterPK.chests.Models;
using System.Collections.Generic;
using System;

namespace CanisterPK.chests.Models
{
	[Variant]
	public class MetadataResult
	{
		[VariantTagProperty]
		public MetadataResultTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public MetadataResult(MetadataResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected MetadataResult()
		{
		}

		public static MetadataResult Err(CallError info)
		{
			return new MetadataResult(MetadataResultTag.Err, info);
		}

		public static MetadataResult Ok(Dictionary<string, Metadata> info)
		{
			return new MetadataResult(MetadataResultTag.Ok, info);
		}

		public CallError AsErr()
		{
			this.ValidateTag(MetadataResultTag.Err);
			return (CallError)this.Value!;
		}

		public Dictionary<string, Metadata> AsOk()
		{
			this.ValidateTag(MetadataResultTag.Ok);
			return (Dictionary<string, Metadata>)this.Value!;
		}

		private void ValidateTag(MetadataResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum MetadataResultTag
	{
		Err,
		Ok
	}
}