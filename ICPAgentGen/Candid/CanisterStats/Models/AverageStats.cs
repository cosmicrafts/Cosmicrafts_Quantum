using EdjCase.ICP.Candid.Mapping;

namespace CanisterPK.CanisterStats.Models
{
	public class AverageStats
	{
		[CandidName("averageDamageDealt")]
		public double AverageDamageDealt { get; set; }

		[CandidName("averageEnergyGenerated")]
		public double AverageEnergyGenerated { get; set; }

		[CandidName("averageEnergyUsed")]
		public double AverageEnergyUsed { get; set; }

		[CandidName("averageEnergyWasted")]
		public double AverageEnergyWasted { get; set; }

		[CandidName("averageKills")]
		public double AverageKills { get; set; }

		[CandidName("averageXpEarned")]
		public double AverageXpEarned { get; set; }

		public AverageStats(double averageDamageDealt, double averageEnergyGenerated, double averageEnergyUsed, double averageEnergyWasted, double averageKills, double averageXpEarned)
		{
			this.AverageDamageDealt = averageDamageDealt;
			this.AverageEnergyGenerated = averageEnergyGenerated;
			this.AverageEnergyUsed = averageEnergyUsed;
			this.AverageEnergyWasted = averageEnergyWasted;
			this.AverageKills = averageKills;
			this.AverageXpEarned = averageXpEarned;
		}

		public AverageStats()
		{
		}
	}
}