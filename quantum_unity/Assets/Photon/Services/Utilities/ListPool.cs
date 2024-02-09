namespace Quantum.Services
{
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;

	public static class ListPool<T>
	{
		private const int POOL_CAPACITY = 4;
		private const int LIST_CAPACITY = 16;

		private static List<List<T>> _pool = new List<List<T>>(POOL_CAPACITY);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static List<T> Get(int capacity)
		{
			lock (_pool)
			{
				int poolCount = _pool.Count;

				if (poolCount == 0)
				{
					return new List<T>(capacity > 0 ? capacity : LIST_CAPACITY);
				}

				for (int i = 0; i < poolCount; ++i)
				{
					List<T> list = _pool[i];

					if (list.Capacity < capacity)
						continue;

					_pool[i] = _pool[_pool.Count - 1];
					_pool.RemoveAt(_pool.Count - 1);

					return list;
				}

				int lastListIndex = poolCount - 1;

				List<T> lastList = _pool[lastListIndex];
				lastList.Capacity = capacity;

				_pool.RemoveAt(lastListIndex);

				return lastList;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Return(List<T> list)
		{
			if (list == null)
				return;

			list.Clear();

			lock (_pool)
			{
				_pool.Add(list);
			}
		}
	}
}
