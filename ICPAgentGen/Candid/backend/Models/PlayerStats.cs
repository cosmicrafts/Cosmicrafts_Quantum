using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Playerid1 = EdjCase.ICP.Candid.Models.Principal;
using PlayerId = EdjCase.ICP.Candid.Models.Principal;

namespace Cosmicrafts.backend.Models
{
	public class PlayerStats
	{
		[CandidName("botDifficulty")]
		public UnboundedUInt BotDifficulty { get; set; }

		[CandidName("botMode")]
		public UnboundedUInt BotMode { get; set; }

		[CandidName("characterID")]
		public UnboundedUInt CharacterID { get; set; }

		[CandidName("damageCritic")]
		public UnboundedUInt DamageCritic { get; set; }

		[CandidName("damageDealt")]
		public UnboundedUInt DamageDealt { get; set; }

		[CandidName("damageEvaded")]
		public UnboundedUInt DamageEvaded { get; set; }

		[CandidName("damageTaken")]
		public UnboundedUInt DamageTaken { get; set; }

		[CandidName("deploys")]
		public UnboundedUInt Deploys { get; set; }

		[CandidName("energyChargeRate")]
		public UnboundedUInt EnergyChargeRate { get; set; }

		[CandidName("energyGenerated")]
		public UnboundedUInt EnergyGenerated { get; set; }

		[CandidName("energyUsed")]
		public UnboundedUInt EnergyUsed { get; set; }

		[CandidName("energyWasted")]
		public UnboundedUInt EnergyWasted { get; set; }

		[CandidName("faction")]
		public UnboundedUInt Faction { get; set; }

		[CandidName("gameMode")]
		public UnboundedUInt GameMode { get; set; }

		[CandidName("kills")]
		public UnboundedUInt Kills { get; set; }

		[CandidName("playerId")]
		public Playerid1 PlayerId { get; set; }

		[CandidName("secRemaining")]
		public UnboundedUInt SecRemaining { get; set; }

		[CandidName("wonGame")]
		public bool WonGame { get; set; }

		[CandidName("xpEarned")]
		public UnboundedUInt XpEarned { get; set; }

		public PlayerStats(UnboundedUInt botDifficulty, UnboundedUInt botMode, UnboundedUInt characterID, UnboundedUInt damageCritic, UnboundedUInt damageDealt, UnboundedUInt damageEvaded, UnboundedUInt damageTaken, UnboundedUInt deploys, UnboundedUInt energyChargeRate, UnboundedUInt energyGenerated, UnboundedUInt energyUsed, UnboundedUInt energyWasted, UnboundedUInt faction, UnboundedUInt gameMode, UnboundedUInt kills, Playerid1 playerId, UnboundedUInt secRemaining, bool wonGame, UnboundedUInt xpEarned)
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
			this.PlayerId = playerId;
			this.SecRemaining = secRemaining;
			this.WonGame = wonGame;
			this.XpEarned = xpEarned;
		}

		public PlayerStats()
		{
		}
	}
}