using System.Diagnostics;

namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct Health
	{
		// PUBLIC MEMBERS

		public bool IsInvulnerable { get { return Flags.IsBitSet(0); } set { Flags = Flags.SetBit(0, value); } }
		public bool IsAlive        { get { return CurrentHealth > FP._0; } }
		public bool HasShield        { get { return CurrentShield > FP._0; } }

		// PUBLIC METHODS

		public void Initialize(UnitSettings settings, byte level)
		{
			MaxHealth    = settings.BaseHealth;
			var perLevel = FP._1 + settings.HealthPerLevelPercent * FP._0_01;

			//Dont up stats from LV
			/*for (int idx = 1; idx < level; idx++)
			{
				MaxHealth *= perLevel;
			}*/
			
			CurrentHealth = MaxHealth;

			MaxShield = settings.BaseShield;
			CurrentShield = MaxShield;
		}

		public void ForceKill(Frame frame, EntityRef entity)
		{
			IsInvulnerable = false;
			CurrentHealth  = FP._0;
			CurrentShield = FP._0;

			frame.Signals.OnDeath(entity, default);
			frame.Events.Death(entity);
		}

		public bool ApplyHealthData(Frame frame, HealthData data)
		{
			data.AttackMode = EAttackMode.None;
			
			if (IsAlive == false)
				return false;

			if (frame.Unsafe.TryGetPointer<Unit>(data.Source, out var unitSource))
			{
				data.SourceTokenID = unitSource-> TokenID;
				
				if (data.Action == EHealthAction.Remove)
				{
					FP randomNum = frame.Global->RngSession.Next((FP)0, (FP)100);

					if ( unitSource->Critic > randomNum )
					{
						data.AttackMode = EAttackMode.Critic;
						data.Value *= FP._1_50;
					}
					
				}
				Log.Debug($"Have UNIT Script In Source: {data.Source}");
			}
			else
			{
				data.SourceTokenID = default; 
				Log.Debug($"Dont Have UNIT Script In Source: {data.Source}");
			}

			data.ValueRefOriginal = data.Value;
			
			////////////
			if (frame.Unsafe.TryGetPointer<Unit>(data.Target, out var unit))
			{
				data.TargetOwner = unit-> Owner;
				data.TargetTokenID = unit-> TokenID;

				if (data.Action == EHealthAction.Remove)
				{
					FP randomNum = frame.Global->RngSession.Next((FP)0, (FP)100);

					if (unit->Evasion > randomNum)
					{
						data.AttackMode = EAttackMode.Evasion;
						data.Value *=  FP._0;
					}
					
				}
			}
			else
			{
				data.TargetOwner = default; data.TargetTokenID = default;
				Log.Debug($"Dont Have UNIT Script In Target");
			}
			
			
			
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

			if (HasShield)
			{
				CurrentShield = FPMath.Max(CurrentShield - data.Value, FP._0);
				//Agregar daño adicioanl a vida después de que supera al escudo
			}
			else
			{
				CurrentHealth = FPMath.Max(CurrentHealth - data.Value, FP._0);
			}
			
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
