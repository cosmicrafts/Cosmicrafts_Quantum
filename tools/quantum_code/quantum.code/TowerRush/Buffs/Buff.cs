namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct Buff
	{
		// PUBLIC MEMBERS

		public bool IsFinished { get { return Flags.IsBitSet(0); } set { Flags = Flags.SetBit(0, value); } }

		// PUBLIC METHODS

		public void Initialize(Frame frame, EntityRef entity, Buff* buff, byte level)
		{
			var behaviors = frame.ResolveList(Behaviors);

			for (int idx = 0, count = behaviors.Count; idx < count; idx++)
			{
				behaviors.GetPointer(idx)->Initialize(frame, entity, buff, level);
			}
		}

		public void Deinitialize(Frame frame, EntityRef entity, Buff* buff)
		{
			var behaviors = frame.ResolveList(Behaviors);

			for (int idx = 0, count = behaviors.Count; idx < count; idx++)
			{
				behaviors.GetPointer(idx)->Deinitialize(frame, entity, buff);
			}
		}

		public void Refresh(Frame frame, EntityRef entity, Buff* buff)
		{
			var behaviors = frame.ResolveList(Behaviors);

			for (int idx = 0, count = behaviors.Count; idx < count; idx++)
			{
				behaviors.GetPointer(idx)->Refresh(buff);
			}
		}

		public void Update(Frame frame, Buff* buff)
		{
			var behaviors = frame.ResolveList(Behaviors);

			for (int idx = 0, count = behaviors.Count; idx < count; idx++)
			{
				behaviors.GetPointer(idx)->Update(frame, buff);
			}
		}

		public (FP, FP) GetDuration(Frame frame)
		{
			var behaviors = frame.ResolveList(Behaviors);

			for (int idx = 0, count = behaviors.Count; idx < count; idx++)
			{
				var (duration, maxDuration) = behaviors[idx].GetDuration();
				if (maxDuration > FP._0)
					return (duration, maxDuration);
			}

			return (default, default);
		}
	}
}
