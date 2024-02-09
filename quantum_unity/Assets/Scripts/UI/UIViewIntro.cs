namespace TowerRush
{
	using TowerRush.Core;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	public class UIViewIntro : UIView, IPointerDownHandler, IPointerUpHandler
	{
		// CONFIGURATION

		[SerializeField] Image m_SkipProgressImage;

		// PRIVATE MEMBERS

		private float m_SkipProgress;
		private bool  m_Pressed;

		// UIView INTERFACE

		protected override void OnUpdate(SceneContext context)
		{
			base.OnUpdate(context);

			var change     = m_Pressed == true ? Time.deltaTime / 2f : -Time.deltaTime;
			m_SkipProgress = Mathf.Clamp01(m_SkipProgress + change);

			m_SkipProgressImage.fillAmount = m_SkipProgress;

			if (m_SkipProgress >= 1f)
			{
				Frontend.Scene.FinishScene();
			}
		}

		// IPointerDownHandler INTERFACE

		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			m_Pressed = true;
		}

		// IPointerUpHandler INTERFACE

		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			m_Pressed = false;
		}
	}
}
