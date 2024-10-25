#if FINAL_BUILD == false
namespace TowerRush.Editor
{
	using UnityEngine;
	using UnityEditor;
	using UnityEditor.Callbacks;

	public class DevelopmentPostprocessor
	{
		// PRIVATE MEMBERS

		private static bool m_Processed;

		// UNITY CALLBACKS

		[PostProcessScene]
		private static void PostprocessScene()
		{
			if (m_Processed == true)
				return;

			#if ENABLE_CHEAT_MANAGER
			if (CheatManager.Instance.ShowDebugInfo == true)
			{
				AddDebugInfo();
			}
			#else
			AddDebugInfo();
			#endif

			EditorApplication.playModeStateChanged += OnPostprocessBuild;
			m_Processed = true;
		}

		[PostProcessBuild]
		private static void PostprocessBuild(BuildTarget target, string path)
		{
			m_Processed = false;
		}

		// HANDLERS

		private static void OnPostprocessBuild(PlayModeStateChange mode)
		{
			if (EditorApplication.isPlaying == true)
				return;

			m_Processed = false;
			EditorApplication.playModeStateChanged -= OnPostprocessBuild;
		}

		// PRIVATE METHODS

		private static void AddDebugInfo()
		{
			var info = Object.Instantiate(Resources.Load<GameObject>("Debug"));
			var game = Object.FindObjectOfType<Game>();

			if (game != null)
			{
				info.transform.SetParent(game.transform);
			}
		}
	}
}
#endif
