using EdjCase.ICP.Candid.Mapping;
using Candid.IcrcLedger.Models;
using System;
using Icrc1BlockIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Candid.IcrcLedger.Models
{
	[Variant]
	public class Icrc1TransferResult
	{
		[VariantTagProperty]
		public Icrc1TransferResultTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Icrc1TransferResult(Icrc1TransferResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Icrc1TransferResult()
		{
		}

		public static Icrc1TransferResult Ok(Icrc1BlockIndex info)
		{
			return new Icrc1TransferResult(Icrc1TransferResultTag.Ok, info);
		}

		public static Icrc1TransferResult Err(Icrc1TransferError info)
		{
			return new Icrc1TransferResult(Icrc1TransferResultTag.Err, info);
		}

		public Icrc1BlockIndex AsOk()
		{
			this.ValidateTag(Icrc1TransferResultTag.Ok);
			return (Icrc1BlockIndex)this.Value!;
		}

		public Icrc1TransferError AsErr()
		{
			this.ValidateTag(Icrc1TransferResultTag.Err);
			return (Icrc1TransferError)this.Value!;
		}

		private void ValidateTag(Icrc1TransferResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Icrc1TransferResultTag
	{
		Ok,
		Err
	}
}