#if UNITY_EDITOR
	#define COLLECT_STATS
#endif

namespace TowerRush.Core
{
	using System.Collections.Generic;
	using Quantum;

	public static class ListPool
	{
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////

		//-------------------------------------------------------------------------------------------------------------
		public static List<T> Get<T>(int capacity)
		{
			return Pool<T>.Get(capacity);
		}

		//-------------------------------------------------------------------------------------------------------------
		public static void Return<T>(List<T> list)
		{
			Pool<T>.Return(list);
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////

		private static class Pool<T>
		{
#if COLLECT_STATS
			private class Stats : PoolStatistics
			{
				public Stats()
				{
					CollectionType = typeof(List<T>);
					ElementType = typeof(T);
				}

				public override int GetCollectionsCount()            { return m_Pool.Count; }
				public override int GetCollectionCapacity(int index) { return m_Pool[index].Capacity; }
			}

			private static readonly Stats m_Stats;
#endif
			private static readonly List<List<T>> m_Pool;

			//---------------------------------------------------------------------------------------------------------
			static Pool()
			{
#if COLLECT_STATS
				m_Stats = new Stats();
#endif
				m_Pool = new List<List<T>>(16);
			}

			//---------------------------------------------------------------------------------------------------------
			public static List<T> Get(int capacity)
			{
#if COLLECT_STATS
				m_Stats.OnGet();
#endif

				int num = m_Pool.Count;
				if (num == 0)
				{
					if (capacity < 4)
						capacity = 4;

					return new List<T>(capacity);
				}

				List<T> list;
				int     bestIndex    = -1;
				int     bestCapacity = -1;

				for (int idx = 0; idx < num; ++idx)
				{
					list = m_Pool[idx];

					if (list.Capacity < bestCapacity)
						continue;

					bestCapacity = list.Capacity;
					bestIndex = idx;

					if (bestCapacity >= capacity)
						break;
				}

				list = m_Pool[bestIndex];

				if (bestCapacity < capacity)
					list.Capacity = capacity;

				m_Pool.RemoveAtWithSwap(bestIndex);
				return list;
			}

			//---------------------------------------------------------------------------------------------------------
			public static void Return(List<T> list)
			{
				if (list == null)
					return;

				list.Clear();

#if !RELEASE_BUILD
				if (list.Capacity == 0)
				{
					Log.Error($"Capacity of pooled 'List<{typeof(T).Name}>' reduced to zero!");
				}
#endif

				m_Pool.Add(list);

#if COLLECT_STATS
				m_Stats.OnReturn();
#endif
			}
		}
	}

}
