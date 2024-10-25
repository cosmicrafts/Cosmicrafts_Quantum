namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct UnitStat
	{
		// PUBLIC MEMBERS

		public bool IsDirty         { get {  return Flags.IsBitSet(0); } set { Flags = Flags.SetBit(0, value); } }
		public bool AdditivePercent { get {  return Flags.IsBitSet(1); } set { Flags = Flags.SetBit(1, value); } }

		// PUBLIC METHODS

		public void Initialize(Frame frame)
		{
			StatModifiers = frame.AllocateList<StatModifier>();
		}

		public void Deinitialize(Frame frame)
		{
			frame.FreeList(StatModifiers);
			StatModifiers = default;
		}

		public void Recalculate(Frame frame)
		{
			var absoluteValue  = FP._0;
			var percentValue   = FP._1;

			var modifiers = frame.ResolveList(StatModifiers);

			for (int idx = 0, count = modifiers.Count; idx < count; idx++)
			{
				var modifier = modifiers.GetPointer(idx);

				absoluteValue += modifier->AbsoluteValue;
				percentValue  += modifier->PercentValue;
			}

			percentValue  = FPMath.Max(percentValue,  FP._0);

			if (AdditivePercent == true)
			{
				FinalValue  = BaseValue + absoluteValue - FP._1; // -1 for percent being additive instead of multiplicative
				FinalValue += percentValue;
			}
			else
			{
				FinalValue  = BaseValue + absoluteValue;
				FinalValue *= percentValue;
			}

			IsDirty = false;
		}
	}
}
