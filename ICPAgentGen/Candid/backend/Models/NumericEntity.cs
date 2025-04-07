using EdjCase.ICP.Candid.Mapping;

namespace Cosmicrafts.backend.Models
{
	public class NumericEntity
	{
		[CandidName("avg")]
		public ulong Avg { get; set; }

		[CandidName("first")]
		public ulong First { get; set; }

		[CandidName("last")]
		public ulong Last { get; set; }

		[CandidName("max")]
		public ulong Max { get; set; }

		[CandidName("min")]
		public ulong Min { get; set; }

		public NumericEntity(ulong avg, ulong first, ulong last, ulong max, ulong min)
		{
			this.Avg = avg;
			this.First = first;
			this.Last = last;
			this.Max = max;
			this.Min = min;
		}

		public NumericEntity()
		{
		}
	}
}