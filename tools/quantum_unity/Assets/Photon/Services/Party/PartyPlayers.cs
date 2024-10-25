namespace Quantum.Services
{
	using System;
	using System.Collections.Generic;

	public sealed partial class PartyPlayers
	{
		//========== PUBLIC MEMBERS ===================================================================================

		public List<PartyPlayer> All   { get { return _players;       } }
		public int               Count { get { return _players.Count; } }

		//========== PRIVATE MEMBERS ==================================================================================

		private List<PartyPlayer> _players = new List<PartyPlayer>();

		//========== PUBLIC METHODS ===================================================================================

		public bool Contains(string userID)
		{
			return Get(userID) != null;
		}

		public bool Contains(int slot)
		{
			return Get(slot) != null;
		}

		public PartyPlayer Get(string userID)
		{
			foreach (PartyPlayer player in _players)
			{
				if (player.UserID == userID)
				{
					return player;
				}
			}

			return null;
		}

		public PartyPlayer Get(int slot)
		{
			foreach (PartyPlayer player in _players)
			{
				if (player.Slot == slot)
				{
					return player;
				}
			}

			return null;
		}

		public PartyPlayer Add(string userID, bool isLocal, int slot, Action<PartyPlayer> sendPlayerData)
		{
			if (userID.HasValue() == false)
			{
				throw new ArgumentNullException();
			}

			PartyPlayer player = new PartyPlayer(userID, isLocal, sendPlayerData);
			player.Status = EPartyPlayerStatus.Connected;
			player.Slot   = slot;

			_players.Add(player);

			return player;
		}

		public bool Remove(string userID)
		{
			for (int i = 0; i < _players.Count; ++i)
			{
				PartyPlayer player = _players[i];
				if (player.UserID == userID)
				{
					player.Dispose();
					_players.RemoveAt(i);
					return true;
				}
			}

			return false;
		}

		public void Clear()
		{
			foreach (PartyPlayer player in _players)
			{
				player.Dispose();
			}

			_players.Clear();
		}

		public void CopyTo(List<PartyPlayer> players)
		{
			players.Clear();
			players.AddRange(_players);
		}

		public Dictionary<string, int> ToDictionary()
		{
			Dictionary<string, int> players = new Dictionary<string, int>();
			foreach (PartyPlayer player in _players)
			{
				players[player.UserID] = player.Slot;
			}
			return players;
		}

		public bool Compare(Dictionary<string, int> others)
		{
			if (others.Count != _players.Count)
				return false;

			foreach (PartyPlayer player in _players)
			{
				if (others.TryGetValue(player.UserID, out int slot) == false || slot != player.Slot)
					return false;
			}

			return true;
		}
	}
}
