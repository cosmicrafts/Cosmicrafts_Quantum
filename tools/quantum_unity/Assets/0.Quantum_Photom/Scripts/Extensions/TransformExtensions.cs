using UnityEngine;
using System.Text;
using System.Collections.Generic;
using TowerRush.Core;

public static class TransformExtension
{
	// PRIVATE MEMBERS

	static StringBuilder m_StringBuilder = new StringBuilder(128);

	// PUBLIC METHODS

	public static Transform[] FindDeepAll(this Transform @this, string name)
	{
		var transformList = ListPool.Get<Transform>(4);

		FindAllDeep(@this, name, transformList);

		var result = transformList.ToArray();
		ListPool.Return(transformList);

		return result;
	}

	public static Transform FindDeep(this Transform @this, string name)
	{
		var child = @this.Find(name);
		if (child != null)
			return child;

		for (int i = 0; i < @this.childCount; ++i)
		{
			child = @this.GetChild(i).FindDeep(name);

			if (child != null)
				return child;
		}

		return null;
	}

	public static T GetComponentAtPath<T>(this Transform @this, string namePath) where T : class
	{
		if (@this == null)
			return default(T);
		
		var child = @this.FindDeep(namePath);
		if (child == null)
			return default(T);

		var component = child as T;

		return component != null ? component : child.GetComponent(typeof(T)) as T;
	}

	public static T GetComponentInParent<T>(this Transform @this, bool includeInactive) where T : class
	{
		if (@this == null)
			return default(T);

		var type = typeof(T);

		if (includeInactive == false)
			return @this.GetComponentInParent(type) as T;

		var parent = @this.transform;

		while (parent != null)
		{
			var component = parent.GetComponent(type);
			if (component != null)
				return component as T;
			
			parent = parent.parent;
		}

		return null;
	}

	public static string GetNamePath(this Transform @this)
	{
		if (ReferenceEquals(@this, null) == true)
			return string.Empty;

		m_StringBuilder.Length = 0;

		var obj = @this;
		while (true)
		{
			m_StringBuilder.Insert(0, obj.name);

			obj = obj.parent;

			if (ReferenceEquals(obj, null) == true)
				break;

			m_StringBuilder.Insert(0, "/");
		}

		return m_StringBuilder.ToString();
	}

	// PRIVATE METHODS

	private static void FindAllDeep(Transform root, string name, List<Transform> list)
	{
		if (root.name == name)
		{
			list.Add(root);
		}

		for (int idx = 0; idx < root.childCount; idx++)
		{
			var child = root.GetChild(idx);
			FindAllDeep(child, name, list);
		}
	}
}
