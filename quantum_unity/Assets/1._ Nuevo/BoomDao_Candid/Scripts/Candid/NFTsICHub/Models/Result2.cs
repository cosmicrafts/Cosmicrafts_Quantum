using EdjCase.ICP.Candid.Mapping;
using CanisterPK.NFTsICHub.Models;
using System;
using TokenIndex = System.UInt32;

namespace CanisterPK.NFTsICHub.Models
{
	[Variant]
	public class Result2
	{
		[VariantTagProperty]
		public Result2Tag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Result2(Result2Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result2()
		{
		}

		public static Result2 Err(CommonError info)
		{
			return new Result2(Result2Tag.Err, info);
		}

		public static Result2 Ok(TokenIndex info)
		{
			return new Result2(Result2Tag.Ok, info);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(Result2Tag.Err);
			return (CommonError)this.Value!;
		}

		public TokenIndex AsOk()
		{
			this.ValidateTag(Result2Tag.Ok);
			return (TokenIndex)this.Value!;
		}

		private void ValidateTag(Result2Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result2Tag
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}