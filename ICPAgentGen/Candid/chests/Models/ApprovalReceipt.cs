using EdjCase.ICP.Candid.Mapping;
using CanisterPK.chests.Models;
using System;
using ApprovalId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.chests.Models
{
	[Variant]
	public class ApprovalReceipt
	{
		[VariantTagProperty]
		public ApprovalReceiptTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public ApprovalReceipt(ApprovalReceiptTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected ApprovalReceipt()
		{
		}

		public static ApprovalReceipt Err(ApprovalError info)
		{
			return new ApprovalReceipt(ApprovalReceiptTag.Err, info);
		}

		public static ApprovalReceipt Ok(ApprovalId info)
		{
			return new ApprovalReceipt(ApprovalReceiptTag.Ok, info);
		}

		public ApprovalError AsErr()
		{
			this.ValidateTag(ApprovalReceiptTag.Err);
			return (ApprovalError)this.Value!;
		}

		public ApprovalId AsOk()
		{
			this.ValidateTag(ApprovalReceiptTag.Ok);
			return (ApprovalId)this.Value!;
		}

		private void ValidateTag(ApprovalReceiptTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum ApprovalReceiptTag
	{
		Err,
		Ok
	}
}