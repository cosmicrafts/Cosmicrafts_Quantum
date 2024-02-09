namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct BuffBehavior
	{
		// PUBLIC METHODS

		public void Initialize(Frame frame, EntityRef entity, Buff* buff, byte level)
		{
			switch (Field)
			{
				case DURATION:       _Duration.Initialize(level);                    break;
				case HEALTHOVERTIME: _HealthOverTime.Initialize(frame, buff, level); break;
				case STATS:          _Stats.Initialize(frame, entity, buff, level);  break;

				default:
					throw new System.NotImplementedException($"{nameof(BuffBehavior)}.{nameof(Initialize)} not implemented");
			}
		}

		public void Deinitialize(Frame frame, EntityRef entity, Buff* buff)
		{
			switch (Field)
			{
				case STATS: _Stats.Deinitialize(frame, entity, buff); break;

				case DURATION:
				case HEALTHOVERTIME:
					break;

				default:
					throw new System.NotImplementedException($"{nameof(BuffBehavior)}.{nameof(Deinitialize)} not implemented");
			}
		}

		public void Refresh(Buff* buff)
		{
			switch (Field)
			{
				case DURATION:       _Duration.Refresh();       break;
				case HEALTHOVERTIME: _HealthOverTime.Refresh(); break;

				case STATS:
					break;

				default:
					throw new System.NotImplementedException($"{nameof(BuffBehavior)}.{nameof(Refresh)} not implemented");
			}
		}

		public void Update(Frame frame, Buff* buff)
		{
			switch (Field)
			{
				case DURATION:       _Duration.Update(frame, buff);       break;
				case HEALTHOVERTIME: _HealthOverTime.Update(frame, buff); break;

				case STATS:
					break;

				default:
					throw new System.NotImplementedException($"{nameof(BuffBehavior)}.{nameof(Update)} not implemented");
			}
		}

		public (FP, FP) GetDuration()
		{
			switch (Field)
			{
				case DURATION:       return _Duration.GetDuration();
				case HEALTHOVERTIME: return _HealthOverTime.GetDuration();

				case STATS:          return (default, default);

				default:
					throw new System.NotImplementedException($"{nameof(BuffBehavior)}.{nameof(GetDuration)} not implemented");
			}
		}
	}
}
