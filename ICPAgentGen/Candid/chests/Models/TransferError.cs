using EdjCase.ICP.Candid.Mapping;
using CanisterPK.chests.Models;
using System;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using TransferId = EdjCase.ICP.Candid.Models.UnboundedUInt;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.chests.Models
{
	[Variant]
	public class TransferError
	{
		[VariantTagProperty]
		public TransferErrorTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public TransferError(TransferErrorTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected TransferError()
		{
		}

		public static TransferError CreatedInFuture(TransferError.CreatedInFutureInfo info)
		{
			return new TransferError(TransferErrorTag.CreatedInFuture, info);
		}

		public static TransferError Duplicate(TransferError.DuplicateInfo info)
		{
			return new TransferError(TransferErrorTag.Duplicate, info);
		}

		public static TransferError GenericError(TransferError.GenericErrorInfo info)
		{
			return new TransferError(TransferErrorTag.GenericError, info);
		}

		public static TransferError TemporarilyUnavailable(TransferError.TemporarilyUnavailableInfo info)
		{
			return new TransferError(TransferErrorTag.TemporarilyUnavailable, info);
		}

		public static TransferError TooOld()
		{
			return new TransferError(TransferErrorTag.TooOld, null);
		}

		public static TransferError Unauthorized(TransferError.UnauthorizedInfo info)
		{
			return new TransferError(TransferErrorTag.Unauthorized, info);
		}

		public TransferError.CreatedInFutureInfo AsCreatedInFuture()
		{
			this.ValidateTag(TransferErrorTag.CreatedInFuture);
			return (TransferError.CreatedInFutureInfo)this.Value!;
		}

		public TransferError.DuplicateInfo AsDuplicate()
		{
			this.ValidateTag(TransferErrorTag.Duplicate);
			return (TransferError.DuplicateInfo)this.Value!;
		}

		public TransferError.GenericErrorInfo AsGenericError()
		{
			this.ValidateTag(TransferErrorTag.GenericError);
			return (TransferError.GenericErrorInfo)this.Value!;
		}

		public TransferError.TemporarilyUnavailableInfo AsTemporarilyUnavailable()
		{
			this.ValidateTag(TransferErrorTag.TemporarilyUnavailable);
			return (TransferError.TemporarilyUnavailableInfo)this.Value!;
		}

		public TransferError.UnauthorizedInfo AsUnauthorized()
		{
			this.ValidateTag(TransferErrorTag.Unauthorized);
			return (TransferError.UnauthorizedInfo)this.Value!;
		}

		private void ValidateTag(TransferErrorTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class CreatedInFutureInfo
		{
			[CandidName("ledger_time")]
			public ulong LedgerTime { get; set; }

			public CreatedInFutureInfo(ulong ledgerTime)
			{
				this.LedgerTime = ledgerTime;
			}

			public CreatedInFutureInfo()
			{
			}
		}

		public class DuplicateInfo
		{
			[CandidName("duplicate_of")]
			public TransferId DuplicateOf { get; set; }

			public DuplicateInfo(TransferId duplicateOf)
			{
				this.DuplicateOf = duplicateOf;
			}

			public DuplicateInfo()
			{
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
			public List<TokenId> TokenIds { get; set; }

			public UnauthorizedInfo(List<TokenId> tokenIds)
			{
				this.TokenIds = tokenIds;
			}

			public UnauthorizedInfo()
			{
			}
		}
	}

	public enum TransferErrorTag
	{
		CreatedInFuture,
		Duplicate,
		GenericError,
		TemporarilyUnavailable,
		TooOld,
		Unauthorized
	}
}