using EdjCase.ICP.Candid.Mapping;
using CanisterPK.testnft.Models;
using System;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.testnft.Models
{
	[Variant]
	public class UpgradeReceipt
	{
		[VariantTagProperty]
		public UpgradeReceiptTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public UpgradeReceipt(UpgradeReceiptTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected UpgradeReceipt()
		{
		}

		public static UpgradeReceipt Err(UpgradeError info)
		{
			return new UpgradeReceipt(UpgradeReceiptTag.Err, info);
		}

		public static UpgradeReceipt Ok(TokenId info)
		{
			return new UpgradeReceipt(UpgradeReceiptTag.Ok, info);
		}

		public UpgradeError AsErr()
		{
			this.ValidateTag(UpgradeReceiptTag.Err);
			return (UpgradeError)this.Value!;
		}

		public TokenId AsOk()
		{
			this.ValidateTag(UpgradeReceiptTag.Ok);
			return (TokenId)this.Value!;
		}

		private void ValidateTag(UpgradeReceiptTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum UpgradeReceiptTag
	{
		Err,
		Ok
	}
}