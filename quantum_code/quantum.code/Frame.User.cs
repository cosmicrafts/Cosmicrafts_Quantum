namespace Quantum
{
	using Photon.Deterministic;

	unsafe partial class Frame
	{
		// PUBLIC METHODS

		public EntityRef CreateEntity(AssetRefEntityPrototype prototype, FPVector2 position, FP rotation)
		{
			var spawnedEntity = Create(prototype);

			var transform       = Unsafe.GetPointer<Transform2D>(spawnedEntity);
			transform->Position = position;
			transform->Rotation = rotation;

			if (Unsafe.TryGetPointer<Movement>(spawnedEntity, out var movement) == true)
			{
				movement->DesiredPosition  = position;
				movement->MovePosition     = position;
				movement->LookPosition     = position + transform->Forward;
			}

			return spawnedEntity;
		}

		public void SpawnCard(CardSettings settings, PlayerRef owner, FPVector2 position, FP rotation, byte level)
		{
			if (settings is UnitSettings unitSettings)
			{
				for (int idx = 0; idx < unitSettings.UnitCount; idx++)
				{
					var unitEntity = CreateEntity(unitSettings.Prefab, TransformPosition(position, unitSettings.UnitCount, idx), rotation);
					var unit       = Unsafe.GetPointer<Unit>(unitEntity);
					
					unit->Initialize(this, owner, unitEntity, unitSettings, level);

				}
			}
			else if (settings is EffectAreaSettings areaSettings)
			{
				var effectEntity = CreateEntity(areaSettings.Prefab, position, rotation);
				var effectArea   = Unsafe.GetPointer<EffectArea>(effectEntity);

				var behaviors         = AllocateList<EffectAreaBehavior>(System.Math.Max(1, areaSettings.Behaviors.SafeLength()));
				effectArea->Behaviors = behaviors;

				for (int idx = 0, count = areaSettings.Behaviors.SafeLength(); idx < count; idx++)
				{
					var behavior = new EffectAreaBehavior();
					areaSettings.Behaviors[idx].Materialize(this, ref behavior, default);
					behaviors.Add(behavior);
				}

				effectArea->Radius     = areaSettings.Radius;
				effectArea->TickCount  = areaSettings.TickCount;
				effectArea->TickTime   = areaSettings.TickTime;
				effectArea->StateTime  = areaSettings.ActivationDelay;
				effectArea->TargetType = areaSettings.Target;
				effectArea->Owner      = owner;
				effectArea->Level      = level;
			}
		}

		private FPVector2 TransformPosition(FPVector2 position, int unitCount, int unitIndex)
		{
			if (unitCount == 1)
				return position;

			if (unitCount <= 4)
			{
				var rotationOffset = FP.Rad_180 * FP._2 / unitCount * unitIndex;
				position += FPVector2.Rotate(FPVector2.Right * FP._0_50, rotationOffset);
			}

			if (unitCount <= 12)
			{
				if (unitIndex < 4)
				{
					var rotationOffset = FP.Rad_180 * FP._2 / 4 * unitIndex;
					position += FPVector2.Rotate(FPVector2.Right * FP._0_50, rotationOffset);
				}
				else
				{
					var rotationOffset = FP.Rad_180 * FP._2 / (unitCount - 4) * unitIndex;
					position += FPVector2.Rotate(FPVector2.Right, rotationOffset);
				}
			}

			return position;
		}
	}
}
