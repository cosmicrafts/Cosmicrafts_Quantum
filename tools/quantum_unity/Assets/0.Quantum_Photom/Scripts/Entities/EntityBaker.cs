namespace TowerRush.Core
{
	using UnityEngine;
	using UnityEngine.SceneManagement;
	using Quantum;

	public interface IBakable
	{
		void Bake();
	}

	public sealed class EntityBaker : MapDataBakerCallback
	{
		// MapDataBakerCallback INTERFACE

		public override void OnBake(MapData data)
		{
			var bakedComponents = 0;

			var activeScene = SceneManager.GetActiveScene();
			var rootObjects = activeScene.GetRootGameObjects();

			for (int i = 0; i < rootObjects.Length; ++i)
			{
				var rootObject = rootObjects[i];
				var bakables   = rootObject.GetComponentsInChildren<IBakable>(true);

				for (int y = 0; y < bakables.Length; ++y)
				{
					var bakable = bakables[y];
					bakable.Bake();

					++bakedComponents;

					var bakableComponent = bakable as Component;

					Log.Info($"Baked {bakableComponent.name} ({bakable.GetType().Name})", bakableComponent.gameObject);

#if UNITY_EDITOR
					UnityEditor.EditorUtility.SetDirty(bakableComponent);
#endif
				}
			}

#if UNITY_EDITOR
			UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(activeScene);
#endif
			Log.Info($"Baked {bakedComponents} scene components");
		}

		public override void OnBeforeBake(MapData data)
		{
		}
	}
}
