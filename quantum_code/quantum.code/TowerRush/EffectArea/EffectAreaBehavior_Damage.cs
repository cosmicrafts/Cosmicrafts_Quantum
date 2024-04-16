using System.Diagnostics;

namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct EffectAreaBehavior_Damage
	{
		public void Initialize(byte level)
		{
			var perLevel = FP._1 + DamagePerLevelPercent * FP._0_01;

			for (int idx = 1; idx < level; idx++)
			{
				Damage *= perLevel;
			}
		}

		public void ProcessEffect(Frame frame, EntityRef sourceEntity, EntityRef entity, EntityRef target)
		{
			//SourceEntity = Spaceship owner of Effect
			// Entity = EntityGameobject of Effect
			
			var position   = frame.Unsafe.GetPointer<Transform2D>(entity)->Position;
			var healthData = new HealthData()
			{
				Action = EHealthAction.Remove,
				Source = sourceEntity,
				Target = target,
				Value  = Damage,
			};
			
			var targetHealth = frame.Unsafe.GetPointer<Health>(target);
			targetHealth->ApplyHealthData(frame, healthData);
		}
	}
}
