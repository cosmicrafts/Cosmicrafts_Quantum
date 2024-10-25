namespace Quantum
{
	unsafe class UnitSystem : SystemMainThread, ISignalOnDeath
	{
		public override void Update(Frame frame)
		{
			foreach (var pair in frame.Unsafe.GetComponentBlockIterator<Unit>())
			{
				pair.Component->Update(frame, pair.Entity);
			}
		}

		// ISignalOnDeath INTERFACE

		void ISignalOnDeath.OnDeath(Frame frame, EntityRef entity, EntityRef killer)
		{
			if (frame.Unsafe.TryGetPointer<Unit>(entity, out var unit) == false || unit->DestroyOnDeath == false)
				return;

			unit->OnDeath(frame, entity);

			frame.Destroy(entity);
		}
	}
}
