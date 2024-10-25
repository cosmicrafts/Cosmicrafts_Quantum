using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.MainCanister.Models;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;

namespace Cosmicrafts.MainCanister.Models
{
	public class RefAccView
	{
		[CandidName("currentTier")]
		public Tier CurrentTier { get; set; }

		[CandidName("multiplier")]
		public double Multiplier { get; set; }

		[CandidName("netWorth")]
		public UnboundedUInt NetWorth { get; set; }

		[CandidName("playerID")]
		public Principal PlayerID { get; set; }

		[CandidName("playerName")]
		public string PlayerName { get; set; }

		[CandidName("signupTokenSum")]
		public UnboundedUInt SignupTokenSum { get; set; }

		[CandidName("singupLink")]
		public string SingupLink { get; set; }

		[CandidName("tierTokenSum")]
		public UnboundedUInt TierTokenSum { get; set; }

		[CandidName("topPlayers")]
		public List<TopView> TopPlayers { get; set; }

		[CandidName("topPosition")]
		public UnboundedUInt TopPosition { get; set; }

		[CandidName("topTokenAmount")]
		public (UnboundedUInt, string) TopTokenAmount { get; set; }

		public RefAccView(Tier currentTier, double multiplier, UnboundedUInt netWorth, Principal playerID, string playerName, UnboundedUInt signupTokenSum, string singupLink, UnboundedUInt tierTokenSum, List<TopView> topPlayers, UnboundedUInt topPosition, (UnboundedUInt, string) topTokenAmount)
		{
			this.CurrentTier = currentTier;
			this.Multiplier = multiplier;
			this.NetWorth = netWorth;
			this.PlayerID = playerID;
			this.PlayerName = playerName;
			this.SignupTokenSum = signupTokenSum;
			this.SingupLink = singupLink;
			this.TierTokenSum = tierTokenSum;
			this.TopPlayers = topPlayers;
			this.TopPosition = topPosition;
			this.TopTokenAmount = topTokenAmount;
		}

		public RefAccView()
		{
		}
	}
}