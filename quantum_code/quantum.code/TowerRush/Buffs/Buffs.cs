namespace Quantum
{
	using Quantum.Collections;

	public unsafe partial struct Buffs
	{
		// PUBLIC METHODS

		public void Initialize(Frame frame)
		{
			BuffList = frame.AllocateList<EntityRef>();
		}

		public void Deinitalize(Frame frame)
		{
			var buffs = frame.ResolveList(BuffList);
			for (int idx = buffs.Count; idx --> 0;)
			{
				var buffEntity = buffs[idx];
				var buff       = frame.Unsafe.GetPointer<Buff>(buffEntity);

				buff->Deinitialize(frame, buffEntity, buff);
			}

			frame.FreeList(BuffList);
			BuffList = default;
		}

		public void Update(Frame frame, EntityRef entity)
		{
			var buffs = frame.ResolveList(BuffList);
			for (int idx = buffs.Count; idx --> 0;)
			{
				var buffEntity = buffs[idx];
				var buff       = frame.Unsafe.GetPointer<Buff>(buffEntity);

				buff->Update(frame, buff);

				if (buff->IsFinished == true)
				{
					buff->Deinitialize(frame, buffEntity, buff);
					buffs.RemoveAtUnordered(idx);

					frame.Destroy(buffEntity);
				}
			}
		}

		public bool AddBuff(Frame frame, EntityRef owner, EntityRef target, AssetRefEntityPrototype prototype, byte level)
		{
			var buffs = frame.ResolveList(BuffList);

			if (TryRefresh(frame, owner, prototype.Id.Value, buffs) == true)
				return true;

			var buffEntity = frame.Create(prototype);
			var buff       = frame.Unsafe.GetPointer<Buff>(buffEntity);

			buff->ID     = prototype.Id.Value;
			buff->Owner  = owner;
			buff->Target = target;

			buff->Initialize(frame, buffEntity, buff, level);

			buffs.Add(buffEntity);

			return true;
		}

		public bool RemoveBuff(Frame frame, EntityRef owner, AssetRefEntityPrototype prototype)
		{
			var buffID = prototype.Id.Value;
			var buffs  = frame.ResolveList(BuffList);

			for (int idx = buffs.Count; idx --> 0;)
			{
				var buffEntity = buffs[idx];
				var buff       = frame.Unsafe.GetPointer<Buff>(buffEntity);
				
				if (buff->ID != buffID)
					continue;
				if (buff->Owner != owner)
					continue;

				buff->Deinitialize(frame, buffEntity, buff);

				buffs.RemoveAtUnordered(idx);
				frame.Destroy(buffEntity);

				return true;
			}

			return false;
		}

		public bool RemoveBuff(Frame frame, EntityRef buffEntity)
		{
			var buffs = frame.ResolveList(BuffList);

			for (int idx = buffs.Count; idx --> 0;)
			{
				if (buffs[idx] == buffEntity)
				{
					var buff = frame.Unsafe.GetPointer<Buff>(buffEntity);
					buff->Deinitialize(frame, buffEntity, buff);

					buffs.RemoveAtUnordered(idx);
					frame.Destroy(buffEntity);
					return true;
				}
			}

			return false;
		}

		public void RemoveAll(Frame frame)
		{
			var buffs = frame.ResolveList(BuffList);

			for (int idx = buffs.Count; idx --> 0;)
			{
				var buffEntity = buffs[idx];
				var buff       = frame.Unsafe.GetPointer<Buff>(buffEntity);
				buff->Deinitialize(frame, buffEntity, buff);

				frame.Destroy(buffEntity);
			}

			buffs.Clear();
		}

		// PRIVATE MEMBERS

		private bool TryRefresh(Frame frame, EntityRef owner, long buffID, QList<EntityRef> buffs)
		{
			for (int idx = buffs.Count; idx --> 0;)
			{
				var buffEntity = buffs[idx];
				var buff       = frame.Unsafe.GetPointer<Buff>(buffEntity);
				
				if (buff->ID != buffID)
					continue;
				if (buff->Owner != owner)
					continue;

				buff->Refresh(frame, buffEntity, buff);
				return true;
			}

			return false;
		}
	}
}
