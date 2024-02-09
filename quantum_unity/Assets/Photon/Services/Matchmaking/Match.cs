namespace Quantum.Services
{
	using System;
	using UnityEngine;
	using Photon.Realtime;

	using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

	public sealed partial class Match
	{
		//========== CONSTANTS ========================================================================================

		private const float  INTERVAL_UPDATE_START           = 1.0f;
		private const float  INTERVAL_INVITE_MISSING_PLAYERS = 1.0f;
		private const int    INTERVAL_UPDATE_TIMEOUT         = 1;

		//========== PUBLIC MEMBERS ===================================================================================

		public MatchRequest  Request         { get; private set; }
		public int           MinStartPlayers { get; private set; }
		public int           ExpectedPlayers { get; private set; }
		public int           ExtraSlots      { get; private set; }
		public int           FillTimeout     { get; private set; }
		public bool          IsConnected     { get; private set; }
		public bool          IsSpectator     { get; private set; }
		public bool          HasTimeout      { get; private set; }
		public bool          HasStarted      { get; private set; }
		public bool          AutoStart       { get; private set; }
		public MatchConfig   Config          { get; private set; }

		public NetworkClient Client          { get { return _client; } }
		public bool          IsInitialized   { get { return _client != null; } }
		public bool          IsInRoom        { get { return _client != null && _client.InRoom == true; } }
		public bool          IsOpen          { get { return _client != null && _client.InRoom == true && _client.CurrentRoom.IsOpen    == true; } }
		public bool          IsVisible       { get { return _client != null && _client.InRoom == true && _client.CurrentRoom.IsVisible == true; } }
		public bool          IsLeader        { get { return _client != null && _client.LocalPlayer.IsMasterClient == true; } }
		public Player        Player          { get { return _client != null ? _client.LocalPlayer : null; } }
		public Room          Room            { get { return _client != null ? _client.CurrentRoom : null; } }

		public float         TimeConnected   { get; private set; }
		public float         TimeConnecting  { get; private set; }

		public event Action<Match, MatchConfig> UpdateConfig;
		public event Action<Match>              Connected;
		public event Action<Match>              Reconnected;
		public event Action<Match>              Disconnected;
		public event Action<Match>              Updated;
		public event Action<Match>              Started;

		//========== PRIVATE MEMBERS ==================================================================================

		private Log           _log;
		private Network       _network;
		private Messages      _messages;
		private Matchmaking   _matchmaking;
		private NetworkClient _client;
		private float         _nextInviteTime;
		private float         _nextStartUpdateTime;
		private float         _nextTimeoutUpdateTime;
		private bool          _propertiesSynchronized;

		//========== CONSTRUCTORS =====================================================================================

		public Match(MatchRequest request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));
			if (request.IsValid() == false)
				throw new InvalidOperationException("Request is not valid!");

			Request = request;
		}

		//========== PUBLIC METHODS ===================================================================================

		public void Initialize(Log log, Network network, Messages messages, Matchmaking matchmaking)
		{
			_log         = log;
			_network     = network;
			_messages    = messages;
			_matchmaking = matchmaking;

			_client = network.Client;

			if (_messages != null)
			{
				_messages.MessageReceived += OnMessageReceived;
			}
		}

		public void Deinitialize()
		{
			if (_messages != null)
			{
				_messages.MessageReceived -= OnMessageReceived;
			}

			UpdateConfig = null;
			Connected    = null;
			Reconnected  = null;
			Disconnected = null;
			Updated      = null;
			Started      = null;

			LeaveMatch();

			_network     = null;
			_matchmaking = null;
			_messages    = null;
			_client      = null;
			_log         = null;
		}

		public void Tick()
		{
			if (_client != _network.Client)
			{
				_client = _network.Client;
				_propertiesSynchronized = false;

				TimeConnected  = 0.0f;
				TimeConnecting = 0.0f;

				_log.Warning(ELogGroup.Matchmaking, "Network client changed!");
			}

			if (_client == null || _client.InRoom == false)
			{
				_propertiesSynchronized = false;

				TimeConnected   = 0.0f;
				TimeConnecting += Time.unscaledDeltaTime;

				if (IsConnected == true)
				{
					IsConnected = false;

					_log.Warning(ELogGroup.Matchmaking, "Disconnected!");
					Disconnected.SafeInvoke(this);
				}

				return;
			}

			TimeConnected += Time.unscaledDeltaTime;
			TimeConnecting = 0.0f;

			if (IsConnected == false)
			{
				IsConnected = true;

				if (HasStarted == true)
				{
					_log.Warning(ELogGroup.Matchmaking, "Reconnected!");
					Reconnected.SafeInvoke(this);
				}
				else
				{
					_log.Warning(ELogGroup.Matchmaking, "Connected!");
					Connected.SafeInvoke(this);
				}
			}

			CreateRejoinRequest();
			SynchronizeProperties(false);
			InviteMissingPlayers();

			if (HasStarted == true)
				return;

			ProcessFillTimeout();
			CheckStart();

			if (HasStarted == true)
				return;

			Updated.SafeInvoke(this);
		}

		public void SetIgnorePlayer(string userID, bool ignorePlayer)
		{
			if (userID.HasValue() == false)
				return;
			if (IsInRoom == false)
				return;

			PhotonHashtable roomProperties = new PhotonHashtable();
			roomProperties[Matchmaking.GetPlayerIgnoreKey(userID)] = ignorePlayer == true ? (object)true : null;
			_client.CurrentRoom.SetCustomProperties(roomProperties);

			if (ignorePlayer == true)
			{
				_client.RemoveExpectedUser(userID);
			}
		}

		public void SetSpectator(bool isSpectator)
		{
			PhotonHashtable playerProperties = new PhotonHashtable();
			playerProperties[Matchmaking.KEY_IS_SPECTATOR] = isSpectator == true ? (object)true : null;
			_client.LocalPlayer.SetCustomProperties(playerProperties);
		}

		public void SetOpen(bool isOpen)
		{
			if (IsInRoom == true)
			{
				_log.Warning(ELogGroup.Matchmaking, "Match.SetOpen({0})", isOpen);

				_client.CurrentRoom.IsOpen = isOpen;
			}
		}

		public void SetVisibility(bool isVisible)
		{
			if (IsInRoom == true)
			{
				_log.Warning(ELogGroup.Matchmaking, "Match.SetVisibility({0})", isVisible);

				_client.CurrentRoom.IsVisible = isVisible;
			}
		}

		public void Start()
		{
			if (IsInRoom == true && HasStarted == false)
			{
				StartMatch();
			}
		}

		public bool HasPlayer(string userID)
		{
			if (IsInRoom == false)
				return false;

			foreach (Player player in _client.CurrentRoom.Players.Values)
			{
				if (player.UserId == userID)
					return true;
			}

			return false;
		}

		public Player GetPlayer(string userID)
		{
			if (IsInRoom == false)
				return null;

			foreach (Player player in _client.CurrentRoom.Players.Values)
			{
				if (player.UserId == userID)
					return player;
			}

			return null;
		}

		public int GetPlayerCount(bool includeSpectators)
		{
			if (IsInRoom == false)
				return 0;

			if (includeSpectators == true)
				return _client.CurrentRoom.PlayerCount;

			int count = 0;

			foreach (Player player in _client.CurrentRoom.Players.Values)
			{
				if (player.CustomProperties.ContainsKey(Matchmaking.KEY_IS_SPECTATOR) == false)
				{
					++count;
				}
			}

			return count;
		}

		public bool IsPlayerSpectator(string userID)
		{
			Player player = GetPlayer(userID);
			return player != null && player.CustomProperties.ContainsKey(Matchmaking.KEY_IS_SPECTATOR) == true;
		}

		public bool IsPlayerIgnored(string userID)
		{
			return _client != null && _client.InRoom == true && _client.CurrentRoom.CustomProperties.ContainsKey(Matchmaking.GetPlayerIgnoreKey(userID)) == true;
		}

		//========== PRIVATE METHODS ==================================================================================

		private void CreateRejoinRequest()
		{
			if (Request.Type != EMatchRequestType.Join && (_client.CurrentRoom.PlayerCount > 1 || _client.CurrentRoom.EmptyRoomTtl > 0))
			{
				_log.Info(ELogGroup.Matchmaking, "Switching match request type: {0} => {1} to reconnect in case of unexpected disconnect", Request.Type, EMatchRequestType.Join);

				Request.Type = EMatchRequestType.Join;
				Request.Room = _client.CurrentRoom.Name;
				Request.ExpectedUserIDs.Clear();
			}
		}

		private void SynchronizeProperties(bool forceOverride)
		{
			if (_propertiesSynchronized == true && forceOverride == false)
				return;

			_propertiesSynchronized = true;

			try
			{
				_client.LocalPlayer.SetCustomProperties(Request.PlayerProperties);

				SetIgnorePlayer(_client.UserId, false);
				SetSpectator(Request.IsSpectator);

				MinStartPlayers = (int)_client.CurrentRoom.CustomProperties[Matchmaking.KEY_MIN_START_PLAYERS];
				ExpectedPlayers = (int)_client.CurrentRoom.CustomProperties[Matchmaking.KEY_EXPECTED_PLAYERS];
				ExtraSlots      = (int)_client.CurrentRoom.CustomProperties[Matchmaking.KEY_EXTRA_SLOTS];
				IsSpectator     = _client.LocalPlayer.CustomProperties.ContainsKey(Matchmaking.KEY_IS_SPECTATOR);
				HasTimeout      = _client.CurrentRoom.CustomProperties.ContainsKey(Matchmaking.KEY_HAS_TIMEOUT);
				AutoStart       = _client.CurrentRoom.CustomProperties.ContainsKey(Matchmaking.KEY_AUTO_START);

				if (HasTimeout == true)
				{
					FillTimeout = (int)_client.CurrentRoom.CustomProperties[Matchmaking.KEY_FILL_TIMEOUT];

					_nextTimeoutUpdateTime = Time.realtimeSinceStartup + INTERVAL_UPDATE_TIMEOUT;
				}

				_log.Info(ELogGroup.Matchmaking, "Synchronized room properties");
			}
			catch (Exception exception)
			{
				_log.Exception(ELogGroup.Matchmaking, exception);
			}
		}

		private void InviteMissingPlayers()
		{
			if (Time.realtimeSinceStartup < _nextInviteTime)
				return;
			if (_client.LocalPlayer.IsMasterClient == false)
				return;

			_nextInviteTime = Time.realtimeSinceStartup + INTERVAL_INVITE_MISSING_PLAYERS;

			foreach (string userID in _client.CurrentRoom.ExpectedUsers)
			{
				if (userID.HasValue() == true && IsPlayerIgnored(userID) == false && HasPlayer(userID) == false)
				{
					_matchmaking.SendMessage(new MatchmakingMessages.Join(_network.Region, Room.Name, HasStarted), userID);
				}
			}
		}

		private void ProcessFillTimeout()
		{
			if (_client.CurrentRoom.CustomProperties.ContainsKey(Matchmaking.KEY_HAS_TIMEOUT) == true)
			{
				HasTimeout  = true;
				FillTimeout = (int)_client.CurrentRoom.CustomProperties[Matchmaking.KEY_FILL_TIMEOUT];

				if (FillTimeout > 0 && Time.realtimeSinceStartup >= _nextTimeoutUpdateTime && _client.LocalPlayer.IsMasterClient == true)
				{
					_nextTimeoutUpdateTime = Time.realtimeSinceStartup + INTERVAL_UPDATE_TIMEOUT;

					PhotonHashtable roomProperties = new PhotonHashtable();
					roomProperties[Matchmaking.KEY_FILL_TIMEOUT] = FillTimeout - INTERVAL_UPDATE_TIMEOUT;

					PhotonHashtable expectedProperties = new PhotonHashtable();
					expectedProperties[Matchmaking.KEY_SKIP_CAS] = null;

					_client.CurrentRoom.SetCustomProperties(roomProperties, expectedProperties);
				}
			}
		}

		private void CheckStart()
		{
			if (_client.CurrentRoom.CustomProperties.ContainsKey(Matchmaking.KEY_MATCH_READY) == true)
			{
				if (_client.CurrentRoom.CustomProperties.TryGetValue(Matchmaking.KEY_MATCH_CONFIG, out object config) == true)
				{
					SynchronizeProperties(true);

					Config = new MatchConfig();
					Config.SetData(config);

					_log.Info(ELogGroup.Matchmaking, "Config loaded!");
				}

				_log.Info(ELogGroup.Matchmaking, "Match started!");

				HasStarted = true;

				Started.SafeInvoke(this);
			}
			else
			{
				if (Time.realtimeSinceStartup >= _nextStartUpdateTime && _client.LocalPlayer.IsMasterClient == true && _client.CurrentRoom.CustomProperties.ContainsKey(Matchmaking.KEY_AUTO_START) == true)
				{
					bool startMatch = HasTimeout == true && FillTimeout == 0;
					if (startMatch == false)
					{
						int playerCount = GetPlayerCount(false);
						startMatch = playerCount >= ExpectedPlayers || (MinStartPlayers > 0 && playerCount >= MinStartPlayers);
					}

					if (startMatch == true)
					{
						_nextStartUpdateTime = Time.realtimeSinceStartup + INTERVAL_UPDATE_START;

						StartMatch();
					}
				}
			}
		}

		private void StartMatch()
		{
			object config = null;

			if (_client.CurrentRoom.CustomProperties.TryGetValue(Matchmaking.KEY_MATCH_DATA, out object data) == true)
			{
				_log.Info(ELogGroup.Matchmaking, "Updating Config");

				MatchConfig dataConfig = new MatchConfig();
				dataConfig.SetData(data);

				UpdateConfig.SafeInvoke(this, dataConfig);

				config = dataConfig.GetData();
			}

			int players    = GetPlayerCount(false);
			int spectators = GetPlayerCount(true) - players;

			_log.Info(ELogGroup.Matchmaking, "Starting match! Players: {0} Spectators: {1}", players, spectators);

			PhotonHashtable roomProperties = new PhotonHashtable();
			roomProperties[Matchmaking.KEY_FILL_TIMEOUT]  = 0;
			roomProperties[Matchmaking.KEY_MATCH_CONFIG]  = config;
			roomProperties[Matchmaking.KEY_MATCH_STARTED] = true;
			roomProperties[Matchmaking.KEY_MATCH_READY]   = true;

			PhotonHashtable expectedProperties = new PhotonHashtable();
			expectedProperties[Matchmaking.KEY_SKIP_CAS] = null;

			_client.CurrentRoom.SetCustomProperties(roomProperties, expectedProperties);
		}

		private void LeaveMatch()
		{
			if (IsInRoom == false)
				return;

			SetIgnorePlayer(_client.UserId, true);

			_log.Warning(ELogGroup.Matchmaking, "Match.Leave()");

			_client.OpLeaveRoom(false);
			_client.LoadBalancingPeer.SendOutgoingCommands();
		}

		private void OnMessageReceived(IMessage message)
		{
			if (message is MatchmakingMessages.Decline decline)
			{
				if (_client.InRoom == true && _client.CurrentRoom.Name == decline.Room)
				{
					SetIgnorePlayer(decline.Sender, true);
				}
			}
		}
	}
}
