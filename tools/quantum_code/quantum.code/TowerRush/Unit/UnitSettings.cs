namespace Quantum
{
	using Photon.Deterministic;
	using Quantum.Inspector;
	using Quantum.Prototypes;

	[System.Serializable]
	public class UnitSettings : CardSettings
	{
		[Header("Unit")]
		public int                      UnitCount = 1;
		public UnitBehavior_Prototype[] UnitBehaviors;

		[Space]
		[Header("Health")]
		public FP                       BaseHealth;
		public FP                       BaseShield;
		public FP                       HealthPerLevelPercent;
		public bool                     DestroyOnDeath = true;
		
		[Header("Health")]
		public FP                       Critic;
		public FP                       Evasion;

		[Header("Target")]
		public ETargetType              TargetType;
		public FP                       UnitSize;

		[Header("AI")]
		public ETargetTypeFlags         AttackTarget;

		[Header("Weapon")]
		public WeaponBehavior_Prototype WeaponBehavior;
		[Space]
		public FP                       AttackRange;
		public FP                       AttackSpeed;
		public FP                       StartAttackDelay;

		[Header("Movement")]
		public FP                       MovementSpeed;
	}
}
