using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Cosmicrafts.backend.Models
{
	public class AverageStats
	{
		[CandidName("averageDamageDealt")]
		public UnboundedUInt AverageDamageDealt { get; set; }

		[CandidName("averageEnergyGenerated")]
		public UnboundedUInt AverageEnergyGenerated { get; set; }

		[CandidName("averageEnergyUsed")]
		public UnboundedUInt AverageEnergyUsed { get; set; }

		[CandidName("averageEnergyWasted")]
		public UnboundedUInt AverageEnergyWasted { get; set; }

		[CandidName("averageKills")]
		public UnboundedUInt AverageKills { get; set; }

		[CandidName("averageXpEarned")]
		public UnboundedUInt AverageXpEarned { get; set; }

		public AverageStats(UnboundedUInt averageDamageDealt, UnboundedUInt averageEnergyGenerated, UnboundedUInt averageEnergyUsed, UnboundedUInt averageEnergyWasted, UnboundedUInt averageKills, UnboundedUInt averageXpEarned)
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