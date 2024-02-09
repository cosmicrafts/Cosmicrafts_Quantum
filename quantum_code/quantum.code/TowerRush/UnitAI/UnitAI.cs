namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct UnitAI
	{
		// CONSTANTS

		public static FP TARGETING_RANGE_SQR = FP._5 * FP._5;

		// PUBLICMETHODS

		public void Initialize(UnitSettings settings, PlayerRef owner)
		{
			Owner          = owner;
			TargetingFlags = settings.AttackTarget;
		}

		public void Stop()
		{
			Enabled = false;
		}

		public void Update(Frame frame, EntityRef entity)
		{
			if (Enabled == false)
				return;

			TargetingUpdateTimer -= frame.DeltaTime;

			if (frame.Exists(Target) == false || frame.DestroyPending(Target) == true)
			{
				Target = EntityRef.None;
			}
			else if (frame.Unsafe.TryGetPointer<Health>(Target, out var targetHealth) == true && targetHealth->IsAlive == false)
			{
				Target = EntityRef.None;
			}

			if (TargetingUpdateTimer > FP._0 && Target != EntityRef.None)
			{
				if (frame.Unsafe.TryGetPointer<Movement>(entity, out var targetMovement) == true)
				{
					targetMovement->MovePosition = frame.Unsafe.GetPointer<Transform2D>(Target)->Position;
				}

				return;
			}

			var position = frame.Unsafe.GetPointer<Transform2D>(entity)->Position;

			var targetPosition   = default(FPVector2);
			var targetEntity     = EntityRef.None;
			var targetDistance   = FP.MaxValue;

			foreach (var targetPair in frame.Unsafe.GetComponentBlockIterator<Target>())
			{
				if (targetPair.Component->OwnerPlayerRef == Owner)
					continue;

				var targetTypeFlag = targetPair.Component->Type.ConvertToFlags();
				if ((TargetingFlags & targetTypeFlag) == 0)
					continue;

				var targetHealth = frame.Unsafe.GetPointer<Health>(targetPair.Entity);
				if (targetHealth->IsAlive == false)
					continue;

				var targetPosition2 = frame.Unsafe.GetPointer<Transform2D>(targetPair.Entity)->Position;
				var distanceSqr     = (position - targetPosition2).SqrMagnitude;

				if (targetDistance > distanceSqr)
				{
					targetDistance = distanceSqr;
					targetEntity   = targetPair.Entity;
					targetPosition = targetPosition2;
				}
			}

			Target = targetEntity;

			if (targetDistance < TARGETING_RANGE_SQR)
			{
				TargetingUpdateTimer = FP._100;
			}
			else
			{
				TargetingUpdateTimer = FP._0_50;
			}

			if (frame.Unsafe.TryGetPointer<Weapon>(entity, out var weapon) == true)
			{
				weapon->PendingTarget = Target;
			}

			if (frame.Unsafe.TryGetPointer<Movement>(entity, out var movement) == true)
			{
				movement->MovePosition = targetPosition;
			}
		}
	}
}
