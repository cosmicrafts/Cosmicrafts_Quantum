using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using UserID = EdjCase.ICP.Candid.Models.Principal;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;

namespace ProxyServer.Cosmicrafts.Models
{
	public class Comment
	{
		[CandidName("content")]
		public string Content { get; set; }

		[CandidName("fromUserID")]
		public UserID FromUserID { get; set; }

		[CandidName("fromUsername")]
		public string FromUsername { get; set; }

		[CandidName("id")]
		public UnboundedUInt Id { get; set; }

		[CandidName("postId")]
		public UnboundedUInt PostId { get; set; }

		[CandidName("timestamp")]
		public Time Timestamp { get; set; }

		public Comment(string content, UserID fromUserID, string fromUsername, UnboundedUInt id, UnboundedUInt postId, Time timestamp)
		{
			this.Content = content;
			this.FromUserID = fromUserID;
			this.FromUsername = fromUsername;
			this.Id = id;
			this.PostId = postId;
			this.Timestamp = timestamp;
		}

		public Comment()
		{
		}
	}
}