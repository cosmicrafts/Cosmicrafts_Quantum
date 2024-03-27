﻿namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct Health
	{
		// PUBLIC MEMBERS

		public bool IsInvulnerable { get { return Flags.IsBitSet(0); } set { Flags = Flags.SetBit(0, value); } }
		public bool IsAlive        { get { return CurrentHealth > FP._0; } }

		// PUBLIC METHODS

		public void Initialize(UnitSettings settings, byte level)
		{
			MaxHealth    = settings.BaseHealth;
			var perLevel = FP._1 + settings.HealthPerLevelPercent * FP._0_01;

			for (int idx = 1; idx < level; idx++)
			{
				MaxHealth *= perLevel;
			}

			CurrentHealth = MaxHealth;
		}

		public void ForceKill(Frame frame, EntityRef entity)
		{
			IsInvulnerable = false;
			CurrentHealth  = FP._0;

			frame.Signals.OnDeath(entity, default);
			frame.Events.Death(entity);
		}

		public bool ApplyHealthData(Frame frame, HealthData data)
		{
			if (IsAlive == false)
				return false;
			
			if (frame.Unsafe.TryGetPointer<Unit>(data.Target, out var unit))
			{
				data.TargetOwner = unit-> Owner;
			}
			else { data.TargetOwner = default; }
			
			
			switch (data.Action)
			{
				case EHealthAction.Add:    return AddHealth(frame, data);
				case EHealthAction.Remove: return RemoveHealth(frame, data);
				default:
					return false;
			}
		}

		// PRIVATE MEMBERS

		private bool AddHealth(Frame frame, HealthData data)
		{
			if (CurrentHealth >= MaxHealth)
				return false;

			frame.Events.OnHealthChanged(data);

			CurrentHealth = FPMath.Min(CurrentHealth + data.Value, MaxHealth);
			return true;
		}

		private bool RemoveHealth(Frame frame, HealthData data)
		{
			if (IsInvulnerable == true)
				return false;

			CurrentHealth = FPMath.Max(CurrentHealth - data.Value, FP._0);

			frame.Events.OnHealthChanged(data);

			if (IsAlive == false)
			{
				frame.Signals.OnDeath(data.Target, data.Source);
				frame.Events.Death(data.Target);
			}

			return true;
		}
	}
}
