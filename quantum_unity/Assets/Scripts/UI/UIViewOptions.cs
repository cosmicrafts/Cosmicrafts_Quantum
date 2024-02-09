namespace TowerRush
{
	using TowerRush.Core;
	using UnityEngine;
	using UnityEngine.UI;

	public class UIViewOptions : UIView
	{
		// CONFIGURATION

		[SerializeField] Slider m_MusicSlider;
		[SerializeField] Slider m_SoundsSlider;

		// UIView INTERFACE
		protected override void OnInitialize()
		{
			base.OnInitialize();

			m_MusicSlider.onValueChanged.AddListener(OnMusicSlider);
			m_SoundsSlider.onValueChanged.AddListener(OnSoundsSlider);

			m_MusicSlider.value  = GameOptions.MusicVolume  * 10;
			m_SoundsSlider.value = GameOptions.SoundsVolume * 10;
		}

		protected override void OnDeinitialize()
		{
			m_MusicSlider.onValueChanged.RemoveAllListeners();
			m_SoundsSlider.onValueChanged.RemoveAllListeners();

			base.OnDeinitialize();
		}

		// PRIVATE METHODS

		private void OnMusicSlider(float value)
		{
			value *= 0.1f;

			Game.Instance.AudioService.SetMusicVolume(value);

			GameOptions.MusicVolume = value;
			GameOptions.Save();
		}

		private void OnSoundsSlider(float value)
		{
			value *= 0.1f;

			Game.Instance.AudioService.SetSoundsVolume(value);

			GameOptions.SoundsVolume = value;
			GameOptions.Save();
		}
	}
}
