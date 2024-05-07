namespace Quantum
{
	public unsafe partial struct EffectAreaBehavior
	{
		public void Initialize(byte level)
		{
			
			Log.Debug("EffectArea Initialize: " + Field);
			
			switch (Field)
			{
				case DAMAGE: _Damage.Initialize(level); break;

				case BUFF:   break;
				
				case EMPTY:   break;

				default:
					throw new System.NotImplementedException();
			}
		}

		public void ProcessEffect(Frame frame, EntityRef sourceEntity, EntityRef entity, EntityRef target, byte level)
		{
			switch (Field)
			{
				case DAMAGE: _Damage.ProcessEffect(frame, sourceEntity, entity, target);      break;
				case BUFF:   _Buff.ProcessEffect(frame, entity, target, level); break;
				case EMPTY:   break;

				default:
					throw new System.NotImplementedException();
			}
		}
	}
}
