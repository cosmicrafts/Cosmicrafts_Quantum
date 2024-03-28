namespace TowerRush.Editor
{
	using UnityEditor;
	using UnityEngine;

	using Delegate_Closed = System.Action<ModalWindow.E_Result, System.Object>;

	public abstract class ModalWindow : EditorWindow
	{
		public enum E_Result
		{
			Ok,
			Cancel,
			FocusLost
		}

		public string  Caption       { get { return titleContent.text; } set { titleContent = new GUIContent(value); } }

		public bool    CloseManually { get; set; }
		public Vector2 Size          { get; set; }

		public event Delegate_Closed OnClose;

		protected static T Show<T>(Vector2 position, System.Action<T> initCallback) where T : ModalWindow
		{
			var window = EditorWindow.CreateInstance<T>();

			initCallback(window);

			window.ShowUtility();

			window.position = new Rect(position.x, position.y, window.Size.x, window.Size.y);

			return window;
		}

		protected void Close(E_Result result, System.Object data)
		{
			OnClose?.Invoke(result, data);

			Close();
		}

		protected virtual void OnLostFocus()
		{
			if (CloseManually)
				Focus();
			else
				Close(E_Result.FocusLost, null);
		}
	}
}
