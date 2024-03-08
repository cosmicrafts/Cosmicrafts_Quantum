using System;
using Photon.Deterministic;

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
		[SerializeField] TMP_InputField  m_HpSlot;
		[SerializeField] TMP_InputField  m_DmgSlot;
		
		// PRIVATE MEMBERS

		private AssetRefCardSettings m_CardSettings;
		private byte                 m_CardLevel;
		private float                m_CardHp;
		private float                m_CardDmg;

		// PUBLIC METHODS

		public void SetData(AssetRefCardSettings settings, MenuCardInfo card, bool inDeck)
		{
			m_CardSettings   = settings;
			m_CardLevel      = card.Level;
			m_CardHp         = card.Hp;
			m_CardDmg        = card.Dmg;

			m_CardName.text  = UnityDB.FindAsset<CardSettingsAsset>(settings.Id).DisplayName;
			m_LevelText.text = card.Level.ToString();
			m_HpSlot.text    = card.Hp.ToString();
			m_DmgSlot.text   = card.Dmg.ToString();
			
			m_InDeck.isOn    = inDeck;
		}

		public MenuCardInfo GetCardInfo()
		{
			return new MenuCardInfo
			{
				CardSettings = m_CardSettings,
				Level        = m_CardLevel,
				InDeck       = m_InDeck.isOn,
				Hp           = m_CardHp,
				Dmg          = m_CardDmg,
			};
		}

		// MonoBehaviour INTERFACE

		private void Awake()
		{
			m_LevelUp.onClick.AddListener(OnLevelUp);
			m_LevelDown.onClick.AddListener(OnLevelDown);
			m_HpSlot.onValueChanged.AddListener(ChangeHp);
			m_DmgSlot.onValueChanged.AddListener(ChangeDmg);
		}

		private void OnDestroy()
		{
			m_LevelUp.onClick.RemoveAllListeners();
			m_LevelDown.onClick.RemoveAllListeners();
			m_HpSlot.onValueChanged.RemoveAllListeners();
			m_DmgSlot.onValueChanged.RemoveAllListeners();
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
		
		public void ChangeHp(string number)
		{
			m_CardHp = float.Parse(number);
		}
		public void ChangeDmg(string number)
		{
			m_CardDmg= float.Parse(number);
		}
	}
}
