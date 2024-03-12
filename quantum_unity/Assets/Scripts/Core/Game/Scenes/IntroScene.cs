namespace TowerRush
{
	using TowerRush.Core;
	using System.Collections;

	public class IntroScene : Scene
	{
		// PUBLIC METHODS

		public static bool CanShowIntro()
		{
#if ENABLE_CHEAT_MANAGER
			if (CheatManager.Instance.IntroBehavior == CheatManager.EIntroBehavior.Always)
				return true;
			else if (CheatManager.Instance.IntroBehavior == CheatManager.EIntroBehavior.Never)
				return false;
#endif

			return DictionaryFile.Exists("Intro") == false;
		}

		// Scene INTERFACE

		protected override void OnInitialize()
		{
		}

		protected override void OnDeinitialize()
		{
		}

		protected override void OnActivate()
		{
			base.OnActivate();

			StartCoroutine(PlayIntro_Coroutine());

			DictionaryFile.Load("Intro");
		}

		// PRIVATE METHODS

		private IEnumerator PlayIntro_Coroutine()
		{
			yield return WaitFor.Seconds(10f);

			FinishScene();
		}
	}
}