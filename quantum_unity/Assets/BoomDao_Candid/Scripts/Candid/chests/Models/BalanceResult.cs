using EdjCase.ICP.Candid.Mapping;
using CanisterPK.chests.Models;
using EdjCase.ICP.Candid.Models;
using System;

namespace CanisterPK.chests.Models
{
	[Variant]
	public class BalanceResult
	{
		[VariantTagProperty]
		public BalanceResultTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public BalanceResult(BalanceResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected BalanceResult()
		{
		}

		public static BalanceResult Err(CallError info)
		{
			return new BalanceResult(BalanceResultTag.Err, info);
		}

		public static BalanceResult Ok(UnboundedUInt info)
		{
			return new BalanceResult(BalanceResultTag.Ok, info);
		}

		public CallError AsErr()
		{
			this.ValidateTag(BalanceResultTag.Err);
			return (CallError)this.Value!;
		}

		public UnboundedUInt AsOk()
		{
			this.ValidateTag(BalanceResultTag.Ok);
			return (UnboundedUInt)this.Value!;
		}

		private void ValidateTag(BalanceResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum BalanceResultTag
	{
		Err,
		Ok
	}
}