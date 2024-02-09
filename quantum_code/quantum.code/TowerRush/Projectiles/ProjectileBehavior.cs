namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct ProjectileBehavior
	{
		public void FinishProjectile(Frame frame, PlayerRef owner, EntityRef source, EntityRef target, FPVector2 targetPosition, byte level)
		{
			switch (Field)
			{
				case AOE:          _AoE.FinishProjectile(frame, owner, source, targetPosition, level); break;
				case SINGLETARGET: _SingleTarget.FinishProjectile(frame, source, target, level);       break;

				default:
					throw new System.NotImplementedException();
			}
		}
	}
}
