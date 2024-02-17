using EdjCase.ICP.Candid.Mapping;
using Userid1 = EdjCase.ICP.Candid.Models.Principal;
using UserID = EdjCase.ICP.Candid.Models.Principal;

namespace CanisterPK.PublicChatICHubCanister.Models
{
	public class RequestJoinData
	{
		[CandidName("dateRequested")]
		public ulong DateRequested { get; set; }

		[CandidName("seenByAdmin")]
		public bool SeenByAdmin { get; set; }

		[CandidName("userID")]
		public Userid1 UserID { get; set; }

		public RequestJoinData(ulong dateRequested, bool seenByAdmin, Userid1 userID)
		{
			this.DateRequested = dateRequested;
			this.SeenByAdmin = seenByAdmin;
			this.UserID = userID;
		}

		public RequestJoinData()
		{
		}
	}
}