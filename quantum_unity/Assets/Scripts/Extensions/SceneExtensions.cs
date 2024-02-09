using TowerRush.Core;
using System.Collections.Generic;
using UnityEngine;

public static class SceneExtensions
{
	public static void GetComponents<T>(this UnityEngine.SceneManagement.Scene @this, List<T> components) where T : Component
	{
		var roots         = ListPool.Get<GameObject>(4);
		var tmpComponents = ListPool.Get<T>(16);

		@this.GetRootGameObjects(roots);

		foreach (var root in roots)
		{
			root.GetComponentsInChildren(tmpComponents);
			components.AddRange(tmpComponents);
		}

		ListPool.Return(roots);
		ListPool.Return(tmpComponents);
	}
}
