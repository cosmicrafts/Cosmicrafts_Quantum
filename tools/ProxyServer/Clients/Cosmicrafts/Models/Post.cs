using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using ProxyServer.Cosmicrafts.Models;
using UserID = EdjCase.ICP.Candid.Models.Principal;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;

namespace ProxyServer.Cosmicrafts.Models
{
	public class Post
	{
		[CandidName("comments")]
		public OptionalValue<List<Comment>> Comments { get; set; }

		[CandidName("content")]
		public string Content { get; set; }

		[CandidName("id")]
		public UnboundedUInt Id { get; set; }

		[CandidName("images")]
		public OptionalValue<List<UnboundedUInt>> Images { get; set; }

		[CandidName("timestamp")]
		public Time Timestamp { get; set; }

		[CandidName("userId")]
		public UserID UserId { get; set; }

		[CandidName("username")]
		public string Username { get; set; }

		public Post(OptionalValue<List<Comment>> comments, string content, UnboundedUInt id, OptionalValue<List<UnboundedUInt>> images, Time timestamp, UserID userId, string username)
		{
			this.Comments = comments;
			this.Content = content;
			this.Id = id;
			this.Images = images;
			this.Timestamp = timestamp;
			this.UserId = userId;
			this.Username = username;
		}

		public Post()
		{
		}
	}
}