using System.Collections.Generic;
using UnityEngine;

public class UIElementCache<T> where T : Component
{
	// PUBLIC MEMBERS

	public T this[int index] { get { return GetElement(index); } }

	// PRIVATE MEMBERS

	private T       m_Prefab;
	private List<T> m_Elements;

	// C-TOR

	public UIElementCache(T prefab, int initCount)
	{
		m_Prefab   = prefab;
		m_Elements = new List<T>(initCount * 2);

		m_Prefab.SetActive(false);

		while (initCount >= m_Elements.Count)
		{
			CreateElement();
		}
	}

	// PUBLIC METHODS

	public T GetElement(int index)
	{
		while (index >= m_Elements.Count)
		{
			CreateElement();
		}

		var element = m_Elements[index];
		element.SetActive(true);
		return element;
	}

	public void HideAll(int startIndex)
	{
		for (int idx = startIndex, count = m_Elements.Count; idx < count; idx++)
		{
			m_Elements[idx].SetActive(false);
		}
	}

	public IEnumerator<T> GetEnumerator()
	{
		return m_Elements.GetEnumerator();
	}

	// PRIVATE METHODS

	private void CreateElement()
	{
		var instance = Object.Instantiate(m_Prefab, m_Prefab.transform.parent);
		m_Elements.Add(instance);
	}
}
