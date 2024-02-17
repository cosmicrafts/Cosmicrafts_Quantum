using EdjCase.ICP.Candid.Mapping;
using CanisterPK.CoreICHubCanister.Models;
using System.Collections.Generic;
using UserID = EdjCase.ICP.Candid.Models.Principal;

namespace CanisterPK.CoreICHubCanister.Models
{
	public class Friends
	{
		[CandidName("list")]
		public Friends.ListInfo List { get; set; }

		[CandidName("pending")]
		public Friends.PendingInfo Pending { get; set; }

		public Friends(Friends.ListInfo list, Friends.PendingInfo pending)
		{
			this.List = list;
			this.Pending = pending;
		}

		public Friends()
		{
		}

		public class ListInfo : List<UserID>
		{
			public ListInfo()
			{
			}
		}

		public class PendingInfo : List<UserID>
		{
			public PendingInfo()
			{
			}
		}
	}
}