namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct Weapon
	{
		// PUBLIC METHODS

		public void Initialize(Frame frame, UnitSettings settings, PlayerRef owner, byte level)
		{
			Owner = owner;

			var context = default(PrototypeMaterializationContext);
			settings.WeaponBehavior.Materialize(frame, ref Behavior, context);

			Level            = level;
			AttackRange      = settings.AttackRange;
			StartAttackDelay = settings.StartAttackDelay;
			AttackSpeed      = settings.AttackSpeed;
		}

		public void Update(Frame frame, EntityRef entity)
		{
			if (Enabled == false)
				return;

			if (frame.Unsafe.TryGetPointer<UnitStats>(entity, out var stats) == true)
			{
				StateTimer -= frame.DeltaTime * stats->GetFinalValue(frame, EStatType.AttackSpeed);
			}
			else
			{
				StateTimer -= frame.DeltaTime;
			}

			if (PendingTarget != Target && frame.Exists(PendingTarget) == true && frame.DestroyPending(PendingTarget) == false)
			{
				var changeTarget = false;

				if (frame.Exists(Target) == false || frame.DestroyPending(Target) == true)
				{
					changeTarget = true;
				}
				else if (frame.Unsafe.GetPointer<Health>(Target)->IsAlive == false || IsInRange(frame, entity, Target) == false)
				{
					changeTarget = true;
				}

				if (changeTarget == true)
				{
					Target     = PendingTarget;
					TargetSize = frame.Unsafe.GetPointer<Target>(Target)->Size;

					if (State == EWeaponState.AttackStart)
					{
						State = EWeaponState.Idle;
					}
				}
			}

			if (frame.Exists(Target) == false || frame.DestroyPending(Target) == true)
			{
				if (State == EWeaponState.AttackStart || StateTimer <= FP._0)
				{
					State = EWeaponState.Idle;
				}

				return;
			}

			bool blockMovement = false;

			if (State == EWeaponState.Idle)
			{
				UpdateIdleState(frame, entity);
			}

			if (State == EWeaponState.AttackStart)
			{
				if (UpdateAttackStartState(frame, entity) == true)
				{
					if (frame.Exists(Target) == false || frame.DestroyPending(Target) == true)
						return;
				}

				blockMovement = true;
			}

			if (State == EWeaponState.AttackCooldown)
			{
				if (IsInRange(frame, entity, Target) == true)
				{
					blockMovement = true;
				}

				if (StateTimer <= FP._0)
				{
					State = EWeaponState.Idle;
				}
			}

			if (blockMovement == true && frame.Unsafe.TryGetPointer<Movement>(entity, out var movement) == true)
			{
				movement->BlockedByWeapon = true;
			}
		}

		public void Stop()
		{
			Enabled = false;
		}

		// PRIVATE METHODS

		private void UpdateIdleState(Frame frame, EntityRef entity)
		{
			if (IsInRange(frame, entity, Target) == false)
				return;

			State      = EWeaponState.AttackStart;
			StateTimer = StartAttackDelay;

			frame.Events.AttackStart(entity);
		}

		private bool UpdateAttackStartState(Frame frame, EntityRef entity)
		{
			if (StateTimer > FP._0)
				return false;

			Behavior.ProcessAttack(frame, Owner, entity, Target, Level);

			StateTimer = AttackSpeed - StartAttackDelay;
			State      = EWeaponState.AttackCooldown;
			return true;
		}

		private bool IsInRange(Frame frame, EntityRef entity, EntityRef target)
		{
			var position        = frame.Unsafe.GetPointer<Transform2D>(entity)->Position;
			var targetPosition  = frame.Unsafe.GetPointer<Transform2D>(target)->Position;
			var attackRangeSqr  = TargetSize + AttackRange;
			    attackRangeSqr *= attackRangeSqr;

			return (position - targetPosition).SqrMagnitude <= attackRangeSqr;
		}
	}
}
