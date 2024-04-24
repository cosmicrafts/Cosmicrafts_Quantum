namespace TowerRush
{
	using Photon.Deterministic;
	using Quantum;
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;

	public class UIUnit : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_HealthText;
    [SerializeField] TextMeshProUGUI m_LevelText;
    [SerializeField] TextMeshProUGUI m_UnitName;
    [SerializeField] Image m_HealthProgressBar;
    [SerializeField] Image m_ShieldProgressBar;
    [SerializeField] Image m_ActivationProgress;
    [SerializeField] FP m_MinimumActivationToShow;
    [SerializeField] bool m_ShowHealthBarWhenFull;
    private FP m_LastHealth;
    private FP m_LastMaxHealth;
    private FP m_LastShield;
    private FP m_LastMaxShield;
    private byte m_LastLevel;
    private UnitSettingsAsset m_Settings;
    private AssetRefCardSettings m_SettingsRef;
    private float m_DefaultWidth;
    private RectTransform m_RectTransform;

    public unsafe void SetData(Health* quantumHealth, Unit* unit, float scale)
    {
        var level = unit->Level;

        if (m_LevelText != null && m_LastLevel != level)
        {
            m_LevelText.text = level.ToString();
            m_LastLevel = level;
        }

        if (m_SettingsRef != unit->Settings)
        {
            m_SettingsRef = unit->Settings;
            m_Settings = UnityDB.FindAsset<UnitSettingsAsset>(m_SettingsRef.Id);
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
            SetWidth(m_DefaultWidth); // Set a fixed width
            return;
        }

        // Animate health and shield bars
        AnimateBar(m_HealthProgressBar, m_LastHealth, quantumHealth->CurrentHealth, quantumHealth->MaxHealth);
        AnimateBar(m_ShieldProgressBar, m_LastShield, quantumHealth->CurrentShield, quantumHealth->MaxShield);

        m_LastHealth = quantumHealth->CurrentHealth;
        m_LastMaxHealth = quantumHealth->MaxHealth;
        m_LastShield = quantumHealth->CurrentShield;
        m_LastMaxShield = quantumHealth->MaxShield;
    }

    private void AnimateBar(Image bar, FP oldValue, FP newValue, FP maxValue)
    {
        if (bar == null)
            return;

        float oldFillAmount = oldValue.AsFloat / maxValue.AsFloat;
        float newFillAmount = newValue.AsFloat / maxValue.AsFloat;

        if (newFillAmount < oldFillAmount)
        {
            // Use LeanTween for interpolation, adjust duration as needed
            LeanTween.value(bar.gameObject, oldFillAmount, newFillAmount, 0.25f)
                .setOnUpdate((float val) =>
                {
                    bar.fillAmount = val;
                });
        }
        else
        {
            bar.fillAmount = newFillAmount;
        }
    }

    private void Awake()
    {
        m_ActivationProgress.SetActive(false);
        m_RectTransform = transform as RectTransform;
        m_DefaultWidth = m_RectTransform.rect.width;
    }

    private void SetWidth(float width)
    {
        var size = m_RectTransform.sizeDelta;
        size.x = width;
        m_RectTransform.sizeDelta = size;
    }
}}