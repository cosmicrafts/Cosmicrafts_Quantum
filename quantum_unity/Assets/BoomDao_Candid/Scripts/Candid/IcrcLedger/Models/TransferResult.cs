using EdjCase.ICP.Candid.Mapping;
using Candid.IcrcLedger.Models;
using System;
using BlockIndex = System.UInt64;

namespace Candid.IcrcLedger.Models
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

		public static TransferResult Ok(BlockIndex info)
		{
			return new TransferResult(TransferResultTag.Ok, info);
		}

		public static TransferResult Err(TransferError info)
		{
			return new TransferResult(TransferResultTag.Err, info);
		}

		public BlockIndex AsOk()
		{
			this.ValidateTag(TransferResultTag.Ok);
			return (BlockIndex)this.Value!;
		}

		public TransferError AsErr()
		{
			this.ValidateTag(TransferResultTag.Err);
			return (TransferError)this.Value!;
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
		Ok,
		Err
	}
}