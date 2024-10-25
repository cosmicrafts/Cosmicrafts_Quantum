namespace Quantum.Services
{
	using System;
	using System.Collections.Generic;

	public sealed partial class LobbyPlayers
	{
		//========== PUBLIC MEMBERS ===================================================================================

		public List<LobbyPlayer> All   { get { return _players;       } }
		public int               Count { get { return _players.Count; } }

		//========== PRIVATE MEMBERS ==================================================================================

		private List<LobbyPlayer> _players = new List<LobbyPlayer>();

		//========== PUBLIC METHODS ===================================================================================

		public bool Contains(string userID)
		{
			return Get(userID) != null;
		}

		public LobbyPlayer Get(string userID)
		{
			foreach (LobbyPlayer player in _players)
			{
				if (player.UserID == userID)
				{
					return player;
				}
			}

			return null;
		}

		public LobbyPlayer Add(string userID, bool isLocal, Action<LobbyPlayer> sendPlayerData)
		{
			if (userID.HasValue() == false)
			{
				throw new ArgumentNullException();
			}

			LobbyPlayer player = new LobbyPlayer(userID, isLocal, sendPlayerData);

			_players.Add(player);

			return player;
		}

		public void Remove(string userID)
		{
			for (int i = 0; i < _players.Count; ++i)
			{
				LobbyPlayer player = _players[i];
				if (player.UserID == userID)
				{
					player.Dispose();
					_players.RemoveAt(i);
					return;
				}
			}
		}

		public void Clear()
		{
			foreach (LobbyPlayer player in _players)
			{
				player.Dispose();
			}

			_players.Clear();
		}
	}
}
