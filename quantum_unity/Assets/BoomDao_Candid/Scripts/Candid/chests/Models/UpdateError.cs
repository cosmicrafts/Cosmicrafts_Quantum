using EdjCase.ICP.Candid.Mapping;
using CanisterPK.chests.Models;
using System;
using EdjCase.ICP.Candid.Models;

namespace CanisterPK.chests.Models
{
	[Variant]
	public class UpdateError
	{
		[VariantTagProperty]
		public UpdateErrorTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public UpdateError(UpdateErrorTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected UpdateError()
		{
		}

		public static UpdateError DoesntExistTokenId()
		{
			return new UpdateError(UpdateErrorTag.DoesntExistTokenId, null);
		}

		public static UpdateError GenericError(UpdateError.GenericErrorInfo info)
		{
			return new UpdateError(UpdateErrorTag.GenericError, info);
		}

		public static UpdateError InvalidRecipient()
		{
			return new UpdateError(UpdateErrorTag.InvalidRecipient, null);
		}

		public static UpdateError Unauthorized()
		{
			return new UpdateError(UpdateErrorTag.Unauthorized, null);
		}

		public UpdateError.GenericErrorInfo AsGenericError()
		{
			this.ValidateTag(UpdateErrorTag.GenericError);
			return (UpdateError.GenericErrorInfo)this.Value!;
		}

		private void ValidateTag(UpdateErrorTag tag)
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

	public enum UpdateErrorTag
	{
		DoesntExistTokenId,
		GenericError,
		InvalidRecipient,
		Unauthorized
	}
}