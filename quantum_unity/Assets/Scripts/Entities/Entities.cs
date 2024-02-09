namespace TowerRush
{
	using TowerRush.Core;
	using UnityEngine;
	using System.Collections.Generic;
	using Quantum;
	using Quantum.Demo;

	using EntityView = global::EntityView;

	public unsafe class Entities : EntityViewUpdater, IUpdatableSceneComponent, ILateUpdatableSceneComponent
	{
		// PUBLIC MEMBERS

		#warning Get rid of all these ugly Singletons
		public static Entities Instance
		{
			get
			{
				if (m_Instance != null)
					return m_Instance;

				return m_Instance = FindObjectOfType<Entities>(true);
			}
		}

		public static int       LocalPlayer       { get; private set; } = -1;
		public static EntityRef LocalPlayerEntity { get; private set; }

		// PRIVATE MEMBERS

		private static Entities m_Instance;
		private        Scene    m_Scene;

		private Dictionary<AssetGuid, List<Entity>>     m_FreeInstances      = new Dictionary<AssetGuid, List<Entity>>();
		private List<DelayedCacheReturn>                m_DelayedReturns     = new List<DelayedCacheReturn>();
		private Stack<DelayedCacheReturn>               m_FreeDelayedReturns = new Stack<DelayedCacheReturn>();

		// PUBLIC METHODS

		public bool SetLocalPlayer(int index)
		{
			var game = QuantumRunner.Default.Game;
			if (game.GetLocalPlayers().Contains(index) == false)
				return false;

			var frame = game.Frames.Predicted;

			foreach (var pair in frame.Unsafe.GetComponentBlockIterator<Player>())
			{
				if (pair.Component->PlayerRef == index)
				{
					LocalPlayer       = index;
					LocalPlayerEntity = pair.Entity;

					Signals.LocalPlayerChanged.Emit(LocalPlayer, LocalPlayerEntity);
					return true;
				}
			}

			return false;
		}
		
		// ISceneComponent INTERFACE

		void ISceneComponent.Initialize(Scene scene)
		{
			m_Scene = scene;
		}

		void ISceneComponent.Deinitialize()
		{
			LocalPlayer       = -1;
			LocalPlayerEntity = default;
		}

		void IUpdatableSceneComponent.OnUpdate(SceneContext context)
		{
			if (LocalPlayer == -1)
			{
				var localPlayers = context.QuantumGame.GetLocalPlayers();
				if (localPlayers.Length > 0 && SetLocalPlayer(localPlayers[0]) == true)
				{
					SetLocalPlayer(localPlayers[0]);
				}
			}

			var deltaTime = Time.deltaTime;

			for (int idx = m_DelayedReturns.Count; idx --> 0;)
			{
				var delayed    = m_DelayedReturns[idx];
				delayed.Delay -= deltaTime;

				if (delayed.Delay <= 0f)
				{
					m_DelayedReturns.RemoveAtWithSwap(idx);
					m_FreeDelayedReturns.Push(delayed);

					ReturnToCache(delayed.Entity);
				}
				else if (delayed.Delay <= delayed.Entity.DestroyDelay - delayed.Entity.DestroyMoveDelay)
				{
					var progress = 1f - delayed.Delay / (delayed.Entity.DestroyDelay - delayed.Entity.DestroyMoveDelay);
					delayed.Entity.transform.position = delayed.StartPosition + progress * delayed.Entity.DestroyMoveOffset;
				}
			}

			foreach (var entityPair in ActiveViews)
			{
				(entityPair.Value as Entity).Update_Internal(context);
			}
		}

		void ILateUpdatableSceneComponent.OnLateUpdate(SceneContext context)
		{
			foreach (var entityPair in ActiveViews)
			{
				(entityPair.Value as Entity).LateUpdate_Internal(context);
			}
		}

		// EntityViewUpdater INTERFACE

		protected override void ActivateMapEntityInstance(EntityView instance, Vector3? position = null, Quaternion? rotation = null)
		{
			base.ActivateMapEntityInstance(instance, position, rotation);

			var entity = instance as Entity;
			entity.Initialize(m_Scene);

			Signals.EntityCreated.Emit(entity);
		}

		protected override void DisableMapEntityInstance(EntityView instance)
		{
			var entity = instance as Entity;
			entity.Deinitialize();

			Signals.EntityDestroyed.Emit(entity);

			base.DisableMapEntityInstance(instance);
		}

		protected override EntityView CreateEntityViewInstance(EntityViewAsset asset, Vector3? position = null, Quaternion? rotation = null)
		{
			if (m_FreeInstances.TryGetValue(asset.AssetObject.Guid, out var cache) == false)
			{
				cache = new List<Entity>();
				m_FreeInstances[asset.AssetObject.Guid] = cache;
			}

			Entity instance;
			if (cache.Count > 0)
			{
				instance = cache.PopLast();
			}
			else
			{
				instance = Instantiate(asset.View) as Entity;
				instance.Initialize(m_Scene);
			}

			if (position.HasValue == true && rotation.HasValue == true)
			{
				instance.transform.SetPositionAndRotation(position.Value, rotation.Value);
			}

			instance.SetActive(true);
			instance.Activate();

			Signals.EntityCreated.Emit(instance);

			return instance;
		}

		protected override void DestroyEntityViewInstance(EntityView instance)
		{
			var entity = instance as Entity;
			if (entity.DestroyDelay > 0f)
			{
				var delayed = m_FreeDelayedReturns.Count > 0 ? m_FreeDelayedReturns.Pop() : new DelayedCacheReturn();

				delayed.Delay         = entity.DestroyDelay;
				delayed.Entity        = entity;
				delayed.StartPosition = entity.transform.position;

				m_DelayedReturns.Add(delayed);
			}
			else
			{
				ReturnToCache(entity);
			}

			Signals.EntityDestroyed.Emit(entity);
		}

		// PRIVATE METHODS

		private void ReturnToCache(Entity entity)
		{
			if (m_FreeInstances.TryGetValue(entity.AssetGuid, out var cache) == false)
			{
				cache = new List<Entity>();
				m_FreeInstances[entity.AssetGuid] = cache;
			}


			cache.Add(entity);
			entity.Deactivate();
			entity.SetActive(false);
		}

		// HELPERS

		private class DelayedCacheReturn
		{
			public Entity    Entity;
			public float     Delay;
			public Vector3   StartPosition;
		}
	}
}