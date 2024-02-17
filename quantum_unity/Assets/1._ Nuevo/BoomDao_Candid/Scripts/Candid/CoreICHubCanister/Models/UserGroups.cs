using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using GroupID = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.CoreICHubCanister.Models
{
	public class UserGroups
	{
		[CandidName("groups")]
		public List<GroupID> Groups { get; set; }

		public UserGroups(List<GroupID> groups)
		{
			this.Groups = groups;
		}

		public UserGroups()
		{
		}
	}
}