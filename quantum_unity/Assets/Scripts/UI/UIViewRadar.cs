namespace TowerRush
{
	using Quantum;
	using System.Collections.Generic;
	using UnityEngine;
	using TowerRush.Core;

	public unsafe class UIViewRadar : UIView
	{
		// CONFIGURATION

		[SerializeField] UIUnit m_NameplateEnemy;
		[SerializeField] UIUnit m_NameplateFriend;

		// PRIVATE MEMBERS

		private UIElementCache<UIUnit>        m_EnemyNameplates;
		private UIElementCache<UIUnit>        m_FriendNameplates;

		private List<EntityComponentUnit>       m_Units     = new List<EntityComponentUnit>();

		private Camera                          m_MainCamera;
		private Camera                          m_UICamera;
		private Canvas                          m_Canvas;
		private RectTransform                   m_CanvasRectTransform;

		// UIView INTERFACE

		protected override void OnInitialize()
		{
			base.OnInitialize();

			m_EnemyNameplates  = new UIElementCache<UIUnit>(m_NameplateEnemy, 4);
			m_FriendNameplates = new UIElementCache<UIUnit>(m_NameplateFriend, 4);

			m_MainCamera = Game.Instance.MainCamera.Camera;

			m_Canvas = GetComponent<Canvas>();
			if (m_Canvas == null)
			{
				m_Canvas = GetComponentInParent<Canvas>();
			}

			m_Canvas   = m_Canvas.rootCanvas;
			m_UICamera = m_Canvas.worldCamera;

			m_CanvasRectTransform = m_Canvas.transform as RectTransform;

			Signals.EntityCreated.Connect(OnEntityCreated);
			Signals.EntityDestroyed.Connect(OnEntityDestroyed);
		}

		protected override void OnDeinitialize()
		{
			Signals.EntityCreated.Disconnect(OnEntityCreated);
			Signals.EntityDestroyed.Disconnect(OnEntityDestroyed);

			base.OnDeinitialize();
		}

		protected override void OnLateUpdate(SceneContext context)
		{
			base.OnLateUpdate(context);

			var frame             = context.Frame;
			var enemyIndex        = 0;
			var friendIndex       = 0;

			for (int idx = 0, count = m_Units.Count; idx < count; idx++)
			{
				var unit = m_Units[idx];

				var quantumHealth = frame.Unsafe.GetPointer<Health>(unit.Entity.EntityRef);
				if (quantumHealth->IsAlive == false)
					continue;

				var quantumUnit  = frame.Unsafe.GetPointer<Unit>(unit.Entity.EntityRef);
				var nameplate    = default(UIUnit);

				if (quantumUnit->Owner == Entities.LocalPlayer)
				{
					nameplate = m_FriendNameplates[friendIndex];
					friendIndex += 1;
				}
				else
				{
					nameplate   = m_EnemyNameplates[enemyIndex];
					enemyIndex += 1;
				}

				nameplate.SetData(quantumHealth, quantumUnit, unit.HUDScale);
				nameplate.transform.position = GetUIPosition(unit.HUDPivotPosition);
			}

			m_EnemyNameplates.HideAll(enemyIndex);
			m_FriendNameplates.HideAll(friendIndex);
		}

		// PRIVATE METHODS

		private void OnEntityCreated(Entity entity)
		{
			var unit = entity.GetComponent<EntityComponentUnit>();
			if (unit != null)
			{
				m_Units.AddUnique(unit);
			}
		}

		private void OnEntityDestroyed(Entity entity)
		{
			var unit = entity.GetComponent<EntityComponentUnit>();
			if (unit != null)
			{
				m_Units.Remove(unit);
			}
		}

		private Vector3 GetUIPosition(Vector3 worldPosition)
		{
			var screenPoint = m_MainCamera.WorldToScreenPoint(worldPosition);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(m_CanvasRectTransform, screenPoint, m_UICamera, out Vector2 screenPosition);
			return m_CanvasRectTransform.TransformPoint(screenPosition);
		}
	}
}