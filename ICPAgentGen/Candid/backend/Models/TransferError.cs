using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.backend.Models;
using System;
using EdjCase.ICP.Candid.Models;
using TxIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Timestamp = System.UInt64;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.backend.Models
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

		public static TransferError BadBurn(TransferError.BadBurnInfo info)
		{
			return new TransferError(TransferErrorTag.BadBurn, info);
		}

		public static TransferError BadFee(TransferError.BadFeeInfo info)
		{
			return new TransferError(TransferErrorTag.BadFee, info);
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

		public static TransferError InsufficientFunds(TransferError.InsufficientFundsInfo info)
		{
			return new TransferError(TransferErrorTag.InsufficientFunds, info);
		}

		public static TransferError TemporarilyUnavailable()
		{
			return new TransferError(TransferErrorTag.TemporarilyUnavailable, null);
		}

		public static TransferError TooOld()
		{
			return new TransferError(TransferErrorTag.TooOld, null);
		}

		public TransferError.BadBurnInfo AsBadBurn()
		{
			this.ValidateTag(TransferErrorTag.BadBurn);
			return (TransferError.BadBurnInfo)this.Value!;
		}

		public TransferError.BadFeeInfo AsBadFee()
		{
			this.ValidateTag(TransferErrorTag.BadFee);
			return (TransferError.BadFeeInfo)this.Value!;
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

		public TransferError.InsufficientFundsInfo AsInsufficientFunds()
		{
			this.ValidateTag(TransferErrorTag.InsufficientFunds);
			return (TransferError.InsufficientFundsInfo)this.Value!;
		}

		private void ValidateTag(TransferErrorTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class BadBurnInfo
		{
			[CandidName("min_burn_amount")]
			public Balance MinBurnAmount { get; set; }

			public BadBurnInfo(Balance minBurnAmount)
			{
				this.MinBurnAmount = minBurnAmount;
			}

			public BadBurnInfo()
			{
			}
		}

		public class BadFeeInfo
		{
			[CandidName("expected_fee")]
			public Balance ExpectedFee { get; set; }

			public BadFeeInfo(Balance expectedFee)
			{
				this.ExpectedFee = expectedFee;
			}

			public BadFeeInfo()
			{
			}
		}

		public class CreatedInFutureInfo
		{
			[CandidName("ledger_time")]
			public Timestamp LedgerTime { get; set; }

			public CreatedInFutureInfo(Timestamp ledgerTime)
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
			public TxIndex DuplicateOf { get; set; }

			public DuplicateInfo(TxIndex duplicateOf)
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

		public class InsufficientFundsInfo
		{
			[CandidName("balance")]
			public Balance Balance { get; set; }

			public InsufficientFundsInfo(Balance balance)
			{
				this.Balance = balance;
			}

			public InsufficientFundsInfo()
			{
			}
		}
	}

	public enum TransferErrorTag
	{
		BadBurn,
		BadFee,
		CreatedInFuture,
		Duplicate,
		GenericError,
		InsufficientFunds,
		TemporarilyUnavailable,
		TooOld
	}
}