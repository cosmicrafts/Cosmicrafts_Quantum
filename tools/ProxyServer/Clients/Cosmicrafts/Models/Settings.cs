using EdjCase.ICP.Candid.Mapping;
using ProxyServer.Cosmicrafts.Models;
using EdjCase.ICP.Candid.Models;

namespace ProxyServer.Cosmicrafts.Models
{
	public class Settings
	{
		[CandidName("audio")]
		public Audio Audio { get; set; }

		[CandidName("language")]
		public UnboundedUInt Language { get; set; }

		[CandidName("ping")]
		public bool Ping { get; set; }

		[CandidName("privacy")]
		public Privacy Privacy { get; set; }

		[CandidName("server")]
		public UnboundedUInt Server { get; set; }

		public Settings(Audio audio, UnboundedUInt language, bool ping, Privacy privacy, UnboundedUInt server)
		{
			this.Audio = audio;
			this.Language = language;
			this.Ping = ping;
			this.Privacy = privacy;
			this.Server = server;
		}

		public Settings()
		{
		}
	}
}