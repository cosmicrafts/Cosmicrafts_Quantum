namespace TowerRush
{
	using TowerRush.Core;
	using System.Collections;
	using UnityEngine;
	using UnityEngine.SceneManagement;

	public class LoadingScene : TowerRush.Core.Scene
	{
		// PRIVATE MEMBERS

		protected override void OnInitialize()
		{
			DontDestroyOnLoad(gameObject);
		}

		protected override void OnDeinitialize()
		{
			Destroy(gameObject);
		}

		protected override void OnActivate()
		{
			// Do not call, because we do NOT want to set Active state just yet
			// base.OnActivate();

			// StartCoroutine(Show_Coroutine());
			m_State = EState.Active;
		}

		protected override void OnDeactivate()
		{
			// Do not call, because we do NOT want to set Finished state just yet
			// base.OnDeactivate();

			// StartCoroutine(Hide_Coroutine());
			m_State = EState.Finished;
		}

		// Method to load a new game scene
		public IEnumerator LoadGameScene(string sceneName)
		{
			// Show loading UI
			yield return Show_Coroutine();

			// Asynchronously load the new scene
			var asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
			while (!asyncOperation.isDone)
			{
				yield return null;
			}

			// Hide loading UI after scene load is complete
			yield return Hide_Coroutine();
		}

		// PRIVATE METHODS

		private IEnumerator Show_Coroutine()
		{
			var view = Frontend.OpenView<UIViewLoading>();

			view.Show();

			while (view.FullyVisible == false)
				yield return null;

			yield return null;
			m_State = EState.Active;
		}

		private IEnumerator Hide_Coroutine()
		{
			var view = Frontend.FindView<UIViewLoading>();

			view.Hide();

			while (view.FullyHidden == false)
				yield return null;

			yield return null;
			m_State = EState.Finished;
		}
	}
}
