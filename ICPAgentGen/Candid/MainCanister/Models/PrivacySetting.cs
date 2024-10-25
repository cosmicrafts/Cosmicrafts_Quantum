using EdjCase.ICP.Candid.Mapping;

namespace Cosmicrafts.MainCanister.Models
{
	public enum PrivacySetting
	{
		[CandidName("acceptAll")]
		AcceptAll,
		[CandidName("blockAll")]
		BlockAll,
		[CandidName("friendsOfFriends")]
		FriendsOfFriends
	}
}