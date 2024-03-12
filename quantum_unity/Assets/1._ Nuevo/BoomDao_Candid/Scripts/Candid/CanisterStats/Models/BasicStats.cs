using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace CanisterPK.CanisterStats.Models
{
	public class BasicStats
	{
		[CandidName("botDifficulty")]
		public UnboundedUInt BotDifficulty { get; set; }

		[CandidName("botMode")]
		public UnboundedUInt BotMode { get; set; }

		[CandidName("characterID")]
		public string CharacterID { get; set; }

		[CandidName("damageCritic")]
		public double DamageCritic { get; set; }

		[CandidName("damageDealt")]
		public double DamageDealt { get; set; }

		[CandidName("damageEvaded")]
		public double DamageEvaded { get; set; }

		[CandidName("damageTaken")]
		public double DamageTaken { get; set; }

		[CandidName("deploys")]
		public double Deploys { get; set; }

		[CandidName("energyChargeRate")]
		public double EnergyChargeRate { get; set; }

		[CandidName("energyGenerated")]
		public double EnergyGenerated { get; set; }

		[CandidName("energyUsed")]
		public double EnergyUsed { get; set; }

		[CandidName("energyWasted")]
		public double EnergyWasted { get; set; }

		[CandidName("faction")]
		public UnboundedUInt Faction { get; set; }

		[CandidName("gameMode")]
		public UnboundedUInt GameMode { get; set; }

		[CandidName("kills")]
		public double Kills { get; set; }

		[CandidName("secRemaining")]
		public double SecRemaining { get; set; }

		[CandidName("wonGame")]
		public bool WonGame { get; set; }

		[CandidName("xpEarned")]
		public double XpEarned { get; set; }

		public BasicStats(UnboundedUInt botDifficulty, UnboundedUInt botMode, string characterID, double damageCritic, double damageDealt, double damageEvaded, double damageTaken, double deploys, double energyChargeRate, double energyGenerated, double energyUsed, double energyWasted, UnboundedUInt faction, UnboundedUInt gameMode, double kills, double secRemaining, bool wonGame, double xpEarned)
		{
			this.BotDifficulty = botDifficulty;
			this.BotMode = botMode;
			this.CharacterID = characterID;
			this.DamageCritic = damageCritic;
			this.DamageDealt = damageDealt;
			this.DamageEvaded = damageEvaded;
			this.DamageTaken = damageTaken;
			this.Deploys = deploys;
			this.EnergyChargeRate = energyChargeRate;
			this.EnergyGenerated = energyGenerated;
			this.EnergyUsed = energyUsed;
			this.EnergyWasted = energyWasted;
			this.Faction = faction;
			this.GameMode = gameMode;
			this.Kills = kills;
			this.SecRemaining = secRemaining;
			this.WonGame = wonGame;
			this.XpEarned = xpEarned;
		}

		public BasicStats()
		{
		}
	}
}