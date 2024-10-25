using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace ProxyServer.Cosmicrafts.Models
{
	public class Audio
	{
		[CandidName("master")]
		public UnboundedUInt Master { get; set; }

		[CandidName("music")]
		public UnboundedUInt Music { get; set; }

		[CandidName("sfx")]
		public UnboundedUInt Sfx { get; set; }

		public Audio(UnboundedUInt master, UnboundedUInt music, UnboundedUInt sfx)
		{
			this.Master = master;
			this.Music = music;
			this.Sfx = sfx;
		}

		public Audio()
		{
		}
	}
}