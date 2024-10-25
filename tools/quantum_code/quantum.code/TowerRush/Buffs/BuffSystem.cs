namespace Quantum
{
	public unsafe class BuffSystem : SystemMainThread, ISignalOnComponentAdded<Buffs>, ISignalOnComponentRemoved<Buffs>, ISignalOnDeath
	{
		// SystemMainThread INTERFACE

		public override void Update(Frame frame)
		{
			foreach (var pair in frame.Unsafe.GetComponentBlockIterator<Buffs>())
			{
				pair.Component->Update(frame, pair.Entity);
			}
		}

		// ISignalOnComponentAdded INTERFACE

		void ISignalOnComponentAdded<Buffs>.OnAdded(Frame frame, EntityRef entity, Buffs* buffs)
		{
			buffs->Initialize(frame);
		}

		// ISignalOnComponentRemoved INTERFACE

		void ISignalOnComponentRemoved<Buffs>.OnRemoved(Frame frame, EntityRef entity, Buffs* buffs)
		{
			buffs->Deinitalize(frame);
		}

		// ISignalOnDeath INTERFACE

		void ISignalOnDeath.OnDeath(Frame frame, EntityRef entity, EntityRef killer)
		{
			if (frame.Unsafe.TryGetPointer<Buffs>(entity, out var buffs) == false)
				return;

			buffs->RemoveAll(frame);
		}
	}
}
