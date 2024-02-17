using EdjCase.ICP.Candid.Mapping;
using CanisterPK.NFTsICHub.Models;
using System;
using Accountidentifier1 = System.String;

namespace CanisterPK.NFTsICHub.Models
{
	[Variant]
	public class Result3
	{
		[VariantTagProperty]
		public Result3Tag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Result3(Result3Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result3()
		{
		}

		public static Result3 Err(CommonError info)
		{
			return new Result3(Result3Tag.Err, info);
		}

		public static Result3 Ok(Accountidentifier1 info)
		{
			return new Result3(Result3Tag.Ok, info);
		}

		public CommonError AsErr()
		{
			this.ValidateTag(Result3Tag.Err);
			return (CommonError)this.Value!;
		}

		public Accountidentifier1 AsOk()
		{
			this.ValidateTag(Result3Tag.Ok);
			return (Accountidentifier1)this.Value!;
		}

		private void ValidateTag(Result3Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result3Tag
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}