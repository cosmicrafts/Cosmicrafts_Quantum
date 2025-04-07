using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.backend.Models;
using Nanos = System.UInt64;

namespace Cosmicrafts.backend.Models
{
	public class CanisterLogMessagesInfo
	{
		[CandidName("count")]
		public uint Count { get; set; }

		[CandidName("features")]
		public List<OptionalValue<CanisterLogFeature>> Features { get; set; }

		[CandidName("firstTimeNanos")]
		public CanisterLogMessagesInfo.FirstTimeNanosInfo FirstTimeNanos { get; set; }

		[CandidName("lastTimeNanos")]
		public CanisterLogMessagesInfo.LastTimeNanosInfo LastTimeNanos { get; set; }

		public CanisterLogMessagesInfo(uint count, List<OptionalValue<CanisterLogFeature>> features, CanisterLogMessagesInfo.FirstTimeNanosInfo firstTimeNanos, CanisterLogMessagesInfo.LastTimeNanosInfo lastTimeNanos)
		{
			this.Count = count;
			this.Features = features;
			this.FirstTimeNanos = firstTimeNanos;
			this.LastTimeNanos = lastTimeNanos;
		}

		public CanisterLogMessagesInfo()
		{
		}

		public class FirstTimeNanosInfo : OptionalValue<Nanos>
		{
			public FirstTimeNanosInfo()
			{
			}

			public FirstTimeNanosInfo(Nanos value) : base(value)
			{
			}
		}

		public class LastTimeNanosInfo : OptionalValue<Nanos>
		{
			public LastTimeNanosInfo()
			{
			}

			public LastTimeNanosInfo(Nanos value) : base(value)
			{
			}
		}
	}
}