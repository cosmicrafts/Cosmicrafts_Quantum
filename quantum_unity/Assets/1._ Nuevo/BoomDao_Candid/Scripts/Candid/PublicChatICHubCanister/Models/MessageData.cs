using EdjCase.ICP.Candid.Mapping;
using Username = System.String;
using Userid1 = EdjCase.ICP.Candid.Models.Principal;
using UserID = EdjCase.ICP.Candid.Models.Principal;

namespace CanisterPK.PublicChatICHubCanister.Models
{
	public class MessageData
	{
		[CandidName("text")]
		public string Text { get; set; }

		[CandidName("time")]
		public ulong Time { get; set; }

		[CandidName("userID")]
		public Userid1 UserID { get; set; }

		[CandidName("username")]
		public Username Username { get; set; }

		public MessageData(string text, ulong time, Userid1 userID, Username username)
		{
			this.Text = text;
			this.Time = time;
			this.UserID = userID;
			this.Username = username;
		}

		public MessageData()
		{
		}
	}
}