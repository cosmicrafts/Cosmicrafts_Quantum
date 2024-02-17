using EdjCase.ICP.Candid.Mapping;
using CanisterPK.NFTsICHub.Models;
using System;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;

namespace CanisterPK.NFTsICHub.Models
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

		public static Metadata Fungible(Metadata.FungibleInfo info)
		{
			return new Metadata(MetadataTag.Fungible, info);
		}

		public static Metadata Nonfungible(Metadata.NonfungibleInfo info)
		{
			return new Metadata(MetadataTag.Nonfungible, info);
		}

		public Metadata.FungibleInfo AsFungible()
		{
			this.ValidateTag(MetadataTag.Fungible);
			return (Metadata.FungibleInfo)this.Value!;
		}

		public Metadata.NonfungibleInfo AsNonfungible()
		{
			this.ValidateTag(MetadataTag.Nonfungible);
			return (Metadata.NonfungibleInfo)this.Value!;
		}

		private void ValidateTag(MetadataTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class FungibleInfo
		{
			[CandidName("decimals")]
			public byte Decimals { get; set; }

			[CandidName("metadata")]
			public OptionalValue<List<byte>> Metadata { get; set; }

			[CandidName("name")]
			public string Name { get; set; }

			[CandidName("symbol")]
			public string Symbol { get; set; }

			public FungibleInfo(byte decimals, OptionalValue<List<byte>> metadata, string name, string symbol)
			{
				this.Decimals = decimals;
				this.Metadata = metadata;
				this.Name = name;
				this.Symbol = symbol;
			}

			public FungibleInfo()
			{
			}
		}

		public class NonfungibleInfo
		{
			[CandidName("metadata")]
			public OptionalValue<List<byte>> Metadata { get; set; }

			public NonfungibleInfo(OptionalValue<List<byte>> metadata)
			{
				this.Metadata = metadata;
			}

			public NonfungibleInfo()
			{
			}
		}
	}

	public enum MetadataTag
	{
		[CandidName("fungible")]
		Fungible,
		[CandidName("nonfungible")]
		Nonfungible
	}
}