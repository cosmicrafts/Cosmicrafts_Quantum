namespace TowerRush.Core
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.SceneManagement;

	public sealed class Coroutines : MonoBehaviour
	{
		// PUBLIC MEMBERS

		public  static bool               IsInitialized { get { return m_Instance != null; } }

		// PRIVATE MEMBERS

		private static Coroutines         m_Instance;
		private        List<Coroutine>    m_Coroutines;

		private        float              m_NextCleanupTime;

		// PUBLIC METHODS

		public void Initialize()
		{
			if (IsInitialized == true)
				return;

			m_Coroutines  = new List<Coroutine>(16);
			m_Instance    = this;

			SceneManager.sceneLoaded += OnSceneWasLoaded;
		}

		public void Deinitialize()
		{
			if (m_Instance != this)
				return;

			SceneManager.sceneLoaded -= OnSceneWasLoaded;

			m_Instance   = null;
			m_Coroutines = null;
		}

		public void Update_Internal()
		{
			m_NextCleanupTime -= Time.unscaledDeltaTime;
			if (m_NextCleanupTime > 0)
				return;

			CleanupCoroutines();
			m_NextCleanupTime = 10f;
		}

		public static Coroutine Start(IEnumerator coroutine, bool destroyOnLoad = true)
		{
			if (IsInitialized == false)
				return null;

			Coroutine crt = m_Instance.StartCoroutine(coroutine);

			if (destroyOnLoad == true)
			{
				m_Instance.m_Coroutines.Add(crt);
			}

			return crt;
		}

		public static void Stop(Coroutine coroutine)
		{
			if (IsInitialized == false)
				return;

			m_Instance.m_Coroutines.Remove(coroutine);
			m_Instance.StopCoroutine(coroutine);
		}

		// HANDLERS

		private void OnSceneWasLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
		{
			if (mode == LoadSceneMode.Single)
			{
				for (int idx = 0; idx < m_Coroutines.Count; idx++)
				{
					var coroutine = m_Coroutines[idx];
					if (coroutine == null)
						continue;

					m_Instance.StopCoroutine(coroutine);
				}

				m_Coroutines.Clear();
			}
		}

		// PRIVATE METHODS

		private void CleanupCoroutines()
		{
			if (m_Coroutines == null)
				return;

			for (int idx = m_Coroutines.Count; idx --> 0;)
			{
				if (m_Coroutines[idx] == null)
				{
					m_Coroutines.RemoveAt(idx);
				}
			}
		}
	}
}
