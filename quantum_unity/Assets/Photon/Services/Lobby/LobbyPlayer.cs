namespace Quantum.Services
{
	using System;

	public sealed partial class LobbyPlayer
	{
		//========== PUBLIC MEMBERS ===================================================================================

		public readonly string          UserID;
		public readonly bool            IsLocal;
		public readonly LobbyPlayerData Data;

		//========== PRIVATE MEMBERS ==================================================================================

		private Action<LobbyPlayer> _sendPlayerData;

		//========== CONSTRUCTORS =====================================================================================

		public LobbyPlayer(string userID, bool isLocal, Action<LobbyPlayer> sendPlayerData)
		{
			_sendPlayerData = sendPlayerData;

			UserID  = userID;
			IsLocal = isLocal;
			Data    = new LobbyPlayerData(SynchronizePlayerData);
		}

		private LobbyPlayer()
		{
		}

		//========== PUBLIC METHODS ===================================================================================

		public void Dispose()
		{
			Data.Dispose();

			_sendPlayerData = null;
		}

		//========== PRIVATE METHODS ==================================================================================

		private void SynchronizePlayerData()
		{
			_sendPlayerData.SafeInvoke(this);
		}
	}
}
