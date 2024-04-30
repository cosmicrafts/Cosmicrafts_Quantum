using System.Diagnostics;

namespace Quantum
{
	public unsafe partial struct WeaponBehavior_Projectile
	{
		public void ProcessAttack(Frame frame, PlayerRef owner, EntityRef sourceEntity, EntityRef targetEntity, byte level)
		{
			
			var cannonTransform = frame.Unsafe.GetPointer<Unit>(sourceEntity)->Cannon;
			Log.Debug("Cannon" + cannonTransform);
			var sourceTransform = frame.Unsafe.GetPointer<Transform2D>(sourceEntity);
			Log.Debug("Source" + sourceTransform->Position);
			var transformDirection = sourceTransform->TransformDirection(cannonTransform);
			Log.Debug("TransformDirection" + transformDirection);
			var newTransform = sourceTransform->Position + transformDirection;
			Log.Debug("newTransform" + newTransform);
			
			
			//var entity          = frame.CreateEntity(Prototype, sourceTransform->Position, sourceTransform->Rotation);
			var entity          = frame.CreateEntity(Prototype, newTransform, sourceTransform->Rotation);
			
			var projectile = frame.Unsafe.GetPointer<Projectile>(entity);

			projectile->Owner          = owner;
			projectile->SourceEntity   = sourceEntity;
			projectile->TargetEntity   = targetEntity;
			projectile->Level          = level;
			projectile->Behavior       = Behavior;
			projectile->Speed          = Speed;

			var targetPosition         = frame.Unsafe.GetPointer<Transform2D>(targetEntity)->Position;
			var direction              = targetPosition - sourceTransform->Position;
			var target                 = frame.Unsafe.GetPointer<Target>(targetEntity);

			// Hit edge of the enemy instead of center
			projectile->TargetPosition = targetPosition - direction.Normalized * target->Size;
		}
	}
}
