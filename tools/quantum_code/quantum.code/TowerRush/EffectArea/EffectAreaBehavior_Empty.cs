using System.Diagnostics;

namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct EffectAreaBehavior_Empty
	{
		public void Initialize(byte level)
		{
			//No hay daño
		}

		public void ProcessEffect(Frame frame, EntityRef sourceEntity, EntityRef entity, EntityRef target)
		{
			//No hacer nada
		}
	}
}
