namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct BuffBehavior_Stats
	{
		// PUBLIC METHODS

		public void Initialize(Frame frame, EntityRef entity, Buff* buff, byte level)
		{
			var multiplier = FP._1;
			var perLevel   = FP._1 + ValuePerLevelIncrease * FP._0_01;

			for (int i = 1; i < level; i++)
			{
				multiplier *= perLevel;
			}

			var stats = frame.Unsafe.GetPointer<UnitStats>(buff->Target);
			stats->AddModifier(frame, StatType, entity, AbsoluteValue * multiplier, PercentValue * multiplier);
		}

		public void Deinitialize(Frame frame, EntityRef entity, Buff* buff)
		{
			var stats = frame.Unsafe.GetPointer<UnitStats>(buff->Target);
			stats->RemoveModifier(frame, StatType, entity);
		}
	}
}
