namespace Quantum
{
	unsafe sealed class UnitStatsSystem : SystemMainThread, ISignalOnComponentAdded<UnitStats>, ISignalOnComponentRemoved<UnitStats>
	{
		// SystemMainThread INTERFACE

		public override void Update(Frame frame)
		{
			foreach (var pair in frame.Unsafe.GetComponentBlockIterator<UnitStats>())
			{
				pair.Component->Update(frame, pair.Entity);
			}
		}

		// ISignalOnComponentAdded INTERFACE

		void ISignalOnComponentAdded<UnitStats>.OnAdded(Frame frame, EntityRef entity, UnitStats* stats)
		{
			stats->Initialize(frame);
		}

		// ISignalOnComponentRemoved INTERFACE

		void ISignalOnComponentRemoved<UnitStats>.OnRemoved(Frame frame, EntityRef entity, UnitStats* stats)
		{
			stats->Deinitialize(frame);
		}
	}
}
