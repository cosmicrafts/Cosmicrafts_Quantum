namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct BuffBehavior_HealthOverTime
	{
		// PUBLIC METHODS

		public void Initialize(Frame frame, Buff* buff, byte level)
		{
			var perLevel = FP._1 + ValuePerLevelIncrease * FP._0_01;

			for (int i = 1; i < level; i++)
			{
				ValuePerTick *= perLevel;
			}

			Refresh();

			Tick(frame, buff);
		}

		public void Refresh()
		{
			RemainingTicks = TickCount;
		}

		public void Update(Frame frame, Buff* buff)
		{
			TimeToTick -= frame.DeltaTime;

			if (TimeToTick <= FP._0)
			{
				Tick(frame, buff);
			}
		}

		public (FP, FP) GetDuration()
		{
			var maxDuration       = (TickCount - 1) * TickTime;
			var remainingDuration = (RemainingTicks - 1) * TickTime + TimeToTick;

			return (remainingDuration, maxDuration);
		}

		// PRIVATE METHODS

		private void Tick(Frame frame, Buff* buff)
		{
			TimeToTick += TickTime;

			var targetHealth  = frame.Unsafe.GetPointer<Health>(buff->Target);

			var healthData = new HealthData
			{
				Action   = Action,
				Value    = ValuePerTick,
				Source   = buff->Owner,
				Target   = buff->Target,
			};

			targetHealth->ApplyHealthData(frame, healthData);

			if (TickCount > 0)
			{
				RemainingTicks   -= 1;
				buff->IsFinished  = RemainingTicks == 0;
			}
		}
	}
}
