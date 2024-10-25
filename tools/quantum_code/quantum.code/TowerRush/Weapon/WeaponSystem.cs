namespace Quantum
{
	public unsafe class WeaponSystem : SystemMainThread, ISignalOnDeath
	{
		// SystemMainThread INTERFACE

		public override void Update(Frame frame)
		{
			foreach (var pair in frame.Unsafe.GetComponentBlockIterator<Weapon>())
			{
				pair.Component->Update(frame, pair.Entity);
			}
		}

		// ISignalOnDeath INTERFACE

		void ISignalOnDeath.OnDeath(Frame frame, EntityRef entity, EntityRef killer)
		{
			if (frame.Unsafe.TryGetPointer<Weapon>(entity, out var weapon) == false)
				return;

			weapon->Stop();
		}
	}
}
