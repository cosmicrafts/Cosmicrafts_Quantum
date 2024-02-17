using EdjCase.ICP.Candid.Mapping;
using CanisterPK.NFTsICHub.Models;
using System;
using Balance1 = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.NFTsICHub.Models
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

		public static Result Err(CommonError info)
		{
			return new Result(ResultTag.Err, info);
		}

		public static Result Ok(Balance1 info)
		{
			return new Result(ResultTag.Ok, info);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(ResultTag.Err);
			return (CommonError)this.Value!;
		}

		public Balance1 AsOk()
		{
			this.ValidateTag(ResultTag.Ok);
			return (Balance1)this.Value!;
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
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}