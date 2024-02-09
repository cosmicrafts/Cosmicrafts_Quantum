namespace Quantum.Services
{
	using System.Collections.Generic;
	using UnityEngine;

	public sealed class Services : IServiceProvider
	{
		//========== PUBLIC MEMBERS ===================================================================================

		public bool        IsInitialized { get { return _isInitialized; } }

		public Log         Log           { get; private set; }
		public Network     Network       { get; private set; }
		public Messages    Messages      { get; private set; }
		public Party       Party         { get; private set; }
		public Lobby       Lobby         { get; private set; }
		public Matchmaking Matchmaking   { get; private set; }

		//========== PRIVATE MEMBERS ==================================================================================

		private bool           _isInitialized;
		private List<IService> _services  = new List<IService>();

		//========== PUBLIC METHODS ===================================================================================

		public void Initialize()
		{
			if (_isInitialized == true)
				return;

			Log         = CreatePlainService<Log>();
			Network     = CreateComponentService<Network>();
			Messages    = CreateComponentService<Messages>();
			Party       = CreateComponentService<Party>();
			Lobby       = CreateComponentService<Lobby>();
			Matchmaking = CreateComponentService<Matchmaking>();

			for (int i = 0, count = _services.Count; i < count; ++i)
			{
				IService service = _services[i];
				service.Initialize(this);
			}

			_isInitialized = true;
		}

		public void Deinitialize()
		{
			_isInitialized = false;

			for (int i = _services.Count - 1; i >= 0; --i)
			{
				IService  service          = _services[i];
				Component serviceComponent = service as Component;

				service.Deinitialize();

				if (serviceComponent != null && serviceComponent.gameObject != null)
				{
					Object.Destroy(serviceComponent.gameObject);
				}
			}

			_services.Clear();
		}

		public void Tick()
		{
			if (_isInitialized == false)
				return;

			for (int i = 0, count = _services.Count; i < count; ++i)
			{
				_services[i].Tick();
			}
		}

		//========== IServiceProvider INTERFACE =======================================================================

		T IServiceProvider.GetService<T>()
		{
			for (int i = 0, count = _services.Count; i < count; ++i)
			{
				if (_services[i] is T tService)
					return tService;
			}

			return default;
		}

		//========== PRIVATE METHODS ==================================================================================

		private T CreatePlainService<T>() where T : IService, new()
		{
			T service = new T();
			_services.Add(service);
			return service;
		}

		private T CreateComponentService<T>() where T : Component, IService
		{
			GameObject serviceGO = new GameObject(typeof(T).Name);
			T service = serviceGO.AddComponent<T>();
			Object.DontDestroyOnLoad(service.gameObject);
			_services.Add(service);
			return service;
		}
	}
}
