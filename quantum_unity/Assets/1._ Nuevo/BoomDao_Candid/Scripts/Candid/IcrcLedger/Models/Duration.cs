using EdjCase.ICP.Candid.Mapping;

namespace Candid.IcrcLedger.Models
{
	public class Duration
	{
		[CandidName("secs")]
		public ulong Secs { get; set; }

		[CandidName("nanos")]
		public uint Nanos { get; set; }

		public Duration(ulong secs, uint nanos)
		{
			this.Secs = secs;
			this.Nanos = nanos;
		}

		public Duration()
		{
		}
	}
}