using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.backend.Models;
using System;

namespace Cosmicrafts.backend.Models
{
	[Variant]
	public class CanisterLogRequest
	{
		[VariantTagProperty]
		public CanisterLogRequestTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public CanisterLogRequest(CanisterLogRequestTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected CanisterLogRequest()
		{
		}

		public static CanisterLogRequest GetLatestMessages(GetLatestLogMessagesParameters info)
		{
			return new CanisterLogRequest(CanisterLogRequestTag.GetLatestMessages, info);
		}

		public static CanisterLogRequest GetMessages(GetLogMessagesParameters info)
		{
			return new CanisterLogRequest(CanisterLogRequestTag.GetMessages, info);
		}

		public static CanisterLogRequest GetMessagesInfo()
		{
			return new CanisterLogRequest(CanisterLogRequestTag.GetMessagesInfo, null);
		}

		public GetLatestLogMessagesParameters AsGetLatestMessages()
		{
			this.ValidateTag(CanisterLogRequestTag.GetLatestMessages);
			return (GetLatestLogMessagesParameters)this.Value!;
		}

		public GetLogMessagesParameters AsGetMessages()
		{
			this.ValidateTag(CanisterLogRequestTag.GetMessages);
			return (GetLogMessagesParameters)this.Value!;
		}

		private void ValidateTag(CanisterLogRequestTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum CanisterLogRequestTag
	{
		[CandidName("getLatestMessages")]
		GetLatestMessages,
		[CandidName("getMessages")]
		GetMessages,
		[CandidName("getMessagesInfo")]
		GetMessagesInfo
	}
}