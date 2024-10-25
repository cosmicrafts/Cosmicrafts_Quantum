namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct WeaponBehavior_SingleTarget
	{
		public void ProcessAttack(Frame frame, PlayerRef owner, EntityRef sourceEntity, EntityRef targetEntity, byte level)
		{
			
			var targetPosition = frame.Unsafe.GetPointer<Transform2D>(targetEntity)->Position;
			
			//Instance Impact
			var entityImpact     = frame.CreateEntity(PrototypeImpact, targetPosition, FP._0);
			var effectArea = frame.Unsafe.GetPointer<EffectArea>(entityImpact);
			effectArea->SourceEntity = sourceEntity;
			
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

			var targetHealth = frame.Unsafe.GetPointer<Health>(targetEntity);
			targetHealth->ApplyHealthData(frame, new HealthData()
			{
				Action = EHealthAction.Remove,
				Source = sourceEntity,
				Target = targetEntity,
				Value  = damage,
			});
		}
	}
}
