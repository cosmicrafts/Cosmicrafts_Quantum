namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct WeaponBehavior
	{
		public void ProcessAttack(Frame frame, PlayerRef owner, EntityRef entity, EntityRef target, byte level)
		{
			switch (Field)
			{
				case SINGLETARGET: _SingleTarget.ProcessAttack(frame, entity, target, level); break;
				case PROJECTILE:   _Projectile.ProcessAttack(frame, owner, entity, target, level);   break;

				default:
					throw new System.NotImplementedException();
			}
		}
	}
}
