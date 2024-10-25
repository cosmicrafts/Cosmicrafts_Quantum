namespace Quantum
{
	using Photon.Deterministic;

	partial class RuntimeConfig
	{
		public AssetRefGameplaySettings GameplaySettings;

		partial void SerializeUserData(BitStream stream)
		{
			stream.Serialize(ref GameplaySettings);
		}
	}
}