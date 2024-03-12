namespace TowerRush
{
	using Quantum;
	using System.Collections.Generic;
	using System.Reflection;
	using TowerRush.Core;
	using UnityEngine;
	using UnityEngine.EventSystems;

	public unsafe class CardManager : MonoBehaviour, IUpdatableSceneComponent
	{
		// CONFIGURATION

		[SerializeField] BlockAreaEntityPair[] m_AlphaBlockAreas;
		[SerializeField] BlockAreaEntityPair[] m_BetaBlockAreas;

		[SerializeField] Vector2    m_GridSizeX;
		[SerializeField] Vector2    m_GridSizeZ;

		[SerializeField] GameObject m_FallbackGhost;

		// PUBLIC MEMBERS

		public CardSettingsAsset ActiveCardSettings { get; set; }

		// PRIVATE MEMBERS

		private Plane                             m_Plane;
		private Dictionary<AssetGuid, GameObject> m_UnitGhosts = new Dictionary<AssetGuid, GameObject>();
		private GameObject                        m_CurrentGhost;
		private bool                              m_CursorOverUI;
		private int                               m_UILayer;
		private FieldInfo                         m_FocusedObjectField;

		// PUBLIC METHODS

		public unsafe void UseCard()
		{
			if (ActiveCardSettings == null)
				return;
			if (m_CursorOverUI == true)
				return;
			if (Entities.LocalPlayerEntity.IsValid == false)
				return;

			var qGame        = QuantumRunner.Default.Game;
			var frame        = qGame.Frames.Predicted;
			var qCardManager = frame.Unsafe.GetPointer<Quantum.CardManager>(Entities.LocalPlayerEntity);

			if (qCardManager->CurrentEnergy < ActiveCardSettings.GetEnergyCost())
				return;

			var cardIndex = -1;
			var cards     = frame.ResolveList(qCardManager->AvailableCards);

			for (int idx = 0, count = cards.Count; idx < count; idx++)
			{
				if (cards.GetPointer(idx)->CardSettings.Id == ActiveCardSettings.GetAssetGuid())
				{
					cardIndex = idx;
					break;
				}
			}

			if (cardIndex < 0)
				return;

			qGame.SendCommand(Entities.LocalPlayer, new Quantum.UseCardCommand()
			{
				Position  = GetClampedCursorPosition().ToFPVector2(),
				CardIndex = (byte)cardIndex,
			});
		}

		// ISceneComponent INTERFACE

		void ISceneComponent.Initialize(Scene scene)
		{
			m_Plane = new Plane(Vector3.up, Vector3.zero);

			m_GridSizeX.x += 0.0001f;
			m_GridSizeX.y -= 0.0001f;
			m_GridSizeZ.x += 0.0001f;
			m_GridSizeZ.y -= 0.0001f;

			m_UILayer            = UnityEngine.LayerMask.NameToLayer("UI");
			m_FocusedObjectField = typeof(StandaloneInputModule).GetField("m_CurrentFocusedGameObject", BindingFlags.NonPublic | BindingFlags.Instance);

			m_FallbackGhost.SetActive(false);
		}

		void ISceneComponent.Deinitialize()
		{
		}

		void IUpdatableSceneComponent.OnUpdate(SceneContext context)
		{
			if (ActiveCardSettings == null || ActiveCardSettings is EffectAreaSettingsAsset)
			{
				HideAllBlockAreas(context);
			}
			else
			{
				ShowBlockAreas(context, m_AlphaBlockAreas, Entities.LocalPlayer == 1);
				ShowBlockAreas(context, m_BetaBlockAreas,  Entities.LocalPlayer == 0);
			}

			m_CursorOverUI = false;
			if (EventSystem.current.IsPointerOverGameObject())
			{
				var focusedObject = m_FocusedObjectField.GetValue(EventSystem.current.currentInputModule) as GameObject;
				if (focusedObject != null)
				{
					if (focusedObject.layer == m_UILayer)
					{
						m_CursorOverUI = true;
					}
				}
			}

			if (ActiveCardSettings == null || m_CursorOverUI == true)
			{
				if (m_CurrentGhost != null)
				{
					m_CurrentGhost.SetActive(false);
					m_CurrentGhost = null;
				}
			}
			else
			{
				if (m_CurrentGhost == null && m_UnitGhosts.TryGetValue(ActiveCardSettings.AssetObject.Guid, out m_CurrentGhost) == false)
				{
					if (ActiveCardSettings.GhostPrefab != null)
					{
						m_CurrentGhost = Instantiate(ActiveCardSettings.GhostPrefab);
						m_UnitGhosts[ActiveCardSettings.AssetObject.Guid] = m_CurrentGhost;
					}
					else
					{
						m_CurrentGhost = m_FallbackGhost;
					}
				}

				m_CurrentGhost.transform.position = GetClampedCursorPosition();
				m_CurrentGhost.SetActive(true);
			}
		}

		private Vector3 GetClampedCursorPosition()
		{
			var camera = Game.Instance.MainCamera.Camera;
			var ray    = camera.ScreenPointToRay(UnityEngine.Input.mousePosition);

			if (m_Plane.Raycast(ray, out var distance) == false)
				return default;

			var position = ray.origin + ray.direction * distance;

			position.x = Mathf.Clamp(position.x, m_GridSizeX.x, m_GridSizeX.y);
			position.z = Mathf.Clamp(position.z, m_GridSizeZ.x, m_GridSizeZ.y);

			if (position.x > 0f)
			{
				position.x = (int)position.x + 0.5f;
			}
			else
			{
				position.x = (int)position.x - 0.5f;
			}

			if (position.z > 0f)
			{
				position.z = (int)position.z + 0.5f;
			}
			else
			{
				position.z = (int)position.z - 0.5f;
			}

			if (ActiveCardSettings is EffectAreaSettingsAsset)
				return position;

			var frame     = QuantumRunner.Default.Game.Frames.Predicted;
			var qGameplay = frame.Unsafe.GetPointerSingleton<Quantum.Gameplay>();
			var area      = Entities.LocalPlayer == 0 ? qGameplay->AlphaArea : qGameplay->BetaArea;

			if (area.IsValidUnitPosition(frame, position.ToFPVector2()) == true)
				return position;

			var range = 1;

			while (range < 50)
			{
				var newPosition = position;

				// negative X
				newPosition.x = position.x - range;
				if (area.IsValidUnitPosition(frame, newPosition.ToFPVector2()) == true)
					return newPosition;

				// positive X
				newPosition.x = position.x + range;
				if (area.IsValidUnitPosition(frame, newPosition.ToFPVector2()) == true)
					return newPosition;

				// negative Y
				newPosition.x = position.x;
				newPosition.z = position.z - range;
				if (area.IsValidUnitPosition(frame, newPosition.ToFPVector2()) == true)
					return newPosition;

				// positive Y
				newPosition.z = position.z + range;
				if (area.IsValidUnitPosition(frame, newPosition.ToFPVector2()) == true)
					return newPosition;

				for (int idx = 1; idx <= range; idx++)
				{
					// negative X
					newPosition.x = position.x - range;
					newPosition.z = position.z - idx;
					if (area.IsValidUnitPosition(frame, newPosition.ToFPVector2()) == true)
						return newPosition;

					newPosition.z = position.z + idx;
					if (area.IsValidUnitPosition(frame, newPosition.ToFPVector2()) == true)
						return newPosition;

					// positive X
					newPosition.x = position.x + range;
					newPosition.z = position.z - idx;
					if (area.IsValidUnitPosition(frame, newPosition.ToFPVector2()) == true)
						return newPosition;

					newPosition.z = position.z + idx;
					if (area.IsValidUnitPosition(frame, newPosition.ToFPVector2()) == true)
						return newPosition;

					if (idx < range)
					{
						// negative Y
						newPosition.z = position.z - range;
						newPosition.x = position.x - idx;
						if (area.IsValidUnitPosition(frame, newPosition.ToFPVector2()) == true)
							return newPosition;

						newPosition.x = position.x + idx;
						if (area.IsValidUnitPosition(frame, newPosition.ToFPVector2()) == true)
							return newPosition;

						// positive Y
						newPosition.z = position.z + range;
						newPosition.x = position.x - idx;
						if (area.IsValidUnitPosition(frame, newPosition.ToFPVector2()) == true)
							return newPosition;

						newPosition.x = position.x + idx;
						if (area.IsValidUnitPosition(frame, newPosition.ToFPVector2()) == true)
							return newPosition;
					}
				}

				range += 1;
			}

			return position;
		}

		private void HideAllBlockAreas(SceneContext context)
		{
			ShowBlockAreas(context, m_AlphaBlockAreas, false);
			ShowBlockAreas(context, m_BetaBlockAreas,  false);
		}

		private void ShowBlockAreas(SceneContext context, BlockAreaEntityPair[] areas, bool show)
		{
			for (int idx = 0, count = areas.Length; idx < count; idx++)
			{
				var area = areas[idx];
				if (show == false || area.Unit == null || area.Unit.IsAlive == true)
				{
					area.Area.SetActive(show);
				}
			}
		}

		[System.Serializable]
		private struct BlockAreaEntityPair
		{
			public GameObject          Area;
			public EntityComponentUnit Unit;
		}
	}
}
