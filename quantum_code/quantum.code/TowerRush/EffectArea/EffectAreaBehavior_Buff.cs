namespace Quantum
{
	public unsafe partial struct EffectAreaBehavior_Buff
	{
		// PUBLIC METHODS

		public void ProcessEffect(Frame frame, EntityRef entity, EntityRef target, byte level)
		{
			if (frame.Unsafe.TryGetPointer<Buffs>(target, out var buffs) == false)
				return;


			buffs->AddBuff(frame, entity, target, Buff, level);
		}
	}
}
