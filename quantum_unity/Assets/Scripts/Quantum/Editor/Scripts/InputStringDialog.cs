namespace TowerRush.Editor
{
	using UnityEngine;
	using UnityEditor;

	using Delegate_Validate = System.Func<string, bool>;

	public class InputStringDialog : ModalWindow
	{
		private bool   m_Opened = true;
		private string m_Name   = string.Empty;

		public  string Name { get { return m_Name; } set { m_Name = value; } }

		public Delegate_Validate Validate = null;

		public static InputStringDialog Show(Vector2 position)
		{
			return Show(position, "", string.Empty);
		}

		public static InputStringDialog Show(Vector2 position, string caption, string name)
		{
			return Show<InputStringDialog>(position, window =>
			{
				window.Caption       = caption;
				window.Name          = name;
				window.CloseManually = true;
				window.Size          = new Vector2(250f, 100f);
			});
		}

		public void OnGUI()
		{
			GUILayout.Space(20f);

			using (new GUILayout.HorizontalScope())
			{
				GUILayout.Space(10f);

				using (new GUILayout.VerticalScope())
				{
					if (m_Opened)
						GUI.SetNextControlName("NameTextField");

					m_Name = EditorGUILayout.TextField(m_Name);

					if (m_Opened)
						EditorGUI.FocusTextInControl("NameTextField");
				}

				GUILayout.Space(10f);
			}

			m_Opened = false;

			GUILayout.FlexibleSpace();

			using (new GUILayout.HorizontalScope())
			{
				GUILayout.Space(40);

				if (GUILayout.Button("Cancel", GUILayout.Width(70f)) == true)
				{
					Close(E_Result.Cancel, null);
				}

				GUILayout.FlexibleSpace();

				using (new EditorGUI.DisabledGroupScope(IsValid(m_Name) == false))
				{
					if (GUILayout.Button("OK", GUILayout.Width(70f)) == true)
					{
						Close(E_Result.Ok, m_Name.Trim());
					}
				}

				GUILayout.Space(40f);
			}

			GUILayout.Space(20.0f);
		}

		private bool IsValid(string name)
		{
			name = name.Trim();

			if (string.IsNullOrEmpty(name) == true)
				return false;

			if (Validate != null)
				return Validate(name);

			return true;
		}
	}
}
