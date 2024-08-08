using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.MainCanister.Models;
using EdjCase.ICP.Candid.Models;
using System;

namespace Cosmicrafts.MainCanister.Models
{
	[Variant]
	public class SkillMetadata
	{
		[VariantTagProperty]
		public SkillMetadataTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public SkillMetadata(SkillMetadataTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected SkillMetadata()
		{
		}

		public static SkillMetadata CriticalStrike(OptionalValue<CriticalStrikeMetadata> info)
		{
			return new SkillMetadata(SkillMetadataTag.CriticalStrike, info);
		}

		public static SkillMetadata Evasion(OptionalValue<EvasionMetadata> info)
		{
			return new SkillMetadata(SkillMetadataTag.Evasion, info);
		}

		public static SkillMetadata Shield(OptionalValue<ShieldMetadata> info)
		{
			return new SkillMetadata(SkillMetadataTag.Shield, info);
		}

		public OptionalValue<CriticalStrikeMetadata> AsCriticalStrike()
		{
			this.ValidateTag(SkillMetadataTag.CriticalStrike);
			return (OptionalValue<CriticalStrikeMetadata>)this.Value!;
		}

		public OptionalValue<EvasionMetadata> AsEvasion()
		{
			this.ValidateTag(SkillMetadataTag.Evasion);
			return (OptionalValue<EvasionMetadata>)this.Value!;
		}

		public OptionalValue<ShieldMetadata> AsShield()
		{
			this.ValidateTag(SkillMetadataTag.Shield);
			return (OptionalValue<ShieldMetadata>)this.Value!;
		}

		private void ValidateTag(SkillMetadataTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum SkillMetadataTag
	{
		[CandidName("criticalStrike")]
		CriticalStrike,
		[CandidName("evasion")]
		Evasion,
		[CandidName("shield")]
		Shield
	}
}