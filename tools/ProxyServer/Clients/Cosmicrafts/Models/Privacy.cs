using EdjCase.ICP.Candid.Mapping;

namespace ProxyServer.Cosmicrafts.Models
{
	public class Privacy
	{
		[CandidName("friend_auto_accept")]
		public bool FriendAutoAccept { get; set; }

		[CandidName("notifications_show_all")]
		public bool NotificationsShowAll { get; set; }

		[CandidName("only_add_friends")]
		public bool OnlyAddFriends { get; set; }

		[CandidName("only_msg_fiends")]
		public bool OnlyMsgFiends { get; set; }

		public Privacy(bool friendAutoAccept, bool notificationsShowAll, bool onlyAddFriends, bool onlyMsgFiends)
		{
			this.FriendAutoAccept = friendAutoAccept;
			this.NotificationsShowAll = notificationsShowAll;
			this.OnlyAddFriends = onlyAddFriends;
			this.OnlyMsgFiends = onlyMsgFiends;
		}

		public Privacy()
		{
		}
	}
}