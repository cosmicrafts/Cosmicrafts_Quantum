namespace Quantum.Services
{
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;

	public static class Pool<T> where T : new()
	{
		private const int POOL_CAPACITY = 4;

		private static List<T> _pool = new List<T>(POOL_CAPACITY);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Get()
		{
			bool found = false;
			T    item  = default;

			lock (_pool)
			{
				int index = _pool.Count - 1;
				if (index >= 0)
				{
					found = true;
					item  = _pool[index];

					_pool[index] = _pool[_pool.Count - 1];
					_pool.RemoveAt(_pool.Count - 1);
				}
			}

			if (found == false)
			{
				item = new T();
			}

			return item;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Return(T item)
		{
			if (item == null)
				return;

			lock (_pool)
			{
				_pool.Add(item);
			}
		}
	}
}

