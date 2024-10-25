using EdjCase.ICP.Candid.Mapping;
using ProxyServer.Cosmicrafts.Models;
using EdjCase.ICP.Candid.Models;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;

namespace ProxyServer.Cosmicrafts.Models
{
	public class Notification
	{
		[CandidName("body")]
		public string Body { get; set; }

		[CandidName("from")]
		public NotificationIdentity From { get; set; }

		[CandidName("id")]
		public UnboundedUInt Id { get; set; }

		[CandidName("timestamp")]
		public Notification.TimestampInfo Timestamp { get; set; }

		[CandidName("to")]
		public NotificationIdentity To { get; set; }

		public Notification(string body, NotificationIdentity from, UnboundedUInt id, Notification.TimestampInfo timestamp, NotificationIdentity to)
		{
			this.Body = body;
			this.From = from;
			this.Id = id;
			this.Timestamp = timestamp;
			this.To = to;
		}

		public Notification()
		{
		}

		public class TimestampInfo : OptionalValue<Time>
		{
			public TimestampInfo()
			{
			}

			public TimestampInfo(Time value) : base(value)
			{
			}
		}
	}
}