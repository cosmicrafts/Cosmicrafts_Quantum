using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Cosmicrafts.backend.Models;
using EdjCase.ICP.Candid.Models;
using Nanos = System.UInt64;

namespace Cosmicrafts.backend.Models
{
	public class CanisterLogMessages
	{
		[CandidName("data")]
		public List<LogMessagesData> Data { get; set; }

		[CandidName("lastAnalyzedMessageTimeNanos")]
		public CanisterLogMessages.LastAnalyzedMessageTimeNanosInfo LastAnalyzedMessageTimeNanos { get; set; }

		public CanisterLogMessages(List<LogMessagesData> data, CanisterLogMessages.LastAnalyzedMessageTimeNanosInfo lastAnalyzedMessageTimeNanos)
		{
			this.Data = data;
			this.LastAnalyzedMessageTimeNanos = lastAnalyzedMessageTimeNanos;
		}

		public CanisterLogMessages()
		{
		}

		public class LastAnalyzedMessageTimeNanosInfo : OptionalValue<Nanos>
		{
			public LastAnalyzedMessageTimeNanosInfo()
			{
			}

			public LastAnalyzedMessageTimeNanosInfo(Nanos value) : base(value)
			{
			}
		}
	}
}