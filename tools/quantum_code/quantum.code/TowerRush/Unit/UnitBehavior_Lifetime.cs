namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct UnitBehavior_Lifetime
	{
		public void Initialize()
		{
			RemainingTime = Duration;
		}

		public void Update(Frame frame, EntityRef entity)
		{
			if (RemoveHealth == true)
			{
				TickTime -= frame.DeltaTime;

				if (TickTime <= FP._1)
				{
					var health = frame.Unsafe.GetPointer<Health>(entity);
					health->ApplyHealthData(frame, new HealthData()
					{
						Action = EHealthAction.Remove,
						Source = entity,
						Target = entity,
						Value  = health->MaxHealth / Duration,
						HideToStats  = true,
					});

					TickTime += FP._1;
				}
			}
			else
			{
				RemainingTime -= frame.DeltaTime;

				if (RemainingTime <= FP._0)
				{
					frame.Destroy(entity);
				}
			}
		}
	}
}
