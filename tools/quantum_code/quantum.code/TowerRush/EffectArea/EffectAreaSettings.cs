namespace Quantum
{
	using Quantum.Prototypes;
	using Quantum.Inspector;
	using Photon.Deterministic;

	[System.Serializable]
	public class EffectAreaSettings : CardSettings
	{
		[Header("Effect Area")]
		public FP                             TickTime;
		public byte                           TickCount;
		public FP                             Radius;
		public EEffectAreaTarget              Target;
		[Space]
		public EffectAreaBehavior_Prototype[] Behaviors;
	}
}
