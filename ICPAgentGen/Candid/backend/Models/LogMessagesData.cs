using EdjCase.ICP.Candid.Mapping;
using Nanos = System.UInt64;

namespace Cosmicrafts.backend.Models
{
	public class LogMessagesData
	{
		[CandidName("message")]
		public string Message { get; set; }

		[CandidName("timeNanos")]
		public Nanos TimeNanos { get; set; }

		public LogMessagesData(string message, Nanos timeNanos)
		{
			this.Message = message;
			this.TimeNanos = timeNanos;
		}

		public LogMessagesData()
		{
		}
	}
}