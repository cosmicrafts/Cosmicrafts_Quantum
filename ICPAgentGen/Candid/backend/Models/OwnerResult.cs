using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.backend.Models;
using System;

namespace Cosmicrafts.backend.Models
{
	[Variant]
	public class OwnerResult
	{
		[VariantTagProperty]
		public OwnerResultTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public OwnerResult(OwnerResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected OwnerResult()
		{
		}

		public static OwnerResult Err(CallError info)
		{
			return new OwnerResult(OwnerResultTag.Err, info);
		}

		public static OwnerResult Ok(Account info)
		{
			return new OwnerResult(OwnerResultTag.Ok, info);
		}

		public CallError AsErr()
		{
			this.ValidateTag(OwnerResultTag.Err);
			return (CallError)this.Value!;
		}

		public Account AsOk()
		{
			this.ValidateTag(OwnerResultTag.Ok);
			return (Account)this.Value!;
		}

		private void ValidateTag(OwnerResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum OwnerResultTag
	{
		Err,
		Ok
	}
}