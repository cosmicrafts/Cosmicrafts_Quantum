namespace Quantum
{
	unsafe class EffectAreaSystem : SystemMainThread
	{
		public override void Update(Frame frame)
		{
			foreach (var pair in frame.Unsafe.GetComponentBlockIterator<EffectArea>())
			{
				pair.Component->Update(frame, pair.Entity);
			}
		}
	}
}
