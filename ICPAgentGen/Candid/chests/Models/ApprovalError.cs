using EdjCase.ICP.Candid.Mapping;
using CanisterPK.chests.Models;
using System;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.chests.Models
{
	[Variant]
	public class ApprovalError
	{
		[VariantTagProperty]
		public ApprovalErrorTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public ApprovalError(ApprovalErrorTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected ApprovalError()
		{
		}

		public static ApprovalError GenericError(ApprovalError.GenericErrorInfo info)
		{
			return new ApprovalError(ApprovalErrorTag.GenericError, info);
		}

		public static ApprovalError TemporarilyUnavailable(ApprovalError.TemporarilyUnavailableInfo info)
		{
			return new ApprovalError(ApprovalErrorTag.TemporarilyUnavailable, info);
		}

		public static ApprovalError TooOld()
		{
			return new ApprovalError(ApprovalErrorTag.TooOld, null);
		}

		public static ApprovalError Unauthorized(ApprovalError.UnauthorizedInfo info)
		{
			return new ApprovalError(ApprovalErrorTag.Unauthorized, info);
		}

		public ApprovalError.GenericErrorInfo AsGenericError()
		{
			this.ValidateTag(ApprovalErrorTag.GenericError);
			return (ApprovalError.GenericErrorInfo)this.Value!;
		}

		public ApprovalError.TemporarilyUnavailableInfo AsTemporarilyUnavailable()
		{
			this.ValidateTag(ApprovalErrorTag.TemporarilyUnavailable);
			return (ApprovalError.TemporarilyUnavailableInfo)this.Value!;
		}

		public ApprovalError.UnauthorizedInfo AsUnauthorized()
		{
			this.ValidateTag(ApprovalErrorTag.Unauthorized);
			return (ApprovalError.UnauthorizedInfo)this.Value!;
		}

		private void ValidateTag(ApprovalErrorTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class GenericErrorInfo
		{
			[CandidName("error_code")]
			public UnboundedUInt ErrorCode { get; set; }

			[CandidName("message")]
			public string Message { get; set; }

			public GenericErrorInfo(UnboundedUInt errorCode, string message)
			{
				this.ErrorCode = errorCode;
				this.Message = message;
			}

			public GenericErrorInfo()
			{
			}
		}

		public class TemporarilyUnavailableInfo
		{
			public TemporarilyUnavailableInfo()
			{
			}
		}

		public class UnauthorizedInfo
		{
			[CandidName("token_ids")]
			public ApprovalError.UnauthorizedInfo.TokenIdsInfo TokenIds { get; set; }

			public UnauthorizedInfo(ApprovalError.UnauthorizedInfo.TokenIdsInfo tokenIds)
			{
				this.TokenIds = tokenIds;
			}

			public UnauthorizedInfo()
			{
			}

			public class TokenIdsInfo : List<TokenId>
			{
				public TokenIdsInfo()
				{
				}
			}
		}
	}

	public enum ApprovalErrorTag
	{
		GenericError,
		TemporarilyUnavailable,
		TooOld,
		Unauthorized
	}
}