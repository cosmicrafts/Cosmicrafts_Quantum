namespace TowerRush.Core
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine.SceneManagement;

	public class SceneContext
	{
		public Game                Game;
		public Scene               ActiveScene;
		public Quantum.QuantumGame QuantumGame;
		public Quantum.Frame       Frame;
	}

	public interface ISceneComponent
	{
		void Initialize(Scene scene);
		void Deinitialize();
	}

	public interface IUpdatableSceneComponent : ISceneComponent
	{
		void OnUpdate(SceneContext context);
	}

	public interface ILateUpdatableSceneComponent : ISceneComponent
	{
		void OnLateUpdate(SceneContext context);
	}

	public abstract class Scene : MonoBehaviour
	{
		// PUBLIC MEMBERS

		public bool Active   { get { return m_State == EState.Active;   } }
		public bool Finished { get { return m_State == EState.Finished; } }

		public UIFrontend Frontend { get; private set; }

		// PROTECTED MEMBERS

		protected List<ISceneComponent>              m_Components              = new List<ISceneComponent>();
		protected List<IUpdatableSceneComponent>     m_UpdatableComponents     = new List<IUpdatableSceneComponent>();
		protected List<ILateUpdatableSceneComponent> m_LateUpdatableComponents = new List<ILateUpdatableSceneComponent>();

		protected EState                             m_State;

		// ABSTRACT INTERFACE
		protected virtual  string UISceneName { get { return string.Empty; } }

		protected virtual  void OnPreInitialize() { }
		protected abstract void OnInitialize();
		protected abstract void OnDeinitialize();
		protected virtual  void OnRegisterComponents(List<ISceneComponent> components) { }
		protected virtual  void OnUpdate() { }

		protected virtual  void OnActivate()
		{
			m_State = EState.Active;
		}

		protected virtual  void OnDeactivate()
		{
			m_State = EState.Finished;
		}

		protected virtual  bool CanUpdateComponents(SceneContext context) { return true; }

		// PUBLIC METHODS

		public IEnumerator Initialize()
		{
			RegisterComponents(gameObject);

			if (Frontend == null && UISceneName.HasValue() == true)
			{
				yield return LoadUIScene_Coroutine();
			}

			OnPreInitialize();

			InitializeComponents();

			OnInitialize();

			m_State = EState.Initialized;
		}

		public void Deinitialize()
		{
			DeinitializeComponents();

			OnDeinitialize();
		}

		public void Activate()
		{
			if (m_State != EState.Initialized)
				return;

			OnActivate();
		}

		public void Deactivate()
		{
			if (m_State < EState.Active)
				return;

			OnDeactivate();
		}

		public void FinishScene()
		{
			if (m_State != EState.Active)
				return;

			m_State = EState.Finished;
		}

		public void OnUpdate_Internal(SceneContext context)
		{
			if (m_State != EState.Active)
				return;

			if (CanUpdateComponents(context) == true)
			{
				foreach (var component in m_UpdatableComponents)
				{
					component.OnUpdate(context);
				}
			}

			OnUpdate();
		}

		public void OnLateUpdate_Internal(SceneContext context)
		{
			if (CanUpdateComponents(context) == true)
			{
				foreach (var component in m_LateUpdatableComponents)
				{
					component.OnLateUpdate(context);
				}
			}
		}

		public T GetSceneComponent<T>() where T : ISceneComponent
		{
			for (int idx = 0, count = m_Components.Count; idx < count; idx++)
			{
				var component = m_Components[idx];
				if (component is T)
					return (T)component;
			}

			return default(T);
		}

		// PROTECTED METHODS

		protected T RegisterComponent<T>(List<ISceneComponent> components) where T : ISceneComponent
		{
			var component = (T)components.Find(obj => obj is T);
			if (component == null)
			{
				Debug.LogWarning($"Requied component of type {typeof(T).Name} not found in scene {GetType().Name}");
				return default;
			}

			components.Remove(component);

			RegisterComponent(component);

			return component;
		}

		// PRIVATE METHODS

		private IEnumerator LoadUIScene_Coroutine()
		{
			var asyncOp = SceneManager.LoadSceneAsync(UISceneName, LoadSceneMode.Additive);
			while (asyncOp.isDone == false)
				yield return null;

			var unityScene = SceneManager.GetSceneByName(UISceneName);
			var roots      = ListPool.Get<GameObject>(2);

			unityScene.GetRootGameObjects(roots);

			for (int idx = 0, count = roots.Count; idx < count; idx++)
			{
				var root = roots[idx];
				root.transform.parent = transform;

				RegisterComponents(root);
			}

			ListPool.Return(roots);

			asyncOp = SceneManager.UnloadSceneAsync(UISceneName);
			while (asyncOp.isDone == false)
				yield return null;
		}

		private void RegisterComponents(GameObject root)
		{
			var components = ListPool.Get<ISceneComponent>(8);

			root.GetComponentsInChildren(components);

			if (Frontend == null)
			{
				Frontend = RegisterComponent<UIFrontend>(components);
			}

			OnRegisterComponents(components);

			foreach (var component in components)
			{
				RegisterComponent(component);
			}

			ListPool.Return(components);
		}

		private void RegisterComponent(ISceneComponent component)
		{
			m_Components.Add(component);
			m_UpdatableComponents.AddIfNotNull(component as IUpdatableSceneComponent);
			m_LateUpdatableComponents.AddIfNotNull(component as ILateUpdatableSceneComponent);
		}

		private void InitializeComponents()
		{
			foreach (var component in m_Components)
			{
				component.Initialize(this);
			}
		}

		private void DeinitializeComponents()
		{
			foreach (var component in m_Components)
			{
				component.Deinitialize();
			}

			m_Components.Clear();
			m_UpdatableComponents.Clear();
		}

		// HELPERS

		protected enum EState
		{
			None,
			Initialized,
			Active,
			Finished,
		}
	}
}
