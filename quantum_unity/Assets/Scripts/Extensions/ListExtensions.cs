using System.Collections.Generic;

public static class ListExtensions
{
	public static bool AddUnique<T>(this List<T> @this, T item)
	{
		if (@this.Contains(item) == true)
			return false;

		@this.Add(item);
		return true;
	}

	public static bool RemoveWithSwap<T>(this List<T> @this, T item)
	{
		if(@this == null)
			return false;
		
		var index = @this.IndexOf(item);
		if (index < 0)
			return false;

		RemoveAtWithSwap(@this, index);
		return true;
	}

	public static void RemoveAtWithSwap<T>(this List<T> @this, int index)
	{
		if(@this == null)
			return;
		
		int last = @this.Count - 1;
		if (last > index)
		{
			@this[index] = @this[last];
		}
		
		@this.RemoveAt(last);
	}

	public static T LastOrDefault<T>(this List<T> @this)
	{
		if (@this.Count == 0)
			return default(T);

		return @this[@this.Count - 1];
	}

	public static T PopLast<T>(this List<T> @this)
	{
		if (@this.Count == 0)
			return default(T);

		var index = @this.Count - 1;
		var item  = @this[index];
		
		@this.RemoveAt(index);
		
		return item;
	}

	public static void Shuffle<T>(this List<T> @this)
	{
		if (@this == null)
			return;

		for (int idx = @this.Count; idx > 1;)
		{
			int rnd = UnityEngine.Random.Range(0, idx--);

			T tmp = @this[rnd];
			@this[rnd] = @this[idx];
			@this[idx] = tmp;
		}
	}

	public static void Shuffle<T>(this List<T> @this, System.Random rng)
	{
		if (@this == null)
			return;

		for (int idx = @this.Count; idx > 1;)
		{
			int rnd = rng.Next(0, idx--);

			T tmp = @this[rnd];
			@this[rnd] = @this[idx];
			@this[idx] = tmp;
		}
	}

	public static bool AddIfNotNull<T>(this List<T> @this, T item)
	{
		if (item == null)
			return false;

		@this.Add(item);
		return true;
	}
}
