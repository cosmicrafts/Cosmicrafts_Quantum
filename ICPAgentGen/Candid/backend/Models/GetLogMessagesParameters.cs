using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.backend.Models;
using Nanos = System.UInt64;

namespace Cosmicrafts.backend.Models
{
	public class GetLogMessagesParameters
	{
		[CandidName("count")]
		public uint Count { get; set; }

		[CandidName("filter")]
		public OptionalValue<GetLogMessagesFilter> Filter { get; set; }

		[CandidName("fromTimeNanos")]
		public GetLogMessagesParameters.FromTimeNanosInfo FromTimeNanos { get; set; }

		public GetLogMessagesParameters(uint count, OptionalValue<GetLogMessagesFilter> filter, GetLogMessagesParameters.FromTimeNanosInfo fromTimeNanos)
		{
			this.Count = count;
			this.Filter = filter;
			this.FromTimeNanos = fromTimeNanos;
		}

		public GetLogMessagesParameters()
		{
		}

		public class FromTimeNanosInfo : OptionalValue<Nanos>
		{
			public FromTimeNanosInfo()
			{
			}

			public FromTimeNanosInfo(Nanos value) : base(value)
			{
			}
		}
	}
}