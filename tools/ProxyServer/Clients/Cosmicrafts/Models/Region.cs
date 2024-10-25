using EdjCase.ICP.Candid.Mapping;

namespace ProxyServer.Cosmicrafts.Models
{
	public enum Region
	{
		Asia,
		Europe,
		[CandidName("South_America")]
		SouthAmerica,
		[CandidName("US_East")]
		UsEast,
		[CandidName("US_West")]
		UsWest
	}
}