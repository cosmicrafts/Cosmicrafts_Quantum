using EdjCase.ICP.Candid.Mapping;
using Candid.IcrcLedger.Models;
using EdjCase.ICP.Candid.Models;
using System;

namespace Candid.IcrcLedger.Models
{
	[Variant]
	public class LedgerCanisterPayload
	{
		[VariantTagProperty]
		public LedgerCanisterPayloadTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public LedgerCanisterPayload(LedgerCanisterPayloadTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected LedgerCanisterPayload()
		{
		}

		public static LedgerCanisterPayload Init(InitArgs info)
		{
			return new LedgerCanisterPayload(LedgerCanisterPayloadTag.Init, info);
		}

		public static LedgerCanisterPayload Upgrade(OptionalValue<UpgradeArgs> info)
		{
			return new LedgerCanisterPayload(LedgerCanisterPayloadTag.Upgrade, info);
		}

		public InitArgs AsInit()
		{
			this.ValidateTag(LedgerCanisterPayloadTag.Init);
			return (InitArgs)this.Value!;
		}

		public OptionalValue<UpgradeArgs> AsUpgrade()
		{
			this.ValidateTag(LedgerCanisterPayloadTag.Upgrade);
			return (OptionalValue<UpgradeArgs>)this.Value!;
		}

		private void ValidateTag(LedgerCanisterPayloadTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum LedgerCanisterPayloadTag
	{
		Init,
		Upgrade
	}
}