namespace Quantum
{
	using System;
	using Photon.Deterministic;

	public unsafe partial struct Unit
	{
		public void Initialize(Frame frame, PlayerRef owner, EntityRef entity, UnitSettings settings, byte level, int tokenID)
		{
			Log.Debug("CardSpawned-Entity: " + entity.Index);
			Owner           = owner;
			TokenID         = tokenID;
			Level           = level;

			Critic          = settings.Critic;
			Evasion         = settings.Evasion;
			
			DestroyOnDeath  = settings.DestroyOnDeath;
			ActivationDelay = settings.ActivationDelay;
			Settings        = settings;

			var behaviors = frame.AllocateList<UnitBehavior>(Math.Max(1, settings.UnitBehaviors.SafeLength()));
			Behaviors     = behaviors;

			for (int idx = 0, count = settings.UnitBehaviors.SafeLength(); idx < count; idx++)
			{
				var behavior = new UnitBehavior();
				settings.UnitBehaviors[idx].Materialize(frame, ref behavior, default);
				behavior.Initialize();
				behaviors.Add(behavior);
			}

			if (frame.Unsafe.TryGetPointer<Target>(entity, out var target) == true)
			{
				target->Initialize(settings, owner);
			}

			if (frame.Unsafe.TryGetPointer<Health>(entity, out var health) == true)
			{
				health->Initialize(settings, level);
			}

			if (frame.Unsafe.TryGetPointer<Movement>(entity, out var movement) == true)
			{
				movement->Initialize(frame, entity, settings);
			}

			if (frame.Unsafe.TryGetPointer<UnitAI>(entity, out var unitAI) == true)
			{
				unitAI->Initialize(settings, owner);
			}

			if (frame.Unsafe.TryGetPointer<Weapon>(entity, out var weapon) == true)
			{
				weapon->Initialize(frame, settings, owner, level);
			}

			if (ActivationDelay <= FP._0)
			{
				Activate(frame, entity);
			}
		}

		public void Update(Frame frame, EntityRef entity)
		{
			// IsInitialized
			if (Settings.Id.IsValid == false)
				return;

			if (ActivationDelay <= FP._0)
			{
				UpdateActiveState(frame, entity);
				return;
			}

			ActivationDelay -= frame.DeltaTime;

			if (ActivationDelay <= FP._0)
			{
				Activate(frame, entity);
			}
		}

		public void OnDeath(Frame frame, EntityRef entity)
		{
			var behaviors = frame.ResolveList(Behaviors);

			for (int idx = 0, count = behaviors.Count; idx < count; idx++)
			{
				behaviors.GetPointer(idx)->OnDeath(frame, Owner, entity, Level);
			}
		}

		private void Activate(Frame frame, EntityRef entity)
		{
			if (frame.Unsafe.TryGetPointer<UnitAI>(entity, out var unitAI) == true)
			{
				unitAI->Enabled = true;
			}

			if (frame.Unsafe.TryGetPointer<Weapon>(entity, out var weapon) == true)
			{
				weapon->Enabled = true;
			}
		}

		private void UpdateActiveState(Frame frame, EntityRef entity)
		{
			var behaviors = frame.ResolveList(Behaviors);

			for (int idx = 0, count = behaviors.Count; idx < count; idx++)
			{
				behaviors.GetPointer(idx)->Update(frame, Owner, entity, Level);
			}
		}
	}
}
