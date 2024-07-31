using EdjCase.ICP.Candid.Mapping;
using CanisterPK.chests.Models;
using System;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.chests.Models
{
	[Variant]
	public class Result
	{
		[VariantTagProperty]
		public ResultTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Result(ResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result()
		{
		}

		public static Result Err(UpdateError info)
		{
			return new Result(ResultTag.Err, info);
		}

		public static Result Ok(TokenId info)
		{
			return new Result(ResultTag.Ok, info);
		}

		public UpdateError AsErr()
		{
			this.ValidateTag(ResultTag.Err);
			return (UpdateError)this.Value!;
		}

		public TokenId AsOk()
		{
			this.ValidateTag(ResultTag.Ok);
			return (TokenId)this.Value!;
		}

		private void ValidateTag(ResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum ResultTag
	{
		Err,
		Ok
	}
}