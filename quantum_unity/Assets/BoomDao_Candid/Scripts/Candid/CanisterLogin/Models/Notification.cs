using EdjCase.ICP.Candid.Mapping;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using Playerid2 = EdjCase.ICP.Candid.Models.Principal;

namespace CanisterPK.CanisterLogin.Models
{
	public class Notification
	{
		[CandidName("from")]
		public Playerid2 From { get; set; }

		[CandidName("message")]
		public string Message { get; set; }

		[CandidName("timestamp")]
		public Time Timestamp { get; set; }

		public Notification(Playerid2 from, string message, Time timestamp)
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