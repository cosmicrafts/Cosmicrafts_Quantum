using EdjCase.ICP.Candid.Mapping;
using ProxyServer.Cosmicrafts.Models;

namespace ProxyServer.Cosmicrafts.Models
{
	public class UserProfile
	{
		[CandidName("basicInfo")]
		public UserBasicInfo BasicInfo { get; set; }

		[CandidName("network")]
		public UserNetwork Network { get; set; }

		public UserProfile(UserBasicInfo basicInfo, UserNetwork network)
		{
			this.BasicInfo = basicInfo;
			this.Network = network;
		}

		public UserProfile()
		{
		}
	}
}