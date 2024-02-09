using TowerRush.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

using Scene = UnityEngine.SceneManagement.Scene;

[InitializeOnLoad]
public class UIViewEditorVisibility : IProcessSceneWithReport
{
	int IOrderedCallback.callbackOrder => int.MinValue;

	void IProcessSceneWithReport.OnProcessScene(Scene scene, BuildReport report)
	{
		var views = ListPool.Get<UIView>(16);
		scene.GetComponents(views);

		foreach (var view in views)
		{
			view.SetActive(true);
		}

		ListPool.Return(views);
	}

	static UIViewEditorVisibility()
	{
		Selection.selectionChanged -= OnSelectionChanged;
		Selection.selectionChanged += OnSelectionChanged;
	}

	private static void OnSelectionChanged()
	{
		if (EditorApplication.isPlaying == true)
			return;

		var sceneViews  = ListPool.Get<UIView>(16);
		var activeScene = EditorSceneManager.GetActiveScene();

		activeScene.GetComponents(sceneViews);

		foreach (var selected in Selection.gameObjects)
		{
			if (selected == null)
				continue;
			if (selected.scene == null)
				continue;

			var view = selected.GetComponent<UIView>();
			if (view == null)
				view = selected.GetComponentInParent<UIView>(true);

			if (view == null)
				continue;

			view.gameObject.SetActive(true);
			sceneViews.Remove(view);
		}

		foreach (var view in sceneViews)
		{
			view.SetActive(false);
		}
	}
}
