using System.Globalization;

namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct ProjectileBehavior_AoE
	{
		public void FinishProjectile(Frame frame, PlayerRef owner, EntityRef source, FPVector2 position, byte level)
		{
			var entity     = frame.CreateEntity(Prototype, position, FP._0);
			var effectArea = frame.Unsafe.GetPointer<EffectArea>(entity);
			effectArea->SourceEntity = source;
			
			var behavior = new EffectAreaBehavior();
			behavior.Damage->Damage                = Damage;
			behavior.Damage->DamagePerLevelPercent = DamagePerLevelPercent;

			var behaviors = frame.AllocateList<EffectAreaBehavior>(1);
			effectArea->Behaviors = behaviors;
			behaviors.Add(behavior);

			effectArea->Radius                 = Radius;
			effectArea->Owner                  = owner;
		}
	}
}
