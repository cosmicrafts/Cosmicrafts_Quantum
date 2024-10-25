using EdjCase.ICP.Candid.Mapping;
using ProxyServer.Cosmicrafts.Models;
using RegistrationDate = EdjCase.ICP.Candid.Models.UnboundedInt;

namespace ProxyServer.Cosmicrafts.Models
{
	public class SocialConnection
	{
		[CandidName("memberSince")]
		public RegistrationDate MemberSince { get; set; }

		[CandidName("platform")]
		public Platform Platform { get; set; }

		[CandidName("profileLink")]
		public string ProfileLink { get; set; }

		[CandidName("username")]
		public string Username { get; set; }

		public SocialConnection(RegistrationDate memberSince, Platform platform, string profileLink, string username)
		{
			this.MemberSince = memberSince;
			this.Platform = platform;
			this.ProfileLink = profileLink;
			this.Username = username;
		}

		public SocialConnection()
		{
		}
	}
}