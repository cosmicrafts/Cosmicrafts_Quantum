using EdjCase.ICP.Candid.Mapping;
using CanisterPK.testnft.Models;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;
using System;
using MetadataArray = System.Collections.Generic.Dictionary<System.String, CanisterPK.testnft.Models.Metadata>;

namespace CanisterPK.testnft.Models
{
	[Variant]
	public class Metadata
	{
		[VariantTagProperty]
		public MetadataTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Metadata(MetadataTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Metadata()
		{
		}

		public static Metadata Blob(List<byte> info)
		{
			return new Metadata(MetadataTag.Blob, info);
		}

		public static Metadata Int(UnboundedInt info)
		{
			return new Metadata(MetadataTag.Int, info);
		}

		public static Metadata MetadataArray(MetadataArray info)
		{
			return new Metadata(MetadataTag.MetadataArray, info);
		}

		public static Metadata Nat(UnboundedUInt info)
		{
			return new Metadata(MetadataTag.Nat, info);
		}

		public static Metadata Text(string info)
		{
			return new Metadata(MetadataTag.Text, info);
		}

		public List<byte> AsBlob()
		{
			this.ValidateTag(MetadataTag.Blob);
			return (List<byte>)this.Value!;
		}

		public UnboundedInt AsInt()
		{
			this.ValidateTag(MetadataTag.Int);
			return (UnboundedInt)this.Value!;
		}

		public MetadataArray AsMetadataArray()
		{
			this.ValidateTag(MetadataTag.MetadataArray);
			return (MetadataArray)this.Value!;
		}

		public UnboundedUInt AsNat()
		{
			this.ValidateTag(MetadataTag.Nat);
			return (UnboundedUInt)this.Value!;
		}

		public string AsText()
		{
			this.ValidateTag(MetadataTag.Text);
			return (string)this.Value!;
		}

		private void ValidateTag(MetadataTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum MetadataTag
	{
		Blob,
		Int,
		MetadataArray,
		Nat,
		Text
	}
}