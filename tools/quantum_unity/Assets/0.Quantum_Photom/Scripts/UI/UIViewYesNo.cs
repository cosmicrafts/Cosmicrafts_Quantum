namespace TowerRush
{
	using UnityEngine;
	using UnityEngine.UI;
	using TowerRush.Core;
	using TMPro;
	using System;

	public class UIViewYesNo : UIView
	{
		// CONFIGURATION

		[SerializeField] TextMeshProUGUI m_DescriptionText;
		[SerializeField] TextMeshProUGUI m_YesText;
		[SerializeField] TextMeshProUGUI m_NoText;

		[SerializeField] Button          m_YesButton;

		// PUBLIC MEMBERS

		public new Action<bool> HasClosed;

		// PRIVATE MEMBERS

		private bool            m_Result;

		// PUBLIC METHODS

		public void SetData(string description, string yes, string no)
		{
			m_DescriptionText.text = description;
			m_YesText.text         = yes;
			m_NoText.text          = no;
		}

		// UIView INTERFACE

		protected override void OnInitialize()
		{
			base.OnInitialize();

			m_YesButton.onClick.AddListener(OnYesButton);

			gameObject.SetActive(false);
		}

		protected override void OnDeinitialize()
		{
			m_YesButton.onClick.RemoveAllListeners();

			base.OnDeinitialize();
		}

		protected override void OnOpen()
		{
			base.OnOpen();

			m_Result = false;
		}

		protected override void OnClosed()
		{
			base.OnClosed();

			HasClosed?.Invoke(m_Result);
		}

		// PRIVATE METHODS

		private void OnYesButton()
		{
			m_Result = true;

			Close();
		}
	}
}