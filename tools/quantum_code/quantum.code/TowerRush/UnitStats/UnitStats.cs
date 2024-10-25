namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct UnitStats
	{
		// PUBLIC MEMBERS

		public bool IsDirty { get { return Flags.IsBitSet(0); } set { Flags = Flags.SetBit(0, value); } }

		// PUBLIC METHODS

		public void Initialize(Frame frame)
		{
			StatDictionary = frame.AllocateDictionary<int, UnitStat>();

			SetBaseValue(frame, EStatType.AttackSpeed,   FP._1)->AdditivePercent = true;
			SetBaseValue(frame, EStatType.MovementSpeed, FP._1)->AdditivePercent = true;

			IsDirty = true;
		}

		public void Deinitialize(Frame frame)
		{
			frame.FreeDictionary(StatDictionary);
			StatDictionary = default;
		}

		public void Update(Frame frame, EntityRef entity)
		{
			if (IsDirty == false)
				return;

			var stats       = frame.ResolveDictionary(StatDictionary);
			var changedMask = default(long);

			foreach (var pair in stats)
			{
				if (pair.Value.IsDirty == true)
				{
					var value = pair.Value;
					value.Recalculate(frame);
					stats[pair.Key] = value;

					changedMask = changedMask.SetBit(pair.Key, true);
				}
			}

			frame.Signals.StatsUpdated(entity, changedMask);
			IsDirty = false;
		}

		public FP GetFinalValue(Frame frame, EStatType statType)
		{
			var stats = frame.ResolveDictionary(StatDictionary);
			if (stats.TryGetValuePointer((int)statType, out var stat) == true)
				return stat->FinalValue;

			return default;
		}

		public UnitStat* SetBaseValue(Frame frame, EStatType statType, FP value)
		{
			var stats = frame.ResolveDictionary(StatDictionary);
			if (stats.TryGetValuePointer((int)statType, out var stat) == false)
			{
				var newStat = new UnitStat()
				{
					BaseValue = value,
					IsDirty   = true,
				};

				newStat.Initialize(frame);

				stats.Add((int)statType, newStat);
				IsDirty = true;
				stats.TryGetValuePointer((int)statType, out stat);
				return stat;
			}
			else
			{
				if (stat->BaseValue == value)
					return stat;

				stat->BaseValue = value;
				stat->IsDirty   = true;
				IsDirty         = true;

				return stat;
			}
		}

		public void AddModifier(Frame frame, EStatType statType, EntityRef owner, FP absoluteValue, FP percentValue)
		{
			var stats = frame.ResolveDictionary(StatDictionary);
			if (stats.TryGetValuePointer((int)statType, out var stat) == false)
				return;

			AddModifier(frame, owner, stat, absoluteValue, percentValue);
		}

		public void RemoveAll(Frame frame, EntityRef owner)
		{
			var stats = frame.ResolveDictionary(StatDictionary);
			foreach (var pair in stats)
			{
				stats.TryGetValuePointer(pair.Key, out var stat);

				RemoveModifier(frame, owner, stat);
			}
		}

		public void RemoveModifier(Frame frame, EStatType statType, EntityRef owner)
		{
			var stats = frame.ResolveDictionary(StatDictionary);
			if (stats.TryGetValuePointer((int)statType, out var stat) == false)
				return;

			RemoveModifier(frame, owner, stat);
		}

		// PRIVATE METHODS

		private void AddModifier(Frame frame, EntityRef owner, UnitStat* stat, FP absoluteValue, FP percentValue)
		{
			percentValue /= FP._100;

			var modifiers = frame.ResolveList(stat->StatModifiers);

			for (int idx = 0, count = modifiers.Count; idx < count; idx++)
			{
				var modifier = modifiers.GetPointer(idx);

				if (modifier->Owner == owner)
				{
					if (modifier->AbsoluteValue != absoluteValue || modifier->PercentValue != percentValue)
					{
						modifier->AbsoluteValue = absoluteValue;
						modifier->PercentValue  = percentValue;

						stat->IsDirty = true;
						IsDirty       = true;
					}

					return;
				}
			}

			var newModifier = new StatModifier
			{
				Owner         = owner,
				AbsoluteValue = absoluteValue,
				PercentValue  = percentValue,
			};

			modifiers.Add(newModifier);

			stat->IsDirty = true;
			IsDirty       = true;
		}

		private void RemoveModifier(Frame frame, EntityRef owner, UnitStat* stat)
		{
			var modifiers = frame.ResolveList(stat->StatModifiers);

			for (int idx = 0, count = modifiers.Count; idx < count; idx++)
			{
				if (modifiers.GetPointer(idx)->Owner == owner)
				{
					stat->IsDirty = true;
					IsDirty       = true;

					modifiers.RemoveAtUnordered(idx);
					return;
				}
			}
		}
	}
}
