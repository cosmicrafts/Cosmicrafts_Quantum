using EdjCase.ICP.Candid.Mapping;

namespace Candid.IcrcLedger.Models
{
	public class TimeStamp
	{
		[CandidName("timestamp_nanos")]
		public ulong TimestampNanos { get; set; }

		public TimeStamp(ulong timestampNanos)
		{
			this.TimestampNanos = timestampNanos;
		}

		public TimeStamp()
		{
		}
	}
}