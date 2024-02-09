namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct BuffBehavior_Duration
	{
		// PUBLIC METHODS

		public void Initialize(byte level)
		{
			var perLevel = FP._1 + DurationPerLevelIncrease * FP._0_01;

			for (int i = 1; i < level; i++)
			{
				MaxDuration *= perLevel;
			}

			Refresh();
		}

		public void Refresh()
		{
			Duration = MaxDuration;
		}

		public void Update(Frame frame, Buff* buff)
		{
			Duration -= frame.DeltaTime;

			if (Duration < FP._0)
			{
				buff->IsFinished = true;
			}
		}

		public (FP, FP) GetDuration()
		{
			return (Duration, MaxDuration);
		}
	}
}
