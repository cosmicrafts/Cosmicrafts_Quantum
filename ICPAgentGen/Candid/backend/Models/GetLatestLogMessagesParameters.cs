using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.backend.Models;
using Nanos = System.UInt64;

namespace Cosmicrafts.backend.Models
{
	public class GetLatestLogMessagesParameters
	{
		[CandidName("count")]
		public uint Count { get; set; }

		[CandidName("filter")]
		public OptionalValue<GetLogMessagesFilter> Filter { get; set; }

		[CandidName("upToTimeNanos")]
		public GetLatestLogMessagesParameters.UpToTimeNanosInfo UpToTimeNanos { get; set; }

		public GetLatestLogMessagesParameters(uint count, OptionalValue<GetLogMessagesFilter> filter, GetLatestLogMessagesParameters.UpToTimeNanosInfo upToTimeNanos)
		{
			this.Count = count;
			this.Filter = filter;
			this.UpToTimeNanos = upToTimeNanos;
		}

		public GetLatestLogMessagesParameters()
		{
		}

		public class UpToTimeNanosInfo : OptionalValue<Nanos>
		{
			public UpToTimeNanosInfo()
			{
			}

			public UpToTimeNanosInfo(Nanos value) : base(value)
			{
			}
		}
	}
}