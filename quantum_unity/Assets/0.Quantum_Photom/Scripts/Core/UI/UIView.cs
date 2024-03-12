namespace TowerRush.Core
{
	using System;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	public class UIView : MonoBehaviour
	{
		// CONFIGURATION

		[SerializeField] Button m_ButtonClose;
		[SerializeField] bool   m_ShowFadeBack;

		// PUBLIC MEMBERS

		public UIFrontend Frontend     { get; private set; }
		public bool       IsOpen       { get { return m_State == EState.Open; } }
		public bool       ShowFadeBack { get { return m_ShowFadeBack; } }

		public Action     HasClosed;

		// PRIVATE MEMBERS

		private EState    m_State;

		// PUBLIC METHODS

		public void Close()
		{
			Frontend.CloseView(this);
		}

		// INTERNAL METHODS

		internal void Initialize_Internal(UIFrontend frontend)
		{
			if (m_State > EState.None)
				return;

			Frontend = frontend;

			if (m_ButtonClose != null)
			{
				m_ButtonClose.onClick.AddListener(Close);
			}

			OnInitialize();
			m_State = EState.Ready;
		}

		internal void Deinitialize_Internal()
		{
			if (m_State < EState.Ready || m_State >= EState.Deinitialized)
				return;

			OnDeinitialize();

			if (m_ButtonClose != null)
			{
				m_ButtonClose.onClick.RemoveListener(Close);
			}

			Frontend = null;
			m_State = EState.Deinitialized;
		}

		internal void Open_Internal()
		{
			if (m_State != EState.Ready)
				return;

			OnOpen();
			m_State = EState.Open;
		}

		internal void Close_Internal()
		{
			if (m_State != EState.Open)
				return;

			OnClosed();

			HasClosed?.Invoke();
			HasClosed = null;

			m_State = EState.Ready;
		}

		internal void CancelPressed_Internal(BaseEventData eventData)
		{
			OnCancel(eventData);
		}

		internal void Update_Internal(SceneContext context)
		{
			if (m_State != EState.Open)
				return;

			OnUpdate(context);
		}

		internal void LateUpdate_Internal(SceneContext context)
		{
			if (m_State != EState.Open)
				return;

			OnLateUpdate(context);
		}

		// VIRTUAL INTERAFCE

		protected virtual void OnInitialize()                     { }
		protected virtual void OnDeinitialize()                   { }
		protected virtual void OnOpen()                           { }
		protected virtual void OnClosed()                         { }
		protected virtual void OnVisible()                        { }
		protected virtual void OnHidden()                         { }
		protected virtual void OnUpdate(SceneContext context)     { }
		protected virtual void OnLateUpdate(SceneContext context) { }

		protected virtual void OnCancel(BaseEventData eventData)
		{
			if (m_ButtonClose == null)
				return;
			if (m_ButtonClose.isActiveAndEnabled == false)
				return;
			if (m_ButtonClose.interactable == false)
				return;

			Close();
			eventData.Use();
		}

		// MonoBehaviour INTERFACE

		protected void OnEnable()
		{
			OnVisible();
		}

		protected void OnDisable()
		{
			OnHidden();
		}

		// HELPERS

		private enum EState
		{
			None,
			Ready,
			Open,
			Deinitialized,
		}
	}
}