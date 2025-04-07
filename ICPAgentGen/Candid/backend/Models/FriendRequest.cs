using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Timestamp = System.UInt64;
using Playerid1 = EdjCase.ICP.Candid.Models.Principal;

namespace Cosmicrafts.backend.Models
{
	public class FriendRequest
	{
		[CandidName("from")]
		public Playerid1 From { get; set; }

		[CandidName("timestamp")]
		public UnboundedInt Timestamp { get; set; }

		[CandidName("to")]
		public Playerid1 To { get; set; }

		public FriendRequest(Playerid1 from, UnboundedInt timestamp, Playerid1 to)
		{
			this.From = from;
			this.Timestamp = timestamp;
			this.To = to;
		}

		public FriendRequest()
		{
		}
	}
}