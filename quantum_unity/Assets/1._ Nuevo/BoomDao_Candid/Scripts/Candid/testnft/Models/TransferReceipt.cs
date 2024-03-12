using EdjCase.ICP.Candid.Mapping;
using CanisterPK.testnft.Models;
using System;
using TransferId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.testnft.Models
{
	[Variant]
	public class TransferReceipt
	{
		[VariantTagProperty]
		public TransferReceiptTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public TransferReceipt(TransferReceiptTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected TransferReceipt()
		{
		}

		public static TransferReceipt Err(TransferError info)
		{
			return new TransferReceipt(TransferReceiptTag.Err, info);
		}

		public static TransferReceipt Ok(TransferId info)
		{
			return new TransferReceipt(TransferReceiptTag.Ok, info);
		}

		public TransferError AsErr()
		{
			this.ValidateTag(TransferReceiptTag.Err);
			return (TransferError)this.Value!;
		}

		public TransferId AsOk()
		{
			this.ValidateTag(TransferReceiptTag.Ok);
			return (TransferId)this.Value!;
		}

		private void ValidateTag(TransferReceiptTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum TransferReceiptTag
	{
		Err,
		Ok
	}
}