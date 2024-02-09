namespace Quantum
{
	public unsafe partial struct EffectAreaBehavior
	{
		public void Initialize(byte level)
		{
			switch (Field)
			{
				case DAMAGE: _Damage.Initialize(level); break;

				case BUFF:   break;

				default:
					throw new System.NotImplementedException();
			}
		}

		public void ProcessEffect(Frame frame, EntityRef entity, EntityRef target, byte level)
		{
			switch (Field)
			{
				case DAMAGE: _Damage.ProcessEffect(frame, entity, target);      break;
				case BUFF:   _Buff.ProcessEffect(frame, entity, target, level); break;

				default:
					throw new System.NotImplementedException();
			}
		}
	}
}
