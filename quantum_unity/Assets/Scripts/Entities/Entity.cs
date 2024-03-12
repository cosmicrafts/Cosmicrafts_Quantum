namespace TowerRush
{
	using UnityEngine;
	using System.Collections.Generic;
	using TowerRush.Core;

	public class Entity : EntityView
	{
		// CONFIGURATION

		[Min(0f), Header("Entity")]
		public float   DestroyDelay;
		public float   DestroyMoveDelay;
		public Vector3 DestroyMoveOffset;

		// PUBLIC MEMBERS

		[HideInInspector]
		public Scene Scene;

		// PRIVATE MEMBERS

		private List<EntityComponent> m_Components = new List<EntityComponent>();

		// PUBLIC METHODS

		public void Initialize(Scene scene)
		{
			Scene = scene;

			GetComponentsInChildren(true, m_Components);

			for (int idx = 0, count = m_Components.Count; idx < count; idx++)
			{
				m_Components[idx].Initialize_Internal(this);
			}
		}

		public void Deinitialize()
		{
			for (int idx = 0, count = m_Components.Count; idx < count; idx++)
			{
				m_Components[idx].Deinitialize_Internal();
			}

			m_Components.Clear();
		}

		public void Activate()
		{
			for (int idx = 0, count = m_Components.Count; idx < count; idx++)
			{
				m_Components[idx].Activate_Internal();
			}
		}

		public void Deactivate()
		{
			for (int idx = 0, count = m_Components.Count; idx < count; idx++)
			{
				m_Components[idx].Deactivate_Internal();
			}
		}

		public void Update_Internal(SceneContext context)
		{
			for (int idx = 0, count = m_Components.Count; idx < count; idx++)
			{
				m_Components[idx].Update_Internal(context);
			}
		}

		public void LateUpdate_Internal(SceneContext context)
		{
			for (int idx = 0, count = m_Components.Count; idx < count; idx++)
			{
				m_Components[idx].LateUpdate_Internal(context);
			}
		}
	}
}