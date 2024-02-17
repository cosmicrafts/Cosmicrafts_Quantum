using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using GroupID = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.CoreICHubCanister.Models
{
	public class GroupData
	{
		[CandidName("avatar")]
		public string Avatar { get; set; }

		[CandidName("canister")]
		public string Canister { get; set; }

		[CandidName("description")]
		public string Description { get; set; }

		[CandidName("groupID")]
		public GroupID GroupID { get; set; }

		[CandidName("isDirect")]
		public bool IsDirect { get; set; }

		[CandidName("isPrivate")]
		public bool IsPrivate { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		[CandidName("owner")]
		public Principal Owner { get; set; }

		public GroupData(string avatar, string canister, string description, GroupID groupID, bool isDirect, bool isPrivate, string name, Principal owner)
		{
			this.Avatar = avatar;
			this.Canister = canister;
			this.Description = description;
			this.GroupID = groupID;
			this.IsDirect = isDirect;
			this.IsPrivate = isPrivate;
			this.Name = name;
			this.Owner = owner;
		}

		public GroupData()
		{
		}
	}
}