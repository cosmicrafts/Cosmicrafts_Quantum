namespace Quantum
{
	public unsafe partial struct UnitBehavior
	{
		public void Initialize()
		{
			switch (Field)
			{
				case LIFETIME: _Lifetime.Initialize(); break;

				case SPAWNONDEATH: break;
				case SPAWNER:      break;

				default:
					throw new System.NotImplementedException();
			}
		}

		public void Update(Frame frame, PlayerRef owner, EntityRef entity, byte level)
		{
			switch (Field)
			{
				case SPAWNER:  _Spawner.Update(frame, owner, entity, level); break;
				case LIFETIME: _Lifetime.Update(frame, entity);              break;

				case SPAWNONDEATH: break;

				default:
					throw new System.NotImplementedException();
			}
		}

		public void OnDeath(Frame frame, PlayerRef owner, EntityRef entity, byte level)
		{
			switch (Field)
			{
				case SPAWNONDEATH: _SpawnOnDeath.OnDeath(frame, owner, entity, level); break;

				case SPAWNER: break;
				case LIFETIME:break;

				default:
					throw new System.NotImplementedException();
			}
		}
	}
}
