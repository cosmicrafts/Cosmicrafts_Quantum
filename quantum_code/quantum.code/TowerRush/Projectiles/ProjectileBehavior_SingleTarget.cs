namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct ProjectileBehavior_SingleTarget
	{
		public void FinishProjectile(Frame frame, EntityRef source, EntityRef target, byte level)
		{
			if (frame.Exists(target) == false || frame.DestroyPending(target) == true)
				return;

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
				Source = source,
				Target = target,
				Value  = damage,
			});
		}
	}
}
