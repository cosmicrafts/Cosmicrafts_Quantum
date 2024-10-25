public static class ArrayExtensions
{
	public static int FindIndex<T>(this T[] @this, System.Predicate<T> predicate)
	{
		for (int idx = 0; idx < @this.Length; idx++)
		{
			var element = @this[idx];
			if (predicate(element) == true)
				return idx;
		}

		return -1;
	}

	public static T Find<T>(this T[] @this, System.Predicate<T> predicate)
	{
		for (int idx = 0; idx < @this.Length; idx++)
		{
			var element = @this[idx];
			if (predicate(element) == true)
				return element;
		}

		return default;
	}

	public static int SafeLength<T>(this T[] @this)
	{
		return @this != null ? @this.Length : 0;
	}
}
