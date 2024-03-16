using EdjCase.ICP.Candid.Mapping;
using CanisterPK.chests.Models;
using System;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.chests.Models
{
	[Variant]
	public class MintReceipt
	{
		[VariantTagProperty]
		public MintReceiptTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public MintReceipt(MintReceiptTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected MintReceipt()
		{
		}

		public static MintReceipt Err(MintError info)
		{
			return new MintReceipt(MintReceiptTag.Err, info);
		}

		public static MintReceipt Ok(TokenId info)
		{
			return new MintReceipt(MintReceiptTag.Ok, info);
		}

		public MintError AsErr()
		{
			this.ValidateTag(MintReceiptTag.Err);
			return (MintError)this.Value!;
		}

		public TokenId AsOk()
		{
			this.ValidateTag(MintReceiptTag.Ok);
			return (TokenId)this.Value!;
		}

		private void ValidateTag(MintReceiptTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum MintReceiptTag
	{
		Err,
		Ok
	}
}