namespace Quantum.Services
{
	using System;

	public enum EPartyPlayerStatus
	{
		None        = 0,
		Connected   = 1,
		Disonnected = 2
	}

	public sealed partial class PartyPlayer
	{
		//========== PUBLIC MEMBERS ===================================================================================

		public readonly string          UserID;
		public readonly bool            IsLocal;
		public readonly PartyPlayerData Data;

		public EPartyPlayerStatus       Status;
		public float                    Timeout;
		public int                      Slot;

		//========== PRIVATE MEMBERS ==================================================================================

		private Action<PartyPlayer> _sendPlayerData;

		//========== CONSTRUCTORS =====================================================================================

		public PartyPlayer(string userID, bool isLocal, Action<PartyPlayer> sendPlayerData)
		{
			_sendPlayerData = sendPlayerData;

			UserID  = userID;
			IsLocal = isLocal;
			Data    = new PartyPlayerData(SynchronizePlayerData);
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
