using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.backend.Models;
using System;

namespace Cosmicrafts.backend.Models
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

		public static MetadataResult Ok(Metadata info)
		{
			return new MetadataResult(MetadataResultTag.Ok, info);
		}

		public CallError AsErr()
		{
			this.ValidateTag(MetadataResultTag.Err);
			return (CallError)this.Value!;
		}

		public Metadata AsOk()
		{
			this.ValidateTag(MetadataResultTag.Ok);
			return (Metadata)this.Value!;
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