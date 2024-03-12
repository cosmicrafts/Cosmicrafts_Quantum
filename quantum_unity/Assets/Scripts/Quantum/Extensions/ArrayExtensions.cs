using System;

public static class ArrayExtensions
{
	public static bool Contains<T>(this T[] @this, T element)
	{
		for (int idx = 0, count = @this.Length; idx < count; idx++)
		{
			if (@this[idx].Equals(element) == true)
				return true;
		}

		return false;
	}

	public static int IndexOf<T>(this T[] @this, T value)
	{
		return Array.IndexOf(@this, value);
	}

	public static bool Any<T>(this T[] @this, Predicate<T> predicate)
	{
		for (int idx = 0; idx < @this.Length; idx++)
		{
			if (predicate(@this[idx]) == true)
				return true;
		}

		return false;
	}

	public static T FirstOrDefault<T>(this T[] @this)
	{
		return @this.SafeLength() > 0 ? @this[0] : default(T);
	}

	public static T LastOrDefault<T>(this T[] @this)
	{
		var length = @this.SafeLength();

		return length > 0 ? @this[length - 1] : default(T);
	}

	public static T RandomOrDefault<T>(this T[] @this)
	{
		var length = @this.SafeLength();

		return length > 0 ? @this[UnityEngine.Random.Range(0, length)] : default;
	}

	public static T RandomOrDefault<T>(this T[] @this, Random rng)
	{
		var length = @this.SafeLength();

		return length > 0 ? @this[rng.Next(0, length)] : default;
	}
}
