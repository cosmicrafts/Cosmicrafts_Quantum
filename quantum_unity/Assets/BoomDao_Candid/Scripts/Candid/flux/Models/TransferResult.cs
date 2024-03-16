using EdjCase.ICP.Candid.Mapping;
using CanisterPK.flux.Models;
using System;
using TxIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.flux.Models
{
	[Variant]
	public class TransferResult
	{
		[VariantTagProperty]
		public TransferResultTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public TransferResult(TransferResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected TransferResult()
		{
		}

		public static TransferResult Err(TransferError info)
		{
			return new TransferResult(TransferResultTag.Err, info);
		}

		public static TransferResult Ok(TxIndex info)
		{
			return new TransferResult(TransferResultTag.Ok, info);
		}

		public TransferError AsErr()
		{
			this.ValidateTag(TransferResultTag.Err);
			return (TransferError)this.Value!;
		}

		public TxIndex AsOk()
		{
			this.ValidateTag(TransferResultTag.Ok);
			return (TxIndex)this.Value!;
		}

		private void ValidateTag(TransferResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum TransferResultTag
	{
		Err,
		Ok
	}
}