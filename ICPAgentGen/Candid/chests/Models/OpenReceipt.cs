using EdjCase.ICP.Candid.Mapping;
using CanisterPK.chests.Models;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;
using System;

namespace CanisterPK.chests.Models
{
	[Variant]
	public class OpenReceipt
	{
		[VariantTagProperty]
		public OpenReceiptTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public OpenReceipt(OpenReceiptTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected OpenReceipt()
		{
		}

		public static OpenReceipt Err(TransferError info)
		{
			return new OpenReceipt(OpenReceiptTag.Err, info);
		}

		public static OpenReceipt Ok(Dictionary<string, UnboundedUInt> info)
		{
			return new OpenReceipt(OpenReceiptTag.Ok, info);
		}

		public TransferError AsErr()
		{
			this.ValidateTag(OpenReceiptTag.Err);
			return (TransferError)this.Value!;
		}

		public Dictionary<string, UnboundedUInt> AsOk()
		{
			this.ValidateTag(OpenReceiptTag.Ok);
			return (Dictionary<string, UnboundedUInt>)this.Value!;
		}

		private void ValidateTag(OpenReceiptTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum OpenReceiptTag
	{
		Err,
		Ok
	}
}