namespace TowerRush
{
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.EventSystems;
	using TMPro;
	using System.Collections;
	using Photon.Deterministic;

	public class UICard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		// CONFIGURATION

		[SerializeField] Image           m_Image;
		[SerializeField] TextMeshProUGUI m_EnergyCost;
		[SerializeField] TextMeshProUGUI m_CardName;
		[SerializeField] RectTransform   m_Frame;
		[SerializeField] Image           m_EnoughEnergy;
		[SerializeField] bool            m_ScaleOnSet;
		[SerializeField] GameObject[]    m_Rarities;

		// PUBLIC MEMBERS

		public  CardSettingsAsset           Settings { get; private set; }

		// PRIVATE METHODS

		private UICardManager.CardCallbacks m_Callbacks;

		// PUBLIC METHODS

		public void SetData(CardSettingsAsset settings, UICardManager.CardCallbacks callbacks)
		{
			if (settings == null)
			{
				m_Frame.SetActive(false);
				m_Callbacks = callbacks;
				Settings    = null;
				return;
			}

			m_Frame.SetActive(true);

			m_Callbacks    = callbacks;
			m_Image.sprite = settings.Sprite;

			if (m_EnergyCost != null)
			{
				m_EnergyCost.text = settings.GetEnergyCost().ToString();
			}

			if (m_CardName != null)
			{
				m_CardName.text = settings.DisplayName;
			}

			if (m_ScaleOnSet == true && settings != Settings)
			{
				StartCoroutine(ScaleFrame_Coroutine());
			}

			var rarity = (int)settings.GetRarity();
			for (int idx = 0, count = m_Rarities.Length; idx < count; idx++)
			{
				m_Rarities[idx].SetActive(rarity == idx);
			}

			Settings = settings;

			m_EnoughEnergy.SetActive(false);
		}

		public void SetEnergy(float energy)
		{
			if (m_EnoughEnergy == null)
				return;
			if (Settings == null)
			{
				m_EnoughEnergy.SetActive(false);
				return;
			}

			m_EnoughEnergy.fillAmount = 1f - energy / Settings.GetEnergyCost();
			m_EnoughEnergy.SetActive(true);
		}

		public void ShowFrame(bool show)
		{
			m_Frame.SetActive(show);
		}

		// IPointerDownHandler INTERFACE

		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			m_Callbacks?.OnPointerDown(this);
		}

		// IPointerUpHandler INTERFACE

		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			m_Callbacks?.OnPointerUp(this);
		}

		// PRIVATE METHODS

	private IEnumerator ScaleFrame_Coroutine()
    {
            m_Frame.localScale = Vector3.zero;

            var duration = 0.45f; // Adjust animation duration
            var originalScale = m_Frame.localScale;
            var targetScale = Vector3.one;
            var bounceHeight = 0.75f; // Adjust bounce height
            var progress = 0.05f;

            while (progress < 1f)
            {
                progress += Time.deltaTime / duration;
                var t = Mathf.Sin(progress * Mathf.PI) * bounceHeight;
                m_Frame.localScale = Vector3.Lerp(originalScale, targetScale, progress) + new Vector3(t, t, 0); // Add bounce effect
                yield return null;
            }

            m_Frame.localScale = targetScale;
        }
    }
}
