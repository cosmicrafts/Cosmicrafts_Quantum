namespace TowerRush
{
	using TowerRush.Core;
	using System.Collections;

	public class LoadingScene : Scene
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
		//	Do not call, because we do NOT want to se Active state just yet
		//	base.OnActivate();

			StartCoroutine(Show_Coroutine());
		}

		protected override void OnDeactivate()
		{
		//	Do not call, because we do NOT want to se Finished state just yet
		//	base.OnDeactivate();

			StartCoroutine(Hide_Coroutine());
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