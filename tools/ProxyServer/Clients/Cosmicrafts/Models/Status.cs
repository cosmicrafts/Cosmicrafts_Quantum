using EdjCase.ICP.Candid.Mapping;

namespace ProxyServer.Cosmicrafts.Models
{
	public enum Status
	{
		Available,
		Away,
		[CandidName("Do_not_Disturb")]
		DoNotDisturb,
		Offline
	}
}