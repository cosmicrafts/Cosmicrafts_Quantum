using EdjCase.ICP.Candid.Mapping;

namespace CanisterPK.PublicChatICHubCanister.Models
{
	public enum UserRoles
	{
		[CandidName("admin")]
		Admin,
		[CandidName("banned")]
		Banned,
		[CandidName("nouser")]
		Nouser,
		[CandidName("owner")]
		Owner,
		[CandidName("user")]
		User
	}
}