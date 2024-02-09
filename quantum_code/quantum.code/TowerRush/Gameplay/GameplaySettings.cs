namespace Quantum
{
	using Photon.Deterministic;

	public partial class GameplaySettings
	{
		public AssetRefCardSettings   CastleSettings;
		public AssetRefCardSettings   TowerSettings;
		public AssetRefCardSettings[] AllCards;

		public byte                   StartEnergy;
		public byte                   MaxEnergy;

		public StateSettings[]        States;

		[System.Serializable]
		public class StateSettings
		{
			public EGameplayState State;
			public FP             EnergyFillRate;
			public FP             DurationSeconds;
		}
	}
}
