using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace CanisterPK.CanisterLogin.Models
{
	public class TopView
	{
		[CandidName("multiplier")]
		public double Multiplier { get; set; }

		[CandidName("netWorth")]
		public UnboundedUInt NetWorth { get; set; }

		[CandidName("playerName")]
		public string PlayerName { get; set; }

		public TopView(double multiplier, UnboundedUInt netWorth, string playerName)
		{
			this.Multiplier = multiplier;
			this.NetWorth = netWorth;
			this.PlayerName = playerName;
		}

		public TopView()
		{
		}
	}
}