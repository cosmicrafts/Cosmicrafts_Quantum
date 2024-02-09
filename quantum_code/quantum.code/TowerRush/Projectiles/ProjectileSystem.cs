namespace Quantum
{
	unsafe class ProjectileSystem : SystemMainThread
	{
		public override void Update(Frame frame)
		{
			foreach (var pair in frame.Unsafe.GetComponentBlockIterator<Projectile>())
			{
				pair.Component->Update(frame, pair.Entity);
			}
		}
	}
}
