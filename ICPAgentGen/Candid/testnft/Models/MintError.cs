using EdjCase.ICP.Candid.Mapping;
using CanisterPK.testnft.Models;
using System;
using EdjCase.ICP.Candid.Models;

namespace CanisterPK.testnft.Models
{
	[Variant]
	public class MintError
	{
		[VariantTagProperty]
		public MintErrorTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public MintError(MintErrorTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected MintError()
		{
		}

		public static MintError AlreadyExistTokenId()
		{
			return new MintError(MintErrorTag.AlreadyExistTokenId, null);
		}

		public static MintError GenericError(MintError.GenericErrorInfo info)
		{
			return new MintError(MintErrorTag.GenericError, info);
		}

		public static MintError InvalidRecipient()
		{
			return new MintError(MintErrorTag.InvalidRecipient, null);
		}

		public static MintError SupplyCapOverflow()
		{
			return new MintError(MintErrorTag.SupplyCapOverflow, null);
		}

		public static MintError Unauthorized()
		{
			return new MintError(MintErrorTag.Unauthorized, null);
		}

		public MintError.GenericErrorInfo AsGenericError()
		{
			this.ValidateTag(MintErrorTag.GenericError);
			return (MintError.GenericErrorInfo)this.Value!;
		}

		private void ValidateTag(MintErrorTag tag)
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
	}

	public enum MintErrorTag
	{
		AlreadyExistTokenId,
		GenericError,
		InvalidRecipient,
		SupplyCapOverflow,
		Unauthorized
	}
}