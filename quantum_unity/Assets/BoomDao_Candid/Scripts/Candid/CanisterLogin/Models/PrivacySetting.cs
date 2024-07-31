using EdjCase.ICP.Candid.Mapping;

namespace CanisterPK.CanisterLogin.Models
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