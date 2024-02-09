namespace Quantum
{
	using Photon.Deterministic;
	using Quantum.Inspector;

	public abstract partial class CardSettings
	{
		[Header("Card")]
		public AssetRefEntityPrototype  Prefab;
		public ERarity                  Rarity;
		public byte                     EnergyCost;
		public FP                       ActivationDelay;
	}
}
