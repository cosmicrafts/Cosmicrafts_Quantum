using EdjCase.ICP.Candid.Mapping;
using CanisterPK.testnft.Models;
using System;
using EdjCase.ICP.Candid.Models;

namespace CanisterPK.testnft.Models
{
	[Variant]
	public class UpgradeError
	{
		[VariantTagProperty]
		public UpgradeErrorTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public UpgradeError(UpgradeErrorTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected UpgradeError()
		{
		}

		public static UpgradeError DoesntExistTokenId()
		{
			return new UpgradeError(UpgradeErrorTag.DoesntExistTokenId, null);
		}

		public static UpgradeError GenericError(UpgradeError.GenericErrorInfo info)
		{
			return new UpgradeError(UpgradeErrorTag.GenericError, info);
		}

		public static UpgradeError InvalidRecipient()
		{
			return new UpgradeError(UpgradeErrorTag.InvalidRecipient, null);
		}

		public static UpgradeError Unauthorized()
		{
			return new UpgradeError(UpgradeErrorTag.Unauthorized, null);
		}

		public UpgradeError.GenericErrorInfo AsGenericError()
		{
			this.ValidateTag(UpgradeErrorTag.GenericError);
			return (UpgradeError.GenericErrorInfo)this.Value!;
		}

		private void ValidateTag(UpgradeErrorTag tag)
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

	public enum UpgradeErrorTag
	{
		DoesntExistTokenId,
		GenericError,
		InvalidRecipient,
		Unauthorized
	}
}