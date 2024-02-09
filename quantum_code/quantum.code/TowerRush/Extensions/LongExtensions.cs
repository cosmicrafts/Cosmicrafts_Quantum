public static class LongExtensions
{
	// PUBLIC METHODS

	public static bool IsBitSet(this long flags, int bit)
	{
		return (flags & (1 << bit)) == (1 << bit);
	}

	public static long SetBit(this long flags, int bit, bool value)
	{
		if (value == true)
		{
			return flags |= (long)(1 << bit);
		}
		else
		{
			return flags &= unchecked((long)~(1 << bit));
		}
	}
}
