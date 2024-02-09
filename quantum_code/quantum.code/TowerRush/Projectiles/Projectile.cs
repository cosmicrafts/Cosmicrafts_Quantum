using Photon.Deterministic;

namespace Quantum
{
	public unsafe partial struct Projectile
	{
		public void Update(Frame frame, EntityRef entity)
		{
			var transform = frame.Unsafe.GetPointer<Transform2D>(entity);
			var direction = TargetPosition - transform->Position;
			var move      = direction.Normalized * Speed * frame.DeltaTime;
			var finished  = false;

			if (move.SqrMagnitude >= direction.SqrMagnitude)
			{
				move     = direction;
				finished = true;
			}

			transform->Position += move;
			transform->Rotation = FPVector2.RadiansSigned(FPVector2.Up, direction);

			if (finished == true)
			{
				Behavior.FinishProjectile(frame, Owner, SourceEntity, TargetEntity, TargetPosition, Level);
				frame.Destroy(entity);
			}
		}
	}
}
