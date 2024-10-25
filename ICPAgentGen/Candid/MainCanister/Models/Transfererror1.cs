using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.MainCanister.Models;
using System;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using TransferId = EdjCase.ICP.Candid.Models.UnboundedUInt;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.MainCanister.Models
{
	[Variant]
	public class Transfererror1
	{
		[VariantTagProperty]
		public Transfererror1Tag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Transfererror1(Transfererror1Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Transfererror1()
		{
		}

		public static Transfererror1 CreatedInFuture(Transfererror1.CreatedInFutureInfo info)
		{
			return new Transfererror1(Transfererror1Tag.CreatedInFuture, info);
		}

		public static Transfererror1 Duplicate(Transfererror1.DuplicateInfo info)
		{
			return new Transfererror1(Transfererror1Tag.Duplicate, info);
		}

		public static Transfererror1 GenericError(Transfererror1.GenericErrorInfo info)
		{
			return new Transfererror1(Transfererror1Tag.GenericError, info);
		}

		public static Transfererror1 TemporarilyUnavailable(Transfererror1.TemporarilyUnavailableInfo info)
		{
			return new Transfererror1(Transfererror1Tag.TemporarilyUnavailable, info);
		}

		public static Transfererror1 TooOld()
		{
			return new Transfererror1(Transfererror1Tag.TooOld, null);
		}

		public static Transfererror1 Unauthorized(Transfererror1.UnauthorizedInfo info)
		{
			return new Transfererror1(Transfererror1Tag.Unauthorized, info);
		}

		public Transfererror1.CreatedInFutureInfo AsCreatedInFuture()
		{
			this.ValidateTag(Transfererror1Tag.CreatedInFuture);
			return (Transfererror1.CreatedInFutureInfo)this.Value!;
		}

		public Transfererror1.DuplicateInfo AsDuplicate()
		{
			this.ValidateTag(Transfererror1Tag.Duplicate);
			return (Transfererror1.DuplicateInfo)this.Value!;
		}

		public Transfererror1.GenericErrorInfo AsGenericError()
		{
			this.ValidateTag(Transfererror1Tag.GenericError);
			return (Transfererror1.GenericErrorInfo)this.Value!;
		}

		public Transfererror1.TemporarilyUnavailableInfo AsTemporarilyUnavailable()
		{
			this.ValidateTag(Transfererror1Tag.TemporarilyUnavailable);
			return (Transfererror1.TemporarilyUnavailableInfo)this.Value!;
		}

		public Transfererror1.UnauthorizedInfo AsUnauthorized()
		{
			this.ValidateTag(Transfererror1Tag.Unauthorized);
			return (Transfererror1.UnauthorizedInfo)this.Value!;
		}

		private void ValidateTag(Transfererror1Tag tag)
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

	public enum Transfererror1Tag
	{
		CreatedInFuture,
		Duplicate,
		GenericError,
		TemporarilyUnavailable,
		TooOld,
		Unauthorized
	}
}