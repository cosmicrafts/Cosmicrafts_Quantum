namespace TowerRush
{
	using Photon.Deterministic;
	using Quantum;
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;

	public class UIUnit : MonoBehaviour
	{
		// CONFIGURATION

		[SerializeField] TextMeshProUGUI m_HealthText;
		[SerializeField] TextMeshProUGUI m_LevelText;
		[SerializeField] TextMeshProUGUI m_UnitName;
		[SerializeField] Image           m_HealthProgressBar;
		[SerializeField] Image           m_ShieldProgressBar;
		[SerializeField] Image           m_ActivationProgress;
		[SerializeField] FP              m_MinimumActivationToShow;
		[SerializeField] bool            m_ShowHealthBarWhenFull;

		// PRIVATE MEMBERS

		private FP                   m_LastHealth;
		private FP                   m_LastMaxHealth;
		private FP                   m_LastShield;
		private FP                   m_LastMaxShield;
		private byte                 m_LastLevel;
		private UnitSettingsAsset    m_Settings;
		private AssetRefCardSettings m_SettingsRef;
		private float                m_DefaultWidth;
		private RectTransform        m_RectTransform;

		// PUBLIC METHODS

		public unsafe void SetData(Health* quantumHealth, Unit* unit, float scale)
		{
			var level = unit->Level;

			if (m_LevelText != null && m_LastLevel != level)
			{
				m_LevelText.text = level.ToString();
				m_LastLevel      = level;
			}

			if (m_SettingsRef != unit->Settings)
			{
				m_SettingsRef   = unit->Settings;
				m_Settings      = UnityDB.FindAsset<UnitSettingsAsset>(m_SettingsRef.Id);
				m_UnitName.text = m_Settings.DisplayName;
			}

			if (m_ActivationProgress != null)
			{
				if (unit->ActivationDelay <= FP._0)
				{
					m_ActivationProgress.SetActive(false);
				}
				else
				{
					if (m_Settings.Settings.ActivationDelay >= m_MinimumActivationToShow)
					{
						m_ActivationProgress.SetActive(true);
						m_ActivationProgress.fillAmount = 1f - (unit->ActivationDelay / m_Settings.Settings.ActivationDelay).AsFloat;
					}
					else
					{
						m_ActivationProgress.SetActive(false);
					}
				}
			}

			if (m_LastHealth == quantumHealth->CurrentHealth && m_LastMaxHealth == quantumHealth->MaxHealth &&
			    m_LastShield == quantumHealth->CurrentShield && m_LastMaxShield == quantumHealth->MaxShield)
				return;

			if (m_ShowHealthBarWhenFull == false && quantumHealth->CurrentHealth == quantumHealth->MaxHealth)
			{
				m_HealthProgressBar.SetActive(false);
				m_HealthText.SetActive(false);
				SetWidth(0);
				return;
			}

			SetWidth(m_DefaultWidth * scale);
			m_HealthProgressBar.SetActive(true);
			m_HealthText.SetActive(true);
			m_ShieldProgressBar.SetActive(true);

			var health    = Mathf.Ceil(quantumHealth->CurrentHealth.AsFloat);
			var maxHealth = Mathf.Ceil(quantumHealth->MaxHealth.AsFloat);
			
			var shield    = Mathf.Ceil(quantumHealth->CurrentShield.AsFloat);
			var maxShield = Mathf.Ceil(quantumHealth->MaxShield.AsFloat);

			if (m_HealthText != null)
			{
				m_HealthText.text = $"{health}/{maxHealth}";
			}

			if (m_HealthProgressBar != null)
			{
				m_HealthProgressBar.fillAmount = health / maxHealth;
			}
			
			if (m_ShieldProgressBar != null)
			{
				m_ShieldProgressBar.fillAmount = shield / maxShield;
			}

			m_LastHealth    = quantumHealth->CurrentHealth;
			m_LastMaxHealth = quantumHealth->MaxHealth;
			
			m_LastShield    = quantumHealth->CurrentShield;
			m_LastMaxShield = quantumHealth->MaxShield;
		}

		// MonoBehaviour INTERFACE

		private void Awake()
		{
			m_ActivationProgress.SetActive(false);
			m_RectTransform = transform as RectTransform;
			m_DefaultWidth = m_RectTransform.rect.width;
		}

		// PRIVATE METHODS

		private void SetWidth(float width)
		{
			var size = m_RectTransform.sizeDelta;
			size.x = width;
			m_RectTransform.sizeDelta = size;
		}
	}
}