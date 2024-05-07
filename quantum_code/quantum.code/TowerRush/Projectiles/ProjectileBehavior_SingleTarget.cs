namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct ProjectileBehavior_SingleTarget
	{
		public void FinishProjectile(Frame frame, PlayerRef owner, EntityRef source, EntityRef target, FPVector2 position, byte level)
		{
			
			if (frame.Exists(target) == false || frame.DestroyPending(target) == true)
				return;
			
			//Instance Bullet
			var entity     = frame.CreateEntity(Prototype, position, FP._0);
			var effectArea = frame.Unsafe.GetPointer<EffectArea>(entity);
			effectArea->SourceEntity = source;
			
			var behavior = new EffectAreaBehavior();
			behavior.Empty->Vector = new FP();

			var behaviors = frame.AllocateList<EffectAreaBehavior>(1);
			effectArea->Behaviors = behaviors;
			behaviors.Add(behavior);
			
			effectArea->Radius = FP._0_50;
			effectArea->Owner                  = owner;
			//

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
