using EdjCase.ICP.Candid.Mapping;
using Timestamp = System.UInt64;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using Playerid1 = EdjCase.ICP.Candid.Models.Principal;

namespace Cosmicrafts.backend.Models
{
	public class Notification
	{
		[CandidName("from")]
		public Playerid1 From { get; set; }

		[CandidName("message")]
		public string Message { get; set; }

		[CandidName("timestamp")]
		public Time Timestamp { get; set; }

		public Notification(Playerid1 from, string message, Time timestamp)
		{
			this.From = from;
			this.Message = message;
			this.Timestamp = timestamp;
		}

		public Notification()
		{
		}
	}
}