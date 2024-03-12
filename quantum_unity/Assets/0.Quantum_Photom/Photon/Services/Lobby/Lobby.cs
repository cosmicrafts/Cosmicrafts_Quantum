namespace Quantum.Services
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Photon.Chat;

	public sealed class Lobby : MonoBehaviour, IService
	{
		//========== CONSTANTS ========================================================================================

		private const string LOBBY_DEFAULT   = "DefaultLobby";
		private const string PROPERTY_LEADER = "LobbyLeader";

		//========== PUBLIC MEMBERS ===================================================================================

		public string       ID          { get { return _lobbyID;  } }
		public string       UserID      { get { return _messages.UserID; } }
		public string       LeaderID    { get { return _lobbyID.HasValue() == true && _messages.CanChatInChannel(_lobbyID) == true && _messages.TryGetChannel(_lobbyID, out ChatChannel channel) == true && channel.TryGetProperty(PROPERTY_LEADER, out object leaderID) == true ? (string)leaderID : null; } }
		public bool         IsConnected { get { return _lobbyID.HasValue() == true && _messages.CanChatInChannel(_lobbyID) == true; } }
		public bool         IsValid     { get { return _lobbyID.HasValue() == true; } }
		public LobbyPlayers Players     { get { return _players; } }

		public event Action Joined;
		public event Action Left;

		//========== PRIVATE MEMBERS ==================================================================================

		private Log          _log;
		private Messages     _messages;
		private string       _lobbyID;
		private Coroutine    _joinCoroutine;
		private LobbyPlayers _players = new LobbyPlayers();
		private float        _nextSynchronizeTime;
		private float        _synchronizationInterval = 1.0f;

		//========== PUBLIC METHODS ===================================================================================

		public void Join(string lobbyID = null)
		{
			if (lobbyID.HasValue() == false)
			{
				lobbyID = LOBBY_DEFAULT;
			}

			if (_lobbyID == lobbyID)
				return;

			Leave();

			_lobbyID = lobbyID;

			_joinCoroutine = StartCoroutine(JoinCoroutine());
		}

		public void Leave()
		{
			if (IsValid == false)
				return;

			string userID = UserID;
			if (userID.HasValue() == true)
			{
				_messages.SetProperties(_lobbyID, new Dictionary<object, object>() { { userID, null } });
			}

			_messages.Unsubscribe(_lobbyID);
			_messages.Flush();

			_log.Warning(ELogGroup.Lobby, "[Leave] Lobby {0} left", _lobbyID);

			Stop();

			_lobbyID = null;

			Left.SafeInvoke();
		}

		public void SetSynchronizationInterval(float interval)
		{
			_synchronizationInterval = Mathf.Max(1.0f, interval);
		}

		public LobbyPlayerData GetPlayerData(string userID)
		{
			LobbyPlayer player = _players.Get(userID);
			return player != null ? player.Data : null;
		}

		public LobbyPlayer GetLocalPlayer()
		{
			List<LobbyPlayer> players = _players.All;
			for (int i = 0, count = players.Count; i < count; ++i)
			{
				LobbyPlayer player = players[i];
				if (player.IsLocal == true)
					return player;
			}

			return null;
		}

		public bool IsLocal(string userID)
		{
			if (userID.HasValue() == false)
				return false;

			LobbyPlayer localPlayer = GetLocalPlayer();
			if (localPlayer != null && localPlayer.UserID == userID)
				return true;

			return userID == UserID;
		}

		public void SendMessage(ILobbyMessage message)
		{
			if (message is ChannelMessage == false)
			{
				throw new NotSupportedException("Only channel message can be sent in lobby! " + message.GetType().FullName);
			}

			_messages.SendMessage(message, _lobbyID);
		}

		public void SendMessage(ILobbyMessage message, string userID)
		{
			if (message is PrivateMessage == false)
			{
				throw new NotSupportedException("Only private message can be sent to player! " + message.GetType().FullName);
			}

			_messages.SendMessage(message, userID);
		}

		//========== IService INTERFACE ===============================================================================

		void IService.Initialize(IServiceProvider serviceProvider)
		{
			_log      = serviceProvider.GetService<Log>();
			_messages = serviceProvider.GetService<Messages>();

			if (_messages != null)
			{
				_messages.ChannelSubscribed   += OnChannelSubscribed;
				_messages.ChannelUnsubscribed += OnChannelUnsubscribed;
			}
		}

		void IService.Deinitialize()
		{
			Leave();
			Stop();

			Joined = null;
			Left   = null;

			_lobbyID = null;

			if (_messages != null)
			{
				_messages.ChannelSubscribed   -= OnChannelSubscribed;
				_messages.ChannelUnsubscribed -= OnChannelUnsubscribed;
			}

			_messages = null;
			_log      = null;
		}

		void IService.Tick()
		{
			if (IsValid == false)
				return;

			if (IsConnected == true)
			{
				if (Time.realtimeSinceStartup < _nextSynchronizeTime)
					return;

				_nextSynchronizeTime = Time.realtimeSinceStartup + _synchronizationInterval;

				if (_messages.TryGetChannel(_lobbyID, out ChatChannel channel) == true)
				{
					SynchronizePlayers(channel);
					CleanupProperties(channel);
				}
			}
			else
			{
				_players.Clear();

				if (_joinCoroutine == null && _messages.IsConnected == true)
				{
					_joinCoroutine = StartCoroutine(JoinCoroutine());
				}
			}
		}

		//========== PRIVATE METHODS ==================================================================================

		private void OnChannelSubscribed(string channel, bool result)
		{
			if (channel == _lobbyID && result == true && _messages.TryGetChannel(_lobbyID, out ChatChannel chatChannel) == true)
			{
				SynchronizePlayers(chatChannel);
			}
		}

		private void OnChannelUnsubscribed(string channel)
		{
			if (channel == _lobbyID)
			{
				_log.Warning(ELogGroup.Lobby, "Lobby {0} left", _lobbyID);

				Stop();

				Left.SafeInvoke();
			}
		}

		private void SynchronizePlayers(ChatChannel channel)
		{
			List<LobbyPlayer> players = _players.All;

			for (int i = players.Count - 1; i >= 0; --i)
			{
				LobbyPlayer player = players[i];
				if (channel.Subscribers.Contains(player.UserID) == false)
				{
					_log.Info(ELogGroup.Lobby, "[SynchronizePlayers] Player {0} disconnected from lobby!", player.UserID);

					player.Dispose();
					players.RemoveAt(i);
				}
			}

			if (channel.Subscribers.Contains(UserID) == false)
				return;

			LobbyPlayer localPlayer = GetLocalPlayer();
			if (localPlayer == null)
			{
				localPlayer = _players.Add(UserID, true, SendPlayerData);

				_log.Warning(ELogGroup.Lobby, "[SynchronizePlayers] Joined lobby {0}", channel.Name);

				Joined.SafeInvoke();
			}

			foreach (string subscriber in channel.Subscribers)
			{
				LobbyPlayer player = _players.Get(subscriber);
				if (player == null)
				{
					player = _players.Add(subscriber, false, null);

					_log.Info(ELogGroup.Lobby, "[SynchronizePlayers] Player {0} connected to lobby!", player.UserID);
				}

				if (channel.TryGetProperty(subscriber, out object dataObject) == true)
				{
					player.Data.SetData(dataObject);
				}
			}
		}

		private void CleanupProperties(ChatChannel channel)
		{
			if (channel.TryGetProperty(PROPERTY_LEADER, out object leaderID) == false || (string)leaderID != UserID)
				return;
			if (channel.Properties == null)
				return;

			Dictionary<object, object> deleteProperties = null;

			foreach (KeyValuePair<object, object> property in channel.Properties)
			{
				if (property.Key is string key)
				{
					if (key != PROPERTY_LEADER && channel.Subscribers.Contains(key) == false)
					{
						if (deleteProperties == null)
						{
							deleteProperties = new Dictionary<object, object>();
						}

						deleteProperties[key] = null;
					}
				}
			}

			if(deleteProperties != null)
			{
				_log.Info(ELogGroup.Lobby, "[CleanupProperties] Removing {0} properties from lobby!", deleteProperties.Count);

				_messages.SetProperties(_lobbyID, deleteProperties);
			}
		}

		private void SetLeader(string leaderID)
		{
			_messages.SetProperties(_lobbyID, new Dictionary<object, object>() { { PROPERTY_LEADER, leaderID } });
		}

		private void SendPlayerData(LobbyPlayer player)
		{
			if (IsValid == false)     { _log.Error(ELogGroup.Lobby, "[SendPlayerData] You are not in lobby!"); return; }
			if (IsConnected == false) { _log.Error(ELogGroup.Lobby, "[SendPlayerData] Not connected!");        return; }

			_log.Info(ELogGroup.Lobby, "[SendPlayerData]");

			_messages.SetProperties(_lobbyID, new Dictionary<object, object>() { { player.UserID, player.Data.GetData() } });
		}

		private void Stop()
		{
			if (this != null && _joinCoroutine != null)
			{
				StopCoroutine(_joinCoroutine);
			}

			_joinCoroutine = null;

			_players.Clear();
		}

		private IEnumerator JoinCoroutine()
		{
			float timeout = Time.realtimeSinceStartup + 5.0f;
			while (_messages.IsConnected == false && Time.realtimeSinceStartup < timeout)
			{
				yield return null;
			}

			if (_messages.IsConnected == false)
			{
				_log.Warning(ELogGroup.Lobby, "Failed to join lobby {0}, not connected to chat!", _lobbyID);

				_joinCoroutine = null;
				yield break;
			}

			_log.Warning(ELogGroup.Lobby, "Joining lobby {0} ...", _lobbyID);

			Dictionary<object, object> channelProperties = new Dictionary<object, object>();
			channelProperties[PROPERTY_LEADER] = UserID;

			ChannelCreationOptions createOptions = new ChannelCreationOptions();
			createOptions.PublishSubscribers = true;
			createOptions.MaxSubscribers     = 0;
			createOptions.Properties         = channelProperties;

			_messages.Subscribe(_lobbyID, 0, 0, createOptions);
			_messages.Flush();

			timeout = Time.realtimeSinceStartup + 5.0f;
			while (IsConnected == false && Time.realtimeSinceStartup < timeout)
			{
				yield return null;
			}

			if (IsConnected == true)
			{
				SetLeader(UserID);
			}
			else
			{
				_log.Warning(ELogGroup.Lobby, "Failed to join lobby {0}", _lobbyID);
			}

			_joinCoroutine = null;
		}
	}
}
