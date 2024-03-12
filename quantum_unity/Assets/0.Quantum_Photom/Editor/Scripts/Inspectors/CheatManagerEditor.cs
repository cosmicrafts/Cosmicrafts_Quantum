namespace TowerRush.Editor
{
	using UnityEditor;
	using UnityEngine;

public class CheatManagerEditor : EditorWindow
{
		private   const int   WIDTH   = 440;
		private   const int   PADDING = 4;

		private   Vector2     m_ScrollPosition;

		[MenuItem("Clash/Cheat Manager")]
		static void Init()
		{
			var window              = GetWindow<CheatManagerEditor>();
			    window.minSize      = new Vector2(WIDTH, 400.0f);
			    window.titleContent = new GUIContent("Cheat Manager");
		}

		private void OnGUI()
		{
			using (new EditorGUI.DisabledGroupScope(Application.isPlaying))
			{
				#if ENABLE_CHEAT_MANAGER
				DrawColumn("CHEAT MANAGER", WIDTH, ref m_ScrollPosition, GuiStyles.Column, OnDrawCheatManager);
				#else
				DrawColumn("CHEAT MANAGER", WIDTH, ref m_ScrollPosition, GuiStyles.Column, OnDrawCheatManagerDisabled);
				#endif
			}
		}

		private void OnDrawCheatManager(float width)
		{
			#if ENABLE_CHEAT_MANAGER
			var cheatManager     = CheatManager.Instance;
			var serializedObject = new SerializedObject(cheatManager);
			var iterator         = serializedObject.GetIterator();

			EditorGUIUtility.wideMode   = true;
			EditorGUIUtility.labelWidth = width * 0.6f;
			EditorGUIUtility.fieldWidth = width * 0.4f;

			EditorGUI.BeginChangeCheck();

			if (CheatManager.Profiles != null)
			{
				var index    = CheatManager.Profiles.IndexOf(CheatManager.ActiveProfile);
				var newIndex = EditorGUILayout.Popup("Profile", index, CheatManager.Profiles);

				if (index != newIndex)
				{
					CheatManager.ActiveProfile = CheatManager.Profiles[newIndex];
				}

				using (new GUILayout.HorizontalScope())
				{
					GUILayout.FlexibleSpace();

					if (GUILayout.Button("Create Profile", GUILayout.Width(150f)) == true)
					{
						var dialog = InputStringDialog.Show(GUIUtility.GUIToScreenPoint(Event.current.mousePosition), "New profile name", "");
						dialog.Validate += value => { return value.HasValue() && CheatManager.Profiles.Contains(value) == false; };
						dialog.OnClose  += (result, obj) =>
						{
							if (result == ModalWindow.E_Result.Ok)
							{
								CheatManager.CreateProfile((string)obj);
							}
						};
					}

					using (new EditorGUI.DisabledGroupScope(CheatManager.Profiles.Length <=  1))
					{
						if (GUILayout.Button("Delete Profile", GUILayout.Width(150f)) == true)
						{
							CheatManager.DeleteProfile(CheatManager.ActiveProfile);
						}
					}
				}
			}

			GUILayout.Space(10f);

			if (serializedObject.targetObject != null)
			{
				for (bool enterChildren = true; iterator.NextVisible(enterChildren) == true; enterChildren = false)
				{
					if (iterator.name == "m_Script")
						continue;

					EditorGUILayout.PropertyField(iterator, true);
				}

				if (EditorGUI.EndChangeCheck() == true)
				{
					serializedObject.ApplyModifiedProperties();
					CheatManager.Save();
				}
			}


			EditorGUIUtility.labelWidth = 0f;
			EditorGUIUtility.fieldWidth = 0f;
			#endif
		}

		private void OnDrawCheatManagerDisabled(float width)
		{
			using (new GUILayout.VerticalScope(GUILayout.Width(width)))
			{
				GUILayout.FlexibleSpace();

				using (new GUILayout.HorizontalScope(GUILayout.Width(width)))
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label("Cheat Manager Disabled");
					GUILayout.FlexibleSpace();
				}

				GUILayout.FlexibleSpace();
			}
		}

		private void DrawColumn(string label, float width, ref Vector2 scrollPos, GUIStyle style, System.Action<float> drawer)
		{
			using (new GUILayout.VerticalScope(GUILayout.Width(width)))
			{
				GUILayout.Space(4.0f);
				GUILayout.Label(label);

				using (new GUILayout.VerticalScope(GuiStyles.Column))
				{
					using (var scroller = new GUILayout.ScrollViewScope(scrollPos))
					{
						GUILayout.Space(PADDING);

						width -= 2 * GUI.skin.verticalScrollbar.fixedWidth + 1.0f;

						drawer(width);

						GUILayout.Space(PADDING);

						GUILayout.FlexibleSpace();

						scrollPos = scroller.scrollPosition;
					}
				}
			}
		}

		private static class GuiStyles
		{
			public static readonly Color    BackgroundSelected    = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			public static readonly Color    BackgroundNotSelected = new Color(0.7f, 0.7f, 0.7f, 0.7f);

			public static readonly GUIStyle Column;
			public static readonly GUIStyle BoldLabel;

			public static readonly GUIStyle Type;
			public static readonly GUIStyle TypeSelected;

			static GuiStyles()
			{
				Column               = new GUIStyle("ProgressBarBack");
				Column.padding       = new RectOffset(0, 0, 0, 0);
				Column.margin        = new RectOffset(0, 0, 0, 0);

				BoldLabel            = new GUIStyle("BoldLabel");
				BoldLabel.margin     = new RectOffset();

				Type                 = new GUIStyle("flow node 0");
				Type.margin          = new RectOffset(PADDING, PADDING, PADDING, PADDING);
				Type.padding         = new RectOffset(PADDING, PADDING, PADDING, PADDING);

				TypeSelected         = new GUIStyle("flow node 1");
				TypeSelected.margin  = Type.margin;
				TypeSelected.padding = Type.padding;
			}
		}
	}
}
