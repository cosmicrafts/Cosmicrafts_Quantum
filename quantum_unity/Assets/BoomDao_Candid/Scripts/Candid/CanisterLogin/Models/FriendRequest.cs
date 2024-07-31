using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Playerid2 = EdjCase.ICP.Candid.Models.Principal;

namespace CanisterPK.CanisterLogin.Models
{
	public class FriendRequest
	{
		[CandidName("from")]
		public Playerid2 From { get; set; }

		[CandidName("timestamp")]
		public UnboundedInt Timestamp { get; set; }

		[CandidName("to")]
		public Playerid2 To { get; set; }

		public FriendRequest(Playerid2 from, UnboundedInt timestamp, Playerid2 to)
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