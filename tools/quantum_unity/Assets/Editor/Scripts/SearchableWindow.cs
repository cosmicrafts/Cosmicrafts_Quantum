namespace TowerRush.Core.Editor
{
	using UnityEngine;
	using UnityEditor;
	using System;
	using System.Reflection;

	public abstract class SearchableWindow : EditorWindow
	{
		// PROTECTED MEMBERS

		protected string m_SearchFilter;
		protected bool   m_FocusSearchField;

		// PRIVATE MEMBERS

		private static readonly Delegate_SearchFieldGUI m_SearchFieldMethod;


		// C-TOR
		static SearchableWindow()
		{
			var methodInfo = typeof(EditorGUI).GetMethod(
				"SearchField",
				BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod,
				null,
				new Type[] { typeof(Rect), typeof(string) },
				null);

			m_SearchFieldMethod = (Delegate_SearchFieldGUI)Delegate.CreateDelegate(
				typeof(Delegate_SearchFieldGUI), methodInfo);
		}

		protected SearchableWindow()
		{
			m_SearchFilter     = "";
			m_FocusSearchField = false;
		}

		// PROTECTED METHODS

		protected string DrawSearchField()
		{
			var rect = GUILayoutUtility.GetRect(0f, Screen.width, 0f, Screen.height, GuiStyles.SearchField);

			return DrawSearchField(rect);
		}

		protected string DrawSearchField(Rect position)
		{
			GUI.SetNextControlName("SearchFilter");

			if (m_FocusSearchField == true)
			{
				EditorGUI.FocusTextInControl("SearchFilter");

				if (Event.current.type == EventType.Repaint)
					m_FocusSearchField = false;
			}

			m_SearchFilter = m_SearchFieldMethod(position, m_SearchFilter);

			return m_SearchFilter;
		}

		// HELPERS

		private delegate string Delegate_SearchFieldGUI(Rect position, string filter);

		private static class GuiStyles
		{
			public static readonly GUIStyle SearchField;

			static GuiStyles()
			{
				SearchField = new GUIStyle("ToolbarSeachTextField");
			}
		}
	}
}
