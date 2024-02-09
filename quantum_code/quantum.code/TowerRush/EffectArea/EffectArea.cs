namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct EffectArea
	{
		public void Update(Frame frame, EntityRef entity)
		{
			StateTime -= frame.DeltaTime;

			if (State == EEffectAreaState.Init)
			{
				if (StateTime <= FP._0)
				{
					State     = EEffectAreaState.Active;
					TickCount = System.Math.Max((byte)1, TickCount);
			
					var behaviors = frame.ResolveList(Behaviors);

					for (int idx = 0, count = behaviors.Count; idx < count; idx++)
					{
						behaviors.GetPointer(idx)->Initialize(Level);
					}
				}
			}

			if (State == EEffectAreaState.Active)
			{
				if (StateTime <= FP._0)
				{
					TickCount -= 1;
					ProcessEffect(frame, entity);

					StateTime = TickTime;
				}

				if (TickCount == 0)
				{
					State     = EEffectAreaState.Finished;
					StateTime = FP._0_50;
				}
			}

			if (State == EEffectAreaState.Finished)
			{
				if (StateTime <= FP._0)
				{
					frame.Destroy(entity);
				}
			}
		}

		private void ProcessEffect(Frame frame, EntityRef entity)
		{
			var position  = frame.Unsafe.GetPointer<Transform2D>(entity)->Position;
			var behaviors = frame.ResolveList(Behaviors);

			foreach (var targetPair in frame.Unsafe.GetComponentBlockIterator<Target>())
			{
				if (TargetType == EEffectAreaTarget.Enemy && targetPair.Component->OwnerPlayerRef == Owner)
					continue;
				if (TargetType == EEffectAreaTarget.Friendly && targetPair.Component->OwnerPlayerRef != Owner)
					continue;

				var targetPosition = frame.Unsafe.GetPointer<Transform2D>(targetPair.Entity)->Position;
				var distance       = (targetPosition - position).SqrMagnitude;

				var radiusSqr  = Radius + targetPair.Component->Size;
				    radiusSqr *= radiusSqr;

				if (distance > radiusSqr)
					continue;

				for (int idx = 0, count = behaviors.Count; idx < count; idx++)
				{
					behaviors.GetPointer(idx)->ProcessEffect(frame, entity, targetPair.Entity, Level);
				}
			}
		}
	}
}
