namespace Quantum.Services
{
	using System;

	public sealed partial class LobbyPlayerData
	{
		//========== PRIVATE MEMBERS ==================================================================================

		private Action _synchronize;

		//========== CONSTRUCTORS =====================================================================================

		public LobbyPlayerData(Action synchronize)
		{
			_synchronize = synchronize;

			Initialize();
		}

		private LobbyPlayerData()
		{
		}

		//========== PUBLIC METHODS ===================================================================================

		public void Dispose()
		{
			Deinitialize();

			_synchronize = null;
		}

		public void Synchronize()
		{
			_synchronize.SafeInvoke();
		}

		public object GetData()
		{
			object data = null;
			Serialize(ref data);
			return data;
		}

		public void SetData(object data)
		{
			Deserialize(ref data);
		}

		//========== PARTIAL METHODS ==================================================================================

		partial void Initialize();
		partial void Deinitialize();
		partial void Serialize(ref object data);
		partial void Deserialize(ref object data);
	}
}
