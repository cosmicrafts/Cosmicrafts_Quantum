namespace TowerRush.Core
{
	using UnityEngine;
	
	public class EntityComponent : MonoBehaviour
	{
		// PUBLIC MEMBERS
	
		public new Transform transform
		{ 
			get
			{
				if (m_TransformCached == true)
					return m_TransformCache;
	
				m_TransformCached = true;
				return m_TransformCache = base.transform;
			}
		}
	
		public new GameObject gameObject
		{ 
			get
			{
				if (m_GameObjectCached == true)
					return m_GameObjectCache;
	
				m_GameObjectCached = true;
				return m_GameObjectCache = base.gameObject;
			}
		}
	
		public new string name
		{ 
			get
			{
				if (m_NameCached == true)
					return m_NameCache;
	
				m_NameCached = true;
				return m_NameCache = base.name;
			}
		}
	
		public Entity Entity { get; private set; }
	
		// PRIVATE MEMBERS
	
		private Transform  m_TransformCache;
		private GameObject m_GameObjectCache;
		private string     m_NameCache;
	
		private bool       m_TransformCached;
		private bool       m_GameObjectCached;
		private bool       m_NameCached;

		// PUBLIC METHODS

		public void Initialize_Internal(Entity entity)
		{
			Entity = entity;
	
			OnInitialize();
		}

		public void Deinitialize_Internal()
		{
			OnDeinitialize();
		}

		public void Activate_Internal()
		{
			OnActivate();
		}

		public void Deactivate_Internal()
		{
			OnDeactivate();
		}

		public void Update_Internal(SceneContext context)
		{
			OnUpdate(context);
		}

		public void LateUpdate_Internal(SceneContext context)
		{
			OnLateUpdate(context);
		}

		// VIRTUAL INTERFACE

		protected virtual void OnInitialize()                     { }
		protected virtual void OnDeinitialize()                   { }
		protected virtual void OnActivate()                       { }
		protected virtual void OnDeactivate()                     { }
		protected virtual void OnUpdate(SceneContext context)     { }
		protected virtual void OnLateUpdate(SceneContext context) { }
	}
}