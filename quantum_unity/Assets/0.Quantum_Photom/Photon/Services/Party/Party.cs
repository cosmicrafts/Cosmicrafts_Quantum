namespace Quantum.Services
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Photon.Chat;

	public sealed class Party : MonoBehaviour, IService
	{
		//========== CONSTANTS ========================================================================================

		private const string PROPERTY_LEADER  = "PartyLeader";
		private const string PROPERTY_PLAYERS = "PartyPlayers";

		//========== PUBLIC MEMBERS ===================================================================================

		public bool         AutoAcceptJoinRequest       = true;
		public bool         AllowRemotePlayerDataChange = false;
		public float        PlayerInviteTimeout         = 15.0f;
		public float        PlayerKickTimeout           = 30.0f;

		public string       ID          { get { return _partyID; } }
		public string       UserID      { get { return _messages.UserID; } }
		public string       LeaderID    { get { return _messages.IsConnected == true && _partyID.HasValue() == true && _messages.CanChatInChannel(_partyID) == true && _messages.TryGetChannel(_partyID, out ChatChannel channel) == true && channel.TryGetProperty(PROPERTY_LEADER, out object leaderID) == true ? (string)leaderID : null; } }
		public bool         IsConnected { get { return _messages.IsConnected == true && _partyID.HasValue() == true && _messages.CanChatInChannel(_partyID) == true; } }
		public bool         IsLeader    { get { return _messages.IsConnected == true && _messages.UserID == LeaderID; } }
		public bool         IsValid     { get { return _partyID.HasValue() == true; } }
		public int          MaxPlayers  { get { return _maxPlayers; } }
		public PartyPlayers Players     { get { return _players; } }
		public PartyInvites Invites     { get { return _invites; } }

		public event Action<PartyPlayer> Joined;
		public event Action<PartyPlayer> Left;
		public event Action<PartyPlayer> PlayerJoined;
		public event Action<PartyPlayer> PlayerLeft;
		public event Action<PartyPlayer> PlayerReconnected;
		public event Action<PartyPlayer> PlayerDisconnected;
		public event Action<PartyPlayer> PlayerSlotChanged;
		public event Action<PartyPlayer> PlayerDataChanged;
		public event Action<PartyPlayer> LeaderChanged;

		//========== PRIVATE MEMBERS ==================================================================================

		private Log          _log;
		private Messages     _messages;
		private string       _partyID;
		private int          _maxPlayers;
		private Coroutine    _joinCoroutine;
		private float        _nextLeaderRequestTime;
		private float        _nextPlayersUpdateTime;
		private float        _nextCleanupPropertiesTime;

		private PartyPlayers _players = new PartyPlayers();
		private PartyInvites _invites = new PartyInvites();

		//========== PUBLIC METHODS ===================================================================================

		public void Create(string partyID = null, int maxPlayers = 0)
		{
			if (partyID.HasValue() == false)
			{
				partyID = _messages.UserID;
			}

			JoinOrCreateParty(partyID, maxPlayers);
		}

		public void Join(string partyID, int maxPlayers = 0)
		{
			JoinOrCreateParty(partyID, maxPlayers);
		}

		public void Leave()
		{
			LeaveParty();
		}

		public void Invite(string userID)
		{
			if (userID.HasValue() == false)        { _log.Error(ELogGroup.Party, "[Invite] Invalid user ID!");                      return; }
			if (userID == UserID)                  { _log.Error(ELogGroup.Party, "[Invite] Cannot invite self!");                   return; }
			if (IsValid == false)                  { _log.Error(ELogGroup.Party, "[Invite] You have no party!");                    return; }
			if (IsConnected == false)              { _log.Error(ELogGroup.Party, "[Invite] Not connected!");                        return; }
			if (_players.Contains(userID) == true) { _log.Error(ELogGroup.Party, "[Invite] {0} is already in your party!", userID); return; }
			if (_invites.Contains(userID) == true) { _log.Error(ELogGroup.Party, "[Invite] {0} is already invited!", userID);       return; }

			PartyInvite invite = _invites.Get(userID);
			if (invite != null)
			{
				invite.Timeout = Time.realtimeSinceStartup + PlayerInviteTimeout;
				_log.Info(ELogGroup.Party, "[Invite] Updated Invite for player {0}, slot {1}", userID, invite.Slot);
			}
			else
			{
				invite = _invites.Add(userID, FindEmptySlot(), PlayerInviteTimeout);
				_log.Info(ELogGroup.Party, "[Invite] Added Invite for player {0}, slot {1}", userID, invite.Slot);
			}

			SendMessage(new PartyMessages.Invite(_partyID, PlayerInviteTimeout, _maxPlayers), userID);
		}

		public void Kick(string userID, string reason)
		{
			if (userID.HasValue() == false) { _log.Error(ELogGroup.Party, "[Kick] Invalid user ID!");   return; }
			if (userID == UserID)           { _log.Error(ELogGroup.Party, "[Kick] Cannot kick self!");  return; }
			if (IsValid == false)           { _log.Error(ELogGroup.Party, "[Kick] You have no party!"); return; }
			if (IsConnected == false)       { _log.Error(ELogGroup.Party, "[Kick] Not connected!");     return; }

			_log.Info(ELogGroup.Party, "[Kick] Kicked player {0}, reason {1}", userID, reason ?? "NULL");

			SendMessage(new PartyMessages.Kick(userID, reason));

			if (_messages.TryGetChannel(_partyID, out ChatChannel chatChannel) == true && chatChannel.Subscribers.Contains(userID) == false)
			{
				Players.Remove(userID);
			}
		}

		public bool SetLeader(string userID)
		{
			if (userID.HasValue() == false) { _log.Error(ELogGroup.Party, "[SetLeader] Invalid user ID!");     return false; }
			if (userID == UserID)           { _log.Error(ELogGroup.Party, "[SetLeader] Cannot promote self!"); return false; }
			if (IsValid == false)           { _log.Error(ELogGroup.Party, "[SetLeader] You have no party!");   return false; }
			if (IsConnected == false)       { _log.Error(ELogGroup.Party, "[SetLeader] Not connected!");       return false; }
			if (IsLeader == false)          { _log.Error(ELogGroup.Party, "[SetLeader] Not a leader!");        return false; }

			SwitchLeader(userID, UserID);

			return true;
		}

		public bool SetPlayerSlot(string userID, int slot)
		{
			if (userID.HasValue() == false)         { _log.Error(ELogGroup.Party, "[SetPlayerSlot] Invalid user ID!");                         return false; }
			if (IsValid == false)                   { _log.Error(ELogGroup.Party, "[SetPlayerSlot] You have no party!");                       return false; }
			if (IsConnected == false)               { _log.Error(ELogGroup.Party, "[SetPlayerSlot] Not connected!");                           return false; }
			if (IsLeader == false)                  { _log.Error(ELogGroup.Party, "[SetPlayerSlot] Not a leader!");                            return false; }
			if (_players.Contains(userID) == false) { _log.Error(ELogGroup.Party, "[SetPlayerSlot] Player {0} is not in your party!", userID); return false; }
			if (_players.Contains(slot) == true)    { _log.Error(ELogGroup.Party, "[SetPlayerSlot] Slot {0} is already taken!", slot);         return false; }
			if (_invites.Contains(slot) == true)    { _log.Error(ELogGroup.Party, "[SetPlayerSlot] Slot {0} is reserved!", slot);              return false; }

			PartyPlayer player = _players.Get(userID);
			player.Slot = slot;

			_log.Info(ELogGroup.Party, "[SetPlayerSlot] Player {0} changed slot to {1}", player.UserID, player.Slot);
			PlayerSlotChanged.SafeInvoke(player);

			if (_messages.TryGetChannel(_partyID, out ChatChannel channel) == true)
			{
				SynchronizePlayers(channel, true);
			}

			return true;
		}

		public bool ExchangePlayerSlots(string userID1, string userID2)
		{
			if (userID1.HasValue() == false)         { _log.Error(ELogGroup.Party, "[ExchangePlayerSlots] Invalid user ID 1!");                        return false; }
			if (userID2.HasValue() == false)         { _log.Error(ELogGroup.Party, "[ExchangePlayerSlots] Invalid user ID 2!");                        return false; }
			if (userID1 == userID2)                  { _log.Error(ELogGroup.Party, "[ExchangePlayerSlots] Same user IDs!");                            return false; }
			if (IsValid == false)                    { _log.Error(ELogGroup.Party, "[ExchangePlayerSlots] You have no party!");                        return false; }
			if (IsConnected == false)                { _log.Error(ELogGroup.Party, "[ExchangePlayerSlots] Not connected!");                            return false; }
			if (IsLeader == false)                   { _log.Error(ELogGroup.Party, "[ExchangePlayerSlots] Not a leader!");                             return false; }
			if (_players.Contains(userID1) == false) { _log.Error(ELogGroup.Party, "[ExchangePlayerSlots] Player {0} is not in your party!", userID1); return false; }
			if (_players.Contains(userID2) == false) { _log.Error(ELogGroup.Party, "[ExchangePlayerSlots] Player {0} is not in your party!", userID2); return false; }

			PartyPlayer player1 = _players.Get(userID1);
			PartyPlayer player2 = _players.Get(userID2);

			int player1Slot = player1.Slot;

			player1.Slot = player2.Slot;
			player2.Slot = player1Slot;

			_log.Info(ELogGroup.Party, "[ExchangePlayerSlots] Player {0} changed slot to {1}", player1.UserID, player1.Slot);
			PlayerSlotChanged.SafeInvoke(player1);
			_log.Info(ELogGroup.Party, "[ExchangePlayerSlots] Player {0} changed slot to {1}", player2.UserID, player2.Slot);
			PlayerSlotChanged.SafeInvoke(player2);

			if (_messages.TryGetChannel(_partyID, out ChatChannel channel) == true)
			{
				SynchronizePlayers(channel, true);
			}

			return true;
		}

		public PartyPlayerData GetPlayerData(string userID)
		{
			PartyPlayer player = _players.Get(userID);
			return player != null ? player.Data : null;
		}

		public PartyPlayer GetLocalPlayer()
		{
			List<PartyPlayer> players = _players.All;
			for (int i = 0, count = players.Count; i < count; ++i)
			{
				PartyPlayer player = players[i];
				if (player.IsLocal == true)
					return player;
			}

			return null;
		}

		public bool IsLocal(string userID)
		{
			if (userID.HasValue() == false)
				return false;

			PartyPlayer localPlayer = GetLocalPlayer();
			if (localPlayer != null && localPlayer.UserID == userID)
				return true;

			return userID == UserID;
		}

		public void SendMessage(IPartyMessage message)
		{
			if (message is ChannelMessage == false)
			{
				throw new NotSupportedException("Only channel message can be sent in party! " + message.GetType().FullName);
			}

			_messages.SendMessage(message, _partyID);
		}

		public void SendMessage(IPartyMessage message, string userID)
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
				_messages.Connected                += OnMessagesConnected;
				_messages.Disconnected             += OnMessagesDisconnected;
				_messages.MessageReceived          += OnMessageReceived;
				_messages.ChannelSubscribed        += OnChannelSubscribed;
				_messages.ChannelPropertiesChanged += OnChannelPropertiesChanged;
			}
		}

		void IService.Deinitialize()
		{
			Stop(true);

			if (_messages != null)
			{
				_messages.Connected                -= OnMessagesConnected;
				_messages.Disconnected             -= OnMessagesDisconnected;
				_messages.MessageReceived          -= OnMessageReceived;
				_messages.ChannelSubscribed        -= OnChannelSubscribed;
				_messages.ChannelPropertiesChanged -= OnChannelPropertiesChanged;
			}

			_messages = null;
			_log      = null;

			Joined             = null;
			Left               = null;
			PlayerJoined       = null;
			PlayerLeft         = null;
			PlayerReconnected  = null;
			PlayerDisconnected = null;
			PlayerSlotChanged  = null;
			PlayerDataChanged  = null;
			LeaderChanged      = null;
		}

		void IService.Tick()
		{
			if (IsValid == false)
				return;

			if (IsConnected == true)
			{
				if (_messages.TryGetChannel(_partyID, out ChatChannel channel) == true)
				{
					SynchronizeProperties(channel);
					SynchronizePlayers(channel);
					SynchronizeStatus(channel);
					CleanupProperties(channel);
				}
			}
			else if (_joinCoroutine == null && _messages.IsConnected == true)
			{
				_joinCoroutine = StartCoroutine(JoinOrCreateCoroutine());
			}
		}

		//========== PRIVATE METHODS ==================================================================================

		private void OnMessagesConnected()
		{
			if (_partyID.HasValue() == true && _joinCoroutine == null)
			{
				_joinCoroutine = StartCoroutine(JoinOrCreateCoroutine());
			}
		}

		private void OnMessagesDisconnected()
		{
			Stop(false);
		}

		private void OnMessageReceived(IMessage message)
		{
			if (message is PartyMessages.Kick kick)
			{
				if (message.Channel != _partyID)
					return;

				if (kick.UserID == UserID)
				{
					_log.Warning(ELogGroup.Party, "Kicked from party {0}, reason {1}", _partyID, kick.Reason);
					LeaveParty();
				}
				else
				{
					if (_messages.TryGetChannel(_partyID, out ChatChannel chatChannel) == true && chatChannel.Subscribers.Contains(kick.UserID) == false)
					{
						Players.Remove(kick.UserID);
					}
				}
			}
			else if (message is PartyMessages.Leave leave)
			{
				if (message.Channel != _partyID)
					return;

				PartyPlayer player = _players.Get(leave.Sender);
				if (player != null)
				{
					player.Status  = EPartyPlayerStatus.Disonnected;
					player.Timeout = PlayerKickTimeout;
				}
			}
			else if (message is PartyMessages.Join join)
			{
				if (_partyID != join.PartyID && AutoAcceptJoinRequest == true)
				{
					Join(join.PartyID, 0);
				}
			}
		}

		private void OnChannelSubscribed(string channel, bool result)
		{
			if (channel == _partyID && result == true && _messages.TryGetChannel(_partyID, out ChatChannel chatChannel) == true)
			{
				SynchronizeProperties(chatChannel);
				SynchronizePlayers(chatChannel);
			}
		}

		private void OnChannelPropertiesChanged(string channel, Dictionary<object, object> properties)
		{
			if (channel == _partyID && _messages.TryGetChannel(channel, out ChatChannel chatChannel) == true)
			{
				if (properties.ContainsKey(PROPERTY_PLAYERS) == true || properties.ContainsKey(PROPERTY_LEADER) == true)
				{
					SynchronizePlayers(chatChannel);
				}

				foreach (PartyPlayer player in _players.All)
				{
					if (properties.TryGetValue(player.UserID, out object dataObject) == true)
					{
						player.Data.SetData(dataObject);

						_log.Info(ELogGroup.Party, "Player data changed ({0})", player.UserID);
						PlayerDataChanged.SafeInvoke(player);
					}
				}

				if (properties.TryGetValue(PROPERTY_LEADER, out object leader) == true)
				{
					string leaderUserID = leader as string;
					PartyPlayer player = _players.Get(leaderUserID);
					if (player != null)
					{
						_log.Warning(ELogGroup.Party, "Player {0} is now leader!", player.UserID);
						LeaderChanged.SafeInvoke(player);
					}
				}
			}
		}

		private void JoinOrCreateParty(string partyID, int maxPlayers)
		{
			if (partyID.HasValue() == false)                   { _log.Error(ELogGroup.Party, "[JoinOrCreateParty] Missing party ID");         return; }
			if (maxPlayers > ChatClient.DefaultMaxSubscribers) { _log.Error(ELogGroup.Party, "[JoinOrCreateParty] Max Players out of range"); return; }

			if (_partyID == partyID && _maxPlayers == maxPlayers)
				return;

			LeaveParty();

			_partyID    = partyID;
			_maxPlayers = maxPlayers;

			_joinCoroutine = StartCoroutine(JoinOrCreateCoroutine());
		}

		private void LeaveParty()
		{
			if (IsValid == false)
				return;

			if (IsLeader == true)
			{
				foreach (PartyPlayer player in _players.All)
				{
					if (player.UserID != UserID)
					{
						SwitchLeader(player.UserID, UserID);
						break;
					}
				}
			}

			SendMessage(new PartyMessages.Leave());

			_messages.Unsubscribe(_partyID);
			_messages.Flush();

			Stop(true);
		}

		private void SynchronizeProperties(ChatChannel channel)
		{
			_maxPlayers = channel.MaxSubscribers;

			List<PartyInvite> invites = _invites.All;
			for (int i = invites.Count - 1; i >= 0; --i)
			{
				if (Time.realtimeSinceStartup >= invites[i].Timeout)
				{
					_log.Info(ELogGroup.Party, "[SynchronizeProperties] Player {0} invitation expired, removing...", invites[i].UserID);
					invites.RemoveAt(i);
				}
			}

			if (channel.TryGetProperty(PROPERTY_LEADER, out object leader) == true)
			{
				string leaderUserID = leader as string;
				if (leaderUserID.HasValue() == false || channel.Subscribers.Contains(leaderUserID) == false)
				{
					SwitchLeader(UserID, leaderUserID);
				}
			}
			else
			{
				SwitchLeader(UserID, null);
			}
		}

		private void SynchronizePlayers(ChatChannel channel, bool forceUpdateProperties = false)
		{
			if (IsLeader == true)
			{
				bool              localPlayerJoined = false;
				List<PartyPlayer> playersLeft       = ListPool<PartyPlayer>.Get(8);
				List<PartyPlayer> playersJoined     = ListPool<PartyPlayer>.Get(8);

				List<PartyPlayer> players = _players.All;
				for (int i = players.Count - 1; i >= 0; --i)
				{
					PartyPlayer player = players[i];
					if (player.IsLocal == false && channel.Subscribers.Contains(player.UserID) == false && player.Timeout >= PlayerKickTimeout)
					{
						player.Dispose();
						players.RemoveAt(i);

						playersLeft.Add(player);
					}
				}

				PartyPlayer localPlayer = _players.Get(UserID);
				if (localPlayer == null && channel.Subscribers.Contains(UserID) == true)
				{
					localPlayer = _players.Add(UserID, true, FindEmptySlot(), SendPlayerData);

					if (channel.TryGetProperty(UserID, out object dataObject) == true && dataObject != null)
					{
						localPlayer.Data.SetData(dataObject);
					}

					localPlayerJoined = true;
				}

				if (localPlayer != null)
				{
					foreach (string userID in channel.Subscribers)
					{
						if (_players.Contains(userID) == false)
						{
							int slot = FindEmptySlot();

							PartyInvite invite = _invites.Get(userID);
							if (invite != null)
							{
								slot = invite.Slot;
							}

							PartyPlayer player = _players.Add(userID, false, slot, SendPlayerData);

							if (channel.TryGetProperty(userID, out object dataObject) == true && dataObject != null)
							{
								player.Data.SetData(dataObject);
							}

							if (localPlayerJoined == false)
							{
								playersJoined.Add(player);
							}
						}

						if (_invites.Remove(userID) == true)
						{
							_log.Info(ELogGroup.Party, "[SynchronizePlayers] Removed invite for incoming player {0}", userID);
						}
					}
				}

				if (localPlayerJoined == true)
				{
					_log.Warning(ELogGroup.Party, "[SynchronizePlayers] Local Player {0} joined party {1}, total players {2}", localPlayer.UserID, channel.Name, _players.All.Count);
					Joined.SafeInvoke(localPlayer);
				}

				foreach (PartyPlayer player in playersLeft)
				{
					_log.Warning(ELogGroup.Party, "[SynchronizePlayers] Player {0} left party {1} and removed from friends", player.UserID, channel.Name);
					PlayerLeft.SafeInvoke(player);
				}

				foreach (PartyPlayer player in playersJoined)
				{
					_log.Warning(ELogGroup.Party, "[SynchronizePlayers] Player {0} joined party {1}, slot {2} and added to friends", player.UserID, channel.Name, player.Slot);
					PlayerJoined.SafeInvoke(player);
				}

				ListPool<PartyPlayer>.Return(playersLeft);
				ListPool<PartyPlayer>.Return(playersJoined);

				if (channel.TryGetProperty(PROPERTY_PLAYERS, out object playersProperty) == false || _players.Compare((Dictionary<string, int>)playersProperty) == false)
				{
					if (Time.realtimeSinceStartup >= _nextPlayersUpdateTime || forceUpdateProperties == true)
					{
						_nextPlayersUpdateTime = Time.realtimeSinceStartup + 0.5f;
						_log.Info(ELogGroup.Party, "[SynchronizePlayers] Updating {0} party players => channel properties", _players.All.Count);
						_messages.SetProperties(_partyID, new Dictionary<object, object>() { { PROPERTY_PLAYERS, _players.ToDictionary() } });
					}
				}
			}
			else
			{
				if (channel.TryGetProperty(PROPERTY_PLAYERS, out object property) == true)
				{
					Dictionary<string, int> propertyPlayers = (Dictionary<string, int>)property;
					if (_players.Compare(propertyPlayers) == false)
					{
						bool              localPlayerJoined  = false;
						List<PartyPlayer> playersLeft        = ListPool<PartyPlayer>.Get(8);
						List<PartyPlayer> playersJoined      = ListPool<PartyPlayer>.Get(8);
						List<PartyPlayer> playerSlotsChanged = ListPool<PartyPlayer>.Get(8);

						List<PartyPlayer> players = _players.All;
						for (int i = players.Count - 1; i >= 0; --i)
						{
							PartyPlayer player = players[i];
							if (player.IsLocal == false && propertyPlayers.ContainsKey(player.UserID) == false)
							{
								player.Dispose();
								players.RemoveAt(i);

								playersLeft.Add(player);
							}
						}

						PartyPlayer localPlayer = GetLocalPlayer();

						if (propertyPlayers.TryGetValue(UserID, out int localPlayerSlot) == true)
						{
							if (localPlayer == null)
							{
								localPlayer = _players.Add(UserID, true, localPlayerSlot, SendPlayerData);

								if (channel.TryGetProperty(UserID, out object dataObject) == true && dataObject != null)
								{
									localPlayer.Data.SetData(dataObject);
								}

								localPlayerJoined = true;
							}
							else
							{
								if (localPlayer.Slot != localPlayerSlot)
								{
									localPlayer.Slot = localPlayerSlot;
									playerSlotsChanged.Add(localPlayer);
								}
							}
						}

						if (localPlayer != null)
						{
							foreach (KeyValuePair<string, int> propertyPlayer in propertyPlayers)
							{
								string userID = propertyPlayer.Key;
								int    slot   = propertyPlayer.Value;

								PartyPlayer player = _players.Get(userID);

								if (player == null)
								{
									player = _players.Add(userID, false, slot, SendPlayerData);

									if (channel.TryGetProperty(userID, out object dataObject) == true && dataObject != null)
									{
										player.Data.SetData(dataObject);
									}

									if (localPlayerJoined == false)
									{
										playersJoined.Add(player);
									}
								}
								else
								{
									if (player.Slot != slot)
									{
										player.Slot = slot;
										playerSlotsChanged.Add(player);
									}
								}
							}

							_log.Info(ELogGroup.Party, "[SynchronizePlayers] Updating channel properties => {0} party players", _players.All.Count);
						}

						if (localPlayerJoined == true)
						{
							_log.Warning(ELogGroup.Party, "[SynchronizePlayers] Local Player {0} joined party {1}, slot {2}, total players {3}", localPlayer.UserID, channel.Name, localPlayer.Slot, _players.All.Count);
							Joined.SafeInvoke(localPlayer);
						}

						foreach (PartyPlayer player in playersLeft)
						{
							_log.Warning(ELogGroup.Party, "[SynchronizePlayers] Player {0} left party {1} and removed from friends", player.UserID, channel.Name);
							PlayerLeft.SafeInvoke(player);
						}

						foreach (PartyPlayer player in playersJoined)
						{
							_log.Warning(ELogGroup.Party, "[SynchronizePlayers] Player {0} joined party {1}, slot {2} and added to friends", player.UserID, channel.Name, player.Slot);
							PlayerJoined.SafeInvoke(player);
						}

						foreach (PartyPlayer player in playerSlotsChanged)
						{
							_log.Info(ELogGroup.Party, "[SynchronizePlayers] Player {0} changed slot to {1}", player.UserID, player.Slot);
							PlayerSlotChanged.SafeInvoke(player);
						}

						ListPool<PartyPlayer>.Return(playersLeft);
						ListPool<PartyPlayer>.Return(playersJoined);
						ListPool<PartyPlayer>.Return(playerSlotsChanged);
					}
				}
			}
		}

		private void SynchronizeStatus(ChatChannel channel)
		{
			foreach (PartyPlayer player in _players.All)
			{
				float timeout = player.Timeout;

				if (channel.Subscribers.Contains(player.UserID) == true)
				{
					player.Status  = EPartyPlayerStatus.Connected;
					player.Timeout = 0.0f;

					if (timeout != 0.0f)
					{
						_log.Warning(ELogGroup.Party, "[SynchronizeStatus] Player {0} reconnected", player.UserID);
						PlayerReconnected.SafeInvoke(player);
					}
				}
				else
				{
					player.Status   = EPartyPlayerStatus.Disonnected;
					player.Timeout += Mathf.Min(Time.unscaledDeltaTime, 1.0f);

					if (timeout == 0.0f)
					{
						_log.Warning(ELogGroup.Party, "[SynchronizeStatus] Player {0} disconnected", player.UserID);
						PlayerDisconnected.SafeInvoke(player);
					}

					if (((int)player.Timeout) != ((int)timeout) && IsLeader == true)
					{
						SendMessage(new PartyMessages.Join(_partyID), player.UserID);
					}
				}
			}
		}

		private void CleanupProperties(ChatChannel channel)
		{
			if (Time.realtimeSinceStartup < _nextCleanupPropertiesTime)
				return;
			if (channel.TryGetProperty(PROPERTY_LEADER, out object leaderID) == false || (string)leaderID != UserID)
				return;
			if (channel.Properties == null)
				return;

			Dictionary<object, object> deleteProperties = null;

			foreach (KeyValuePair<object, object> property in channel.Properties)
			{
				if (property.Key is string key)
				{
					if (key != PROPERTY_LEADER && key != PROPERTY_PLAYERS && channel.Subscribers.Contains(key) == false && Players.Contains(key) == false)
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
				_nextCleanupPropertiesTime = Time.realtimeSinceStartup + 0.5f;

				_log.Info(ELogGroup.Party, "[CleanupProperties] Removing {0} properties from party!", deleteProperties.Count);

				_messages.SetProperties(_partyID, deleteProperties);
			}
		}

		private void SwitchLeader(string leaderUserID, string expectedUserID)
		{
			if (leaderUserID.HasValue() == false)
				return;

			if (Time.realtimeSinceStartup >= _nextLeaderRequestTime)
			{
				_nextLeaderRequestTime = Time.realtimeSinceStartup + 0.5f;
				_log.Info(ELogGroup.Party, "[SwitchLeader] Switching leader {0} => {1}", expectedUserID, leaderUserID);
				_messages.SetProperties(_partyID, new Dictionary<object, object>() { { PROPERTY_LEADER, leaderUserID } }, new Dictionary<object, object>() { { PROPERTY_LEADER, expectedUserID } });
			}
		}

		private void SendPlayerData(PartyPlayer player)
		{
			if (IsValid == false)     { _log.Error(ELogGroup.Party, "[SendPlayerData] You have no party!"); return; }
			if (IsConnected == false) { _log.Error(ELogGroup.Party, "[SendPlayerData] Not connected!");     return; }

			if (player.IsLocal == false && AllowRemotePlayerDataChange == false)
			{
				throw new InvalidOperationException("[SendPlayerData] Changing remote player data is not allowed!");
			}

			_log.Info(ELogGroup.Party, "[SendPlayerData]");

			_messages.SetProperties(_partyID, new Dictionary<object, object>() { { player.UserID, player.Data.GetData() } });
		}

		private int FindEmptySlot()
		{
			for (int slot = 0;;++slot)
			{
				if (_invites.Contains(slot) == false && _players.Contains(slot) == false)
				{
					return slot;
				}
			}
		}

		private void Stop(bool clearProperties)
		{
			if (this != null && _joinCoroutine != null)
			{
				StopCoroutine(_joinCoroutine);
			}

			_joinCoroutine = null;

			PartyPlayer localPlayer = GetLocalPlayer();

			_players.Clear();
			_invites.Clear();

			if (clearProperties == true)
			{
				_partyID    = null;
				_maxPlayers = 0;
			}

			if (localPlayer != null)
			{
				_log.Warning(ELogGroup.Party, "[Stop] Local Player {0} left party {1}", UserID, _partyID);
				Left.SafeInvoke(localPlayer);
			}
		}

		private IEnumerator JoinOrCreateCoroutine()
		{
			float timeout = Time.realtimeSinceStartup + 5.0f;
			while (_messages.IsConnected == false && Time.realtimeSinceStartup < timeout)
			{
				yield return null;
			}

			if (_messages.IsConnected == false)
			{
				_log.Warning(ELogGroup.Party, "Failed to join or create party {0}, not connected to chat!", _partyID);

				_joinCoroutine = null;
				yield break;
			}

			_log.Warning(ELogGroup.Party, "Join or create party {0} ...", _partyID);

			Dictionary<object, object> channelProperties = new Dictionary<object, object>();
			channelProperties[PROPERTY_LEADER] = UserID;

			ChannelCreationOptions createOptions = new ChannelCreationOptions();
			createOptions.PublishSubscribers = true;
			createOptions.MaxSubscribers     = _maxPlayers;
			createOptions.Properties         = channelProperties;

			_messages.Subscribe(_partyID, 0, 0, createOptions);
			_messages.Flush();

			timeout = Time.realtimeSinceStartup + 5.0f;
			while (IsConnected == false && Time.realtimeSinceStartup < timeout)
			{
				yield return null;
			}

			if (IsConnected == false)
			{
				_log.Warning(ELogGroup.Party, "Failed to join or create party {0}", _partyID);
			}

			_joinCoroutine = null;
		}
	}
}
