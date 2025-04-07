using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.backend.Models;
using System.Collections.Generic;
using System;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.backend.Models
{
	[Variant]
	public class TokensOfResult
	{
		[VariantTagProperty]
		public TokensOfResultTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public TokensOfResult(TokensOfResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected TokensOfResult()
		{
		}

		public static TokensOfResult Err(CallError info)
		{
			return new TokensOfResult(TokensOfResultTag.Err, info);
		}

		public static TokensOfResult Ok(List<TokenId> info)
		{
			return new TokensOfResult(TokensOfResultTag.Ok, info);
		}

		public CallError AsErr()
		{
			this.ValidateTag(TokensOfResultTag.Err);
			return (CallError)this.Value!;
		}

		public List<TokenId> AsOk()
		{
			this.ValidateTag(TokensOfResultTag.Ok);
			return (List<TokenId>)this.Value!;
		}

		private void ValidateTag(TokensOfResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum TokensOfResultTag
	{
		Err,
		Ok
	}
}