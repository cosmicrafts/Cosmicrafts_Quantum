using UnityEngine;

public static class GameObjectExtensions
{
	public static void SetLayerRecursively(this GameObject @this, int layer)
	{
		@this.layer   = layer;
		var transform = @this.transform;
		var count     = transform.childCount;

		for (int idx = 0; idx < count; idx++)
		{
			var child = transform.GetChild(idx);
			child.gameObject.SetLayerRecursively(layer);
		}
	}
}
