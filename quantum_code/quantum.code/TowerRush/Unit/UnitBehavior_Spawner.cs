﻿namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct UnitBehavior_Spawner
	{
		public void Update(Frame frame, PlayerRef owner, EntityRef entity, byte level)
		{
			RemainingSpawnTime -= frame.DeltaTime;

			if (RemainingSpawnTime > FP._0)
				return;

			var transform = frame.Unsafe.GetPointer<Transform2D>(entity);

			var rotation = transform->Rotation;
			var position = transform->Position + FPVector2.Rotate(Offset, rotation);

			var settings = frame.FindAsset<CardSettings>(Card.Id);
			frame.SpawnCard(settings, owner, position, rotation, level);

			RemainingSpawnTime += SpawnTime;
		}
	}
}
