namespace TowerRush
{
	using TowerRush.Core;
	using UnityEngine;
	using UnityEngine.UI;

	public class UIViewPauseMenu : UIView
	{
		// CONFIGURATION

		[SerializeField] Button    m_ButtonOptions;
		[SerializeField] Button    m_ButtonQuit;

		// UIView INTERFACE

		protected override void OnInitialize()
		{
			base.OnInitialize();

			m_ButtonOptions.onClick.AddListener(OnOptionsButton);
			m_ButtonQuit.onClick.AddListener(OnQuitButton);
		}

		protected override void OnDeinitialize()
		{
			m_ButtonOptions.onClick.RemoveAllListeners();
			m_ButtonQuit.onClick.RemoveAllListeners();

			base.OnDeinitialize();
		}

		// PRIVATE METHODS

		private void OnOptionsButton()
		{
			Frontend.OpenView<UIViewOptions>();
		}

		private void OnQuitButton()
		{
			if (Frontend.Scene is GameScene)
			{
				Frontend.Scene.FinishScene();
			}
			else
			{
				Game.QuitGame();
			}
		}
	}
}
