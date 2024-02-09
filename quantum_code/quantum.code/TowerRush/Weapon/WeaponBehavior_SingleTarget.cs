namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct WeaponBehavior_SingleTarget
	{
		public void ProcessAttack(Frame frame, EntityRef entity, EntityRef target, byte level)
		{
			var damage   = Damage;
			var perLevel = FP._1 + DamagePerLevelPercent * FP._0_01;
		
			for (int idx = 1; idx < level; idx++)
			{
				Damage *= perLevel;
			}

			var targetHealth = frame.Unsafe.GetPointer<Health>(target);
			targetHealth->ApplyHealthData(frame, new HealthData()
			{
				Action = EHealthAction.Remove,
				Source = entity,
				Target = target,
				Value  = damage,
			});
		}
	}
}
