#if ENABLE_CHEAT_MANAGER
namespace TowerRush.Editor
{
	using UnityEngine;
	using UnityEditor;
	using UnityEditor.Callbacks;
	using System.Reflection;

	public class CheatManagerPostprocessor
	{
		private static bool m_Processed;

		// UNITY CALLBACKS

		[PostProcessScene(int.MaxValue)]
		private static void PostprocessScene()
		{
			if (m_Processed == true)
				return;
			if (BuildPipeline.isBuildingPlayer == false)
				return;

			var json = CheatManager.SaveToJson();
			var game = GameObject.FindObjectOfType<Game>();

			var fieldInfo = typeof(Game).GetField("m_CheatManagerJson", BindingFlags.Instance | BindingFlags.NonPublic);
			fieldInfo.SetValue(game, json);

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
	}
}
#endif
