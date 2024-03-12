namespace TowerRush
{
	using TowerRush.Core;

	public class StartScene : Scene
	{
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

			FinishScene();
		}
	}
}