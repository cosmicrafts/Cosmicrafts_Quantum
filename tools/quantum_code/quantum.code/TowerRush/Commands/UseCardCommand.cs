namespace Quantum
{
	using Photon.Deterministic;

	public class UseCardCommand : DeterministicCommand
	{
		public byte      CardIndex;
		public FPVector2 Position;

		public override void Serialize(BitStream stream)
		{
			stream.Serialize(ref CardIndex);
			stream.Serialize(ref Position);
		}
	}
}
