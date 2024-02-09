namespace Quantum
{
	using Photon.Deterministic;

	unsafe class MovementSystem : SystemMainThread, ISignalOnNavMeshMoveAgent, ISignalOnDeath
	{
		// SystemMainThread INTERFACE

		public override void Update(Frame frame)
		{
			foreach (var pair in frame.Unsafe.GetComponentBlockIterator<Movement>())
			{
				pair.Component->Update(frame, pair.Entity);
			}
		}

		// ISignalOnDeath INTERFACE

		void ISignalOnDeath.OnDeath(Frame frame, EntityRef entity, EntityRef killer)
		{
			if (frame.Unsafe.TryGetPointer<Movement>(entity, out var movement) == false)
				return;
			
			movement->Stop(frame, entity);
		}

		// ISignalOnNavMeshMoveAgent INTERFACE

		void ISignalOnNavMeshMoveAgent.OnNavMeshMoveAgent(Frame frame, EntityRef entity, FPVector2 desiredDirection)
		{
			var movement = frame.Unsafe.GetPointer<Movement>(entity);
			movement->OnNavMeshMoveAgent(frame, entity, desiredDirection);
		}
	}
}
