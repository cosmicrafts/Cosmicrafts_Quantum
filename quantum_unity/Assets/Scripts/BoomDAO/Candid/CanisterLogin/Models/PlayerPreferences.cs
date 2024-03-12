using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace CanisterPK.CanisterLogin.Models
{
	public class PlayerPreferences
	{
		[CandidName("language")]
		public UnboundedUInt Language { get; set; }

		[CandidName("playerChar")]
		public string PlayerChar { get; set; }

		public PlayerPreferences(UnboundedUInt language, string playerChar)
		{
			this.Language = language;
			this.PlayerChar = playerChar;
		}

		public PlayerPreferences()
		{
		}
	}
}