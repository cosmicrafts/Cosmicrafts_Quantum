namespace TowerRush
{
	using UnityEngine;
	using UnityEngine.UI;
	using TMPro;
	using Quantum;

	using Button = UnityEngine.UI.Button;

	public class UIMenuCard : MonoBehaviour
	{
		// CONFIGURATION

		[SerializeField] TextMeshProUGUI m_CardName;
		[SerializeField] Toggle          m_InDeck;
		[SerializeField] TextMeshProUGUI m_LevelText;
		[SerializeField] Button          m_LevelUp;
		[SerializeField] Button          m_LevelDown;

		// PRIVATE MEMBERS

		private AssetRefCardSettings m_CardSettings;
		private byte                 m_CardLevel;

		// PUBLIC METHODS

		public void SetData(AssetRefCardSettings settings, byte level, bool inDeck)
		{
			m_CardSettings   = settings;
			m_CardLevel      = level;

			m_CardName.text  = UnityDB.FindAsset<CardSettingsAsset>(settings.Id).DisplayName;
			m_LevelText.text = level.ToString();
			m_InDeck.isOn    = inDeck;
		}

		public MenuCardInfo GetCardInfo()
		{
			return new MenuCardInfo
			{
				CardSettings = m_CardSettings,
				Level        = m_CardLevel,
				InDeck       = m_InDeck.isOn,
			};
		}

		// MonoBehaviour INTERFACE

		private void Awake()
		{
			m_LevelUp.onClick.AddListener(OnLevelUp);
			m_LevelDown.onClick.AddListener(OnLevelDown);
		}

		private void OnDestroy()
		{
			m_LevelUp.onClick.RemoveAllListeners();
			m_LevelDown.onClick.RemoveAllListeners();
		}

		// PRIVATE METHODS

		private void OnLevelUp()
		{
			ChangeLevel(1);
		}

		private void OnLevelDown()
		{
			ChangeLevel(-1);
		}

		private void ChangeLevel(int diff)
		{
			m_CardLevel = (byte)Mathf.Clamp(m_CardLevel + diff, 1, 20);
			m_LevelText.text = m_CardLevel.ToString();
		}
	}
}
