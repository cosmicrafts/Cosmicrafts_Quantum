namespace TowerRush.Core
{
	using Quantum;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.EventSystems;
	
	using Input = UnityEngine.Input;

	public unsafe class UIFrontend : MonoBehaviour, IUpdatableSceneComponent, ILateUpdatableSceneComponent
	{
		// CONFIGURATION

		[SerializeField] Canvas        m_Canvas;
		[SerializeField] RectTransform m_FadeBack;
		[SerializeField] UIView[]      m_DefaultViews;

		// PUBLIC MEMBERS

		public Canvas Canvas { get { return m_Canvas; } }
		public Scene  Scene  { get; private set; }

		// PRIVATE MEMBERS

		private List<UIView> m_Views       = new List<UIView>(16);
		private List<UIView> m_OpenedViews = new List<UIView>(16);

		private BaseEventData        m_CachedEventData;
		private UIView               m_LastDefaultView;

		// PUBLIC METHODS

		public T OpenView<T>(string name = "") where T : UIView
		{
			var view = FindView<T>(name);
			if (view == null)
				return null;

			return OpenView(view);
		}

		public T OpenView<T>(T view)  where T : UIView
		{
			if (m_OpenedViews.AddUnique(view) == false)
				return view;

			view.Open_Internal();
			view.SetActive(true);

			UpdateViewOrder();
			return view;
		}

		public bool CloseView<T>(string name = "") where T : UIView
		{
			var view = FindView<T>(name);
			if (view == null)
				return false;

			return CloseView(view);
		}

		public bool CloseView<T>(T view)  where T : UIView
		{
			if (m_OpenedViews.Remove(view) == false)
				return false;

			view.SetActive(false);
			view.Close_Internal();

			UpdateViewOrder();
			return true;
		}

		public T FindView<T>(string name = "") where T : UIView
		{
			if (name.IsNullOrEmpty() == true)
			{
				for (int idx = 0, count = m_Views.Count; idx < count; idx++)
				{
					var view = m_Views[idx];
					if (view is T tView)
						return tView;
				}
			}
			else
			{
				for (int idx = 0, count = m_Views.Count; idx < count; idx++)
				{
					var view = m_Views[idx];
					if (view is T tView && view.name == name)
						return tView;
				}
			}

			return null;
		}

		// ISceneComponent INTERFACE

		void ISceneComponent.Initialize(Scene scene)
		{
			Scene = scene;

			m_CachedEventData = new BaseEventData(EventSystem.current);

			GetComponentsInChildren(true, m_Views);

			Game.Instance.MainCamera.AddOverlayCamera(m_Canvas.worldCamera, m_Canvas.sortingOrder);

			for (int idx = 0, count = m_Views.Count; idx < count; idx++)
			{
				var view = m_Views[idx];
				view.SetActive(true);
				view.Initialize_Internal(this);
				view.SetActive(false);
			}

			for (int idx = 0, count = m_DefaultViews.Length; idx < count; idx++)
			{
				OpenView(m_DefaultViews[idx]);
			}

			m_LastDefaultView = m_DefaultViews.LastOrDefault();
		}

		void ISceneComponent.Deinitialize()
		{
			for (int idx = m_OpenedViews.Count; idx --> 0;)
			{
				CloseView(m_OpenedViews[idx]);
			}

			Game.Instance.MainCamera.RemoveOverlayCamera(m_Canvas.worldCamera);

			for (int idx = 0, count = m_Views.Count; idx < count; idx++)
			{
				m_Views[idx].Deinitialize_Internal();
			}

			m_OpenedViews.Clear();
			m_Views.Clear();

			Scene = null;
		}

		// IUpdatableSceneComponent INTERFACE

		void IUpdatableSceneComponent.OnUpdate(SceneContext context)
		{
			ProcessInput();

			for (int idx = 0, count = m_OpenedViews.Count; idx < count; idx++)
			{
				m_OpenedViews[idx].Update_Internal(context);
			}
		}

		// ILateUpdatableSceneComponent INTERFACE

		void ILateUpdatableSceneComponent.OnLateUpdate(SceneContext context)
		{
			for (int idx = 0, count = m_OpenedViews.Count; idx < count; idx++)
			{
				m_OpenedViews[idx].LateUpdate_Internal(context);
			}
		}

		// PRIVATE METHODS

		private void UpdateViewOrder()
		{
			var lastShowFade = -1;

			for (int idx = 0, count = m_OpenedViews.Count; idx < count; idx++)
			{
				var view = m_OpenedViews[idx];
				view.transform.SetSiblingIndex(idx);

				if (view.ShowFadeBack == true)
				{
					lastShowFade = idx;
				}
			}

			if (m_FadeBack != null)
			{
				if (lastShowFade >= 0)
				{
					m_FadeBack.SetSiblingIndex(lastShowFade);
					m_FadeBack.SetActive(true);
				}
				else
				{
					m_FadeBack.SetActive(false);
				}
			}
		}

		private void ProcessInput()
		{
			if (Input.GetButtonDown("Cancel") == true) 
			{
				// HACK - ICancelHandler.OnCancel can't be used because it is
				// send to EventSystem.current.currentSelectedGameObject only :(

				for (int idx = m_OpenedViews.Count; idx --> 0;)
				{
					m_OpenedViews[idx].CancelPressed_Internal(m_CachedEventData);

					if (m_CachedEventData.used == true)
						break;
				}

				if (m_CachedEventData.used == false && m_OpenedViews.LastOrDefault() == m_LastDefaultView)
				{
					OpenView<UIViewPauseMenu>();
				}

				m_CachedEventData.Reset();
			}
		}
	}
}