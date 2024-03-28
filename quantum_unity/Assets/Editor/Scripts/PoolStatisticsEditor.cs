namespace TowerRush.Core.Editor
{
	using UnityEngine;
	using UnityEditor;
	using System;
	using System.Collections.Generic;

	public class PooledStatisticsViewer : SearchableWindow
	{
		// PRIVATE MEMBERS

		private        string                    m_Filter;
		private        Vector2                   m_ScrollPosition;

		private        int                       m_TotalTypes;
		private        int                       m_TotalCollections;
		private        int                       m_TotalMemory;

		private        ESortedBy                 m_SortedBy;
		private        List<TRecord>             m_TRecords            = new List<TRecord>(32);
		private        List<CRecord>             m_CRecords            = new List<CRecord>(64);

		private static GUIContent[]              m_SortOptions;

		private static Dictionary<int, string>   m_NamesCache          = new Dictionary<int, string>(32);
		private static Dictionary<ulong, string> m_StringsCache        = new Dictionary<ulong, string>(128);

		private static List<TRecord>             m_TRecordsPool        = new List<TRecord>(32);
		private static List<CRecord>             m_CRecordsPool        = new List<CRecord>(64);

		// UNITY METHODS

		[MenuItem("Photon/Utilities/Pooled Collections Stats")]
		static void MenuItem()
		{
			var window     = EditorWindow.GetWindow<PooledStatisticsViewer>("Pools Stats", true);
			window.minSize = new Vector2(300f, 300f);
		}

		void Update()
		{
			Repaint();
		}

		void OnGUI()
		{
			Init();

			DrawToolbar();

			if (CollectStats() == true)
			{
				DrawStats();
				DrawFooter();
			}
			else
			{
				DrawMessage("Pooled collections not used (yet).");
			}
		}

		// PRIVATE METHODS

		private static void Init()
		{
			if (m_StringsCache.Count > 512)
			{
				m_StringsCache.Clear();
			}

			if (m_SortOptions == null)
			{
				var names     = Enum.GetNames(typeof(ESortedBy));
				m_SortOptions = Array.ConvertAll(names, x => new GUIContent(ObjectNames.NicifyVariableName(x)));
			}
		}

		private void DrawToolbar()
		{
			using (new GUILayout.HorizontalScope(GuiStyles.Toolbar))
			{
				m_SortedBy = (ESortedBy)EditorGUILayout.Popup((int)m_SortedBy, m_SortOptions, GuiStyles.DropDown, GUILayout.Width(140f));

				GUILayout.Space(3f);

				m_Filter = DrawSearchField().ToUpper();
			}
		}

		private void DrawStats()
		{
			using (var view = new GUILayout.ScrollViewScope(m_ScrollPosition))
			{
				view.handleScrollWheel = true;

				for (int idx = 0; idx < m_TRecords.Count; ++idx)
				{
					DrawRecord(m_TRecords[idx]);
				}

				if (m_TotalTypes == 0)
				{
					DrawMessage("Search result is empty!");
				}

				m_ScrollPosition = view.scrollPosition;
			}
		}

		private void DrawRecord(TRecord rec)
		{
			GUI.backgroundColor = rec.Errors == 0 ? Color.Lerp(Color.green, Color.white, Mathf.Clamp01(rec.AccessTime))
												  : new Color(1.0f, 0.0f, 0.0f, 0.3f);

			using (new GUILayout.VerticalScope(GuiStyles.BackgroundOdd))
			{
				DrawHeader(rec);
			}

			if (rec.Count > 0)
			{
				using (new GUILayout.VerticalScope(GuiStyles.BackgroundEven))
				{
					for (int idx = 0; idx < rec.Count; ++idx)
					{
						DrawCollection(m_CRecords[rec.Index + idx]);
					}
				}
			}

			GUI.backgroundColor = Color.white;
		}

		private void DrawHeader(TRecord record)
		{
			var rect   = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.label);
			var width0 = (rect.xMax - 4) * 0.4f;
			var width1 = (rect.xMax - width0) * 0.6f;
			var width2 = (rect.xMax - width0 - width1);

			rect.width = width0;
			GUI.Label(rect, record.TypeName, EditorStyles.boldLabel);

			rect.xMin  = rect.xMax;
			rect.width = width1;
			GUI.Label(rect, $"{record.Gets}/{record.Returns}", EditorStyles.boldLabel);

			rect.xMin  = rect.xMax;
			rect.width = width2;
			GUI.Label(rect, GetTimeString(record.AccessTime), EditorStyles.boldLabel);
		}

		private void DrawCollection(CRecord rec)
		{
			var rect   = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.label);
			var width0 = (rect.xMax - 4) * 0.4f;
			var width1 = (rect.xMax - width0) * 0.6f;
			var width2 = (rect.xMax - width0 - width1);

			rect.width = width0;
			rect.xMin += 8f;
			GUI.Label(rect, rec.TypeName, EditorStyles.miniLabel);

			rect.xMin  = rect.xMax;
			rect.width = width1;
			GUI.Label(rect, rec.Capacity.ToString(), EditorStyles.miniLabel);

			rect.xMin  = rect.xMax;
			rect.width = width2;
			GUI.Label(rect, GetSizeString(rec.AllocMemory), EditorStyles.miniLabel);
		}

		private void DrawFooter()
		{
			using (new GUILayout.HorizontalScope(GuiStyles.Toolbar))
			{
				var rect = GUILayoutUtility.GetRect(0f, Screen.width, 0f, Screen.height, EditorStyles.miniLabel);
				var text = $"Types: {m_TotalTypes} | Collections: {m_TotalCollections} | Memory: {GetSizeString(m_TotalMemory)}";

				GUI.Label(rect, new GUIContent(text), EditorStyles.miniLabel);
			}
		}

		private static void DrawMessage(string text)
		{
			var rect = GUILayoutUtility.GetRect(0f, Screen.width, 0f, 100f, EditorStyles.centeredGreyMiniLabel);

			GUI.Label(rect, new GUIContent(text), EditorStyles.centeredGreyMiniLabel);
		}

		private bool CollectStats(bool mergeByType = true)
		{
			// check if not already done...

			if (Event.current.type != EventType.Layout)
				return m_TRecords.Count > 0;

			// reset...

			m_TotalTypes       = 0;
			m_TotalMemory      = 0;
			m_TotalCollections = 0;

			m_TRecordsPool.AddRange(m_TRecords);
			m_CRecordsPool.AddRange(m_CRecords);

			m_TRecords.Clear();
			m_CRecords.Clear();

			// collect...

			PoolStatistics.List.Sort(PoolStatisticsComparer.Get());

			int idx = 0;
			int num = PoolStatistics.List.Count;

			while (idx < num)
			{
				var stats    = PoolStatistics.List[idx];
				var type     = stats.ElementType;
				var typeName = GetTypeName(type);

				if (m_Filter.HasValue() == true && typeName.ToUpper().Contains(m_Filter) == false)
				{
					++idx; continue;
				}

				var typeSize     = GetTypeSize(type);

				var trec         = m_TRecordsPool.Count > 0 ? m_TRecordsPool.PopLast() : new TRecord();
				trec.TypeName    = typeName;
				trec.Gets        = 0;
				trec.Returns     = 0;
				trec.AccessTime  = float.MaxValue;
				trec.AllocMemory = 0;
				trec.Index       = m_CRecords.Count;
				trec.Count       = 0;

				while (stats.ElementType == type)
				{
					trec.Gets      += stats.GetsCount;
					trec.Returns   += stats.ReturnsCount;
					trec.AccessTime = Math.Min(trec.AccessTime, stats.GetSecondsFromLastAccess());

					typeName        = GetTypeName(stats.CollectionType);

					for (int idx2 = stats.GetCollectionsCount(); idx2-- > 0;)
					{
						var crec         = m_CRecordsPool.Count > 0 ? m_CRecordsPool.PopLast() : new CRecord();
						crec.TypeName    = typeName;
						crec.Capacity    = stats.GetCollectionCapacity(idx2);
						crec.AllocMemory = typeSize * crec.Capacity;

						m_CRecords.Add(crec);

						trec.Count       += 1;
						trec.AllocMemory += crec.AllocMemory;
					}

					if (++idx == num || mergeByType == false)
						break;

					stats = PoolStatistics.List[idx];
				}

				trec.Errors = (int)(trec.Gets - trec.Returns);

				m_CRecords.Sort(trec.Index, trec.Count, CRecordComparer.Get());

				m_TotalTypes       += 1;
				m_TotalMemory      += trec.AllocMemory;
				m_TotalCollections += trec.Count;

				m_TRecords.Add(trec);
			}

			// sort...

			m_TRecords.Sort(TRecordComparer.Get(m_SortedBy));

			return m_TRecords.Count > 0;
		}

		static string GetTypeName(Type type)
		{
			string name;
			int    key  = type.GetHashCode();

			if (m_NamesCache.TryGetValue(key, out name) == false)
			{
				name = type.Name;

				var idx = name.IndexOf('`');
				if (idx != -1)
				{
					name = name.Substring(0, idx);
				}

				m_NamesCache[key] = name;
			}

			return name;
		}

		private static int GetTypeSize(Type type)
		{
			if (type.IsEnum == true)
			{
				return System.Runtime.InteropServices.Marshal.SizeOf(Enum.GetUnderlyingType(type));
			}

			if (type.IsValueType == true)
			{
				return System.Runtime.InteropServices.Marshal.SizeOf(type);
			}

#if UNITY_EDITOR_64
			return sizeof(long);
#else
			return sizeof(int);
#endif
		}

		private static string GetTimeString(float seconds)
		{
			if (seconds < 1f)
				return GetString("{0:0.00} s", seconds);

			if (seconds < 60f)
				return GetString("{0} s", Mathf.FloorToInt(seconds));

			return GetString("{0} m", Mathf.FloorToInt(seconds / 60f));
		}

		private static string GetSizeString(int bytes)
		{
			if (bytes < 1024)
				return GetString("{0} B", bytes);

			var kilo = (float)bytes / 1024;
			if (kilo < 1024f)
				return GetString("{0:0.0} KB", kilo);

			var mega = (float)bytes / 1048576;
			return GetString("{0:0.0} MB", mega);
		}

		private static string GetString(string format, object param)
		{
			string str;
			ulong  key = (ulong)format.GetHashCode() << 32 | (uint)param.GetHashCode();

			if (m_StringsCache.TryGetValue(key, out str) == false)
			{
				m_StringsCache[key] = str = string.Format(format, param);
			}

			return str;
		}

		// HELPERS
		private enum ESortedBy
		{
			AccessTime,
			TypeName,
			AllocatedMemory,
			NumberOfCollections
		}

		private class TRecord
		{
			public string TypeName;      // name of collection's "element" type
			public int    Errors;        // difference between gets & returns --> leaks ?!?
			public uint   Gets;          // number of get-calls
			public uint   Returns;       // number of return-calls
			public float  AccessTime;    // seconds from last get/return call
			public int    AllocMemory;   // sum of 'AllocMemory' from all collections
			public int    Index;         // index of the 1st relevant collection-record
			public int    Count;         // count of relevant collection-records
		}

		private class TRecordComparer : Comparer<TRecord>
		{
			private static readonly TRecordComparer m_Instance = new TRecordComparer();
			private                 ESortedBy       m_SortBy;

			public static TRecordComparer Get(ESortedBy sortBy)
			{
				m_Instance.m_SortBy = sortBy;
				return m_Instance;
			}

			public override int Compare(TRecord x, TRecord y)
			{
				int result = y.Errors.CompareTo(x.Errors);
				if (result != 0)
					return result;

				switch (m_SortBy)
				{
					case ESortedBy.AccessTime:          result = x.AccessTime.CompareTo(y.AccessTime); break;
					case ESortedBy.AllocatedMemory:     result = y.AllocMemory.CompareTo(x.AllocMemory); break;
					case ESortedBy.NumberOfCollections: result = y.Count.CompareTo(x.Count); break;
				}

				if (result == 0)
					result = x.TypeName.CompareTo(y.TypeName);

				return result;
			}
		}

		private class CRecord
		{
			public string TypeName;      // name of collection type
			public int    Capacity;      // current capacity of collection
			public int    AllocMemory;   // type-size * capacity
		}

		private class CRecordComparer : Comparer<CRecord>
		{
			private static readonly CRecordComparer m_Instance = new CRecordComparer();

			public static CRecordComparer Get()
			{
				return m_Instance;
			}

			public override int Compare(CRecord x, CRecord y)
			{
				int res = x.TypeName.CompareTo(y.TypeName);
				if (res == 0)
					res = y.Capacity.CompareTo(x.Capacity);

				return res;
			}
		}

		private class PoolStatisticsComparer : Comparer<PoolStatistics>
		{
			private static readonly PoolStatisticsComparer m_Instance = new PoolStatisticsComparer();

			public static PoolStatisticsComparer Get()
			{
				return m_Instance;
			}

			public override int Compare(PoolStatistics x, PoolStatistics y)
			{
				return x.ElementType.GetHashCode().CompareTo(y.ElementType.GetHashCode());
			}
		}

		private static class GuiStyles
		{
			public static readonly GUIStyle Toolbar;
			public static readonly GUIStyle DropDown;
			public static readonly GUIStyle BackgroundEven;
			public static readonly GUIStyle BackgroundOdd;

			static GuiStyles()
			{
				Toolbar                = new GUIStyle("Toolbar");
				Toolbar.fixedHeight    = 22f;

				DropDown               = new GUIStyle("toolbarDropDown");
				DropDown.fixedHeight   = 22f;

				BackgroundEven         = new GUIStyle("CN EntryBackEven");
				BackgroundEven.margin  = new RectOffset();
				BackgroundEven.padding = new RectOffset();

				BackgroundOdd          = new GUIStyle("CN EntryBackOdd");
				BackgroundOdd.margin   = new RectOffset();
				BackgroundOdd.padding  = new RectOffset();
			}
		}
	}
}
