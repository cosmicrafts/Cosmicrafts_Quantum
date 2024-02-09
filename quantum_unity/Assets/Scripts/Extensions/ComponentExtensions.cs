using UnityEngine;

public static class ComponentExtensions
{
	public static void SetActive(this Component @this, bool enable)
	{
		if (@this == null)
			return;

		@this.gameObject.SetActive(enable);
	}

	public static string GetNamePath(this Component @this)
	{
		if (ReferenceEquals(@this, null) == true)
			return string.Empty;

		return @this.transform.GetNamePath();
	}
}
