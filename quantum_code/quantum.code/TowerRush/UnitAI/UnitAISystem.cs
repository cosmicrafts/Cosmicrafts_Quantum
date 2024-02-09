namespace Quantum
{
	public unsafe class UnitAISystem : SystemMainThread, ISignalOnDeath
	{
		// SystemMainThread INTERFACE

		public override void Update(Frame frame)
		{
			foreach (var pair in frame.Unsafe.GetComponentBlockIterator<UnitAI>())
			{
				pair.Component->Update(frame, pair.Entity);
			}
		}

		// ISignalOnDeath INTERFACE

		void ISignalOnDeath.OnDeath(Frame frame, EntityRef entity, EntityRef killer)
		{
			if (frame.Unsafe.TryGetPointer<UnitAI>(entity, out var unitAI) == false)
				return;

			unitAI->Stop();
		}
	}
}
