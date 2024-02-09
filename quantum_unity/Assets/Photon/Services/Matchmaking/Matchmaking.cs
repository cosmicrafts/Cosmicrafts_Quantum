namespace Quantum.Services
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using Photon.Realtime;

	using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

	public sealed class Matchmaking : MonoBehaviour, IService
	{
		//========== CONSTANTS ========================================================================================

		public const string KEY_SERVICE           = "MM1475963";
		public const string KEY_MIN_START_PLAYERS = "MinStartPlayers";
		public const string KEY_EXPECTED_PLAYERS  = "ExpectedPlayers";
		public const string KEY_EXTRA_SLOTS       = "ExtraSlots";
		public const string KEY_HAS_TIMEOUT       = "HasTimeout";
		public const string KEY_FILL_TIMEOUT      = "FillTimeout";
		public const string KEY_AUTO_START        = "AutoStart";
		public const string KEY_SKIP_CAS          = "SkipCAS";
		public const string KEY_MATCH_DATA        = "MatchData";
		public const string KEY_MATCH_READY       = "MatchReady";
		public const string KEY_MATCH_CONFIG      = "MatchConfig";
		public const string KEY_MATCH_STARTED     = "MatchStarted";
		public const string KEY_PLAYER_IGNORE     = "PlayerIgnore_";
		public const string KEY_IS_SPECTATOR      = "IsSpectator";

		private const float ROOM_OPERATION_DELAY = 0.5f;

		//========== PUBLIC MEMBERS ===================================================================================

		public bool  IsActive     { get { return _currentMatch != null || _pendingMatch != null; } }
		public Match CurrentMatch { get { return _currentMatch; } }
		public Match PendingMatch { get { return _pendingMatch; } }

		public event Func<string> CreateRoomName;

		//========== PRIVATE MEMBERS ==================================================================================

		private Log      _log;
		private Network  _network;
		private Messages _messages;
		private Match    _currentMatch;
		private Match    _pendingMatch;
		private float    _nextRoomOperationTime;

		//========== PUBLIC METHODS ===================================================================================

		public void Run(Match match)
		{
			_pendingMatch = match;
		}

		public Match Run(MatchRequest matchRequest)
		{
			_pendingMatch = new Match(matchRequest);
			return _pendingMatch;
		}

		public void Leave()
		{
			if (_currentMatch == null && _pendingMatch == null)
				return;

			_log.Warning(ELogGroup.Matchmaking, "Leave()");

			if (_currentMatch != null)
			{
				_currentMatch.Deinitialize();
			}

			_pendingMatch = null;
			_currentMatch = null;
		}

		public void SendMessage(IMatchmakingMessage message, string userID)
		{
			if (message is PrivateMessage == false)
			{
				throw new NotSupportedException("Only private message can be sent to player! " + message.GetType().FullName);
			}

			_messages.SendMessage(message, userID);
		}

		public static string GetPlayerIgnoreKey(string userID)
		{
			return KEY_PLAYER_IGNORE + userID;
		}

		//========== IService INTERFACE ===============================================================================

		void IService.Initialize(IServiceProvider serviceProvider)
		{
			_log      = serviceProvider.GetService<Log>();
			_network  = serviceProvider.GetService<Network>();
			_messages = serviceProvider.GetService<Messages>();
		}

		void IService.Deinitialize()
		{
			CreateRoomName = null;

			Leave();

			_messages = null;
			_network  = null;
			_log      = null;
		}

		void IService.Tick()
		{
			if (_network.Client != null && _network.Client.IsConnectedAndReady == true)
			{
				if (_pendingMatch != null)
				{
					if (_currentMatch != null)
					{
						_currentMatch.Deinitialize();
						_currentMatch = null;
					}

					if (_network.Client.InRoom == false)
					{
						_currentMatch = _pendingMatch;
						_pendingMatch = null;

						_log.Info(ELogGroup.Matchmaking, "Processing new Match");

						_currentMatch.Initialize(_log, _network, _messages, this);
					}
				}

				SynchronizeRoom();
			}

			if (_currentMatch != null)
			{
				_currentMatch.Tick();
			}
		}

		//========== PRIVATE METHODS ==================================================================================

		private void SynchronizeRoom()
		{
			if (Time.realtimeSinceStartup < _nextRoomOperationTime)
				return;

			if (_currentMatch == null)
			{
				TryLeaveRoom();
				return;
			}

			if (_network.Client.InRoom == true)
				return;

			MatchRequest request = _currentMatch.Request;

			_currentMatch.SetSpectator(request.IsSpectator);

			switch (request.Type)
			{
				case EMatchRequestType.Join:               { TryJoinRoom(request);               break; }
				case EMatchRequestType.Create:             { TryCreateRoom(request);             break; }
				case EMatchRequestType.JoinOrCreate:       { TryJoinOrCreateRoom(request);       break; }
				case EMatchRequestType.JoinRandom:         { TryJoinRandomRoom(request);         break; }
				case EMatchRequestType.CreateRandom:       { TryCreateRandomRoom(request);       break; }
				case EMatchRequestType.JoinOrCreateRandom: { TryJoinOrCreateRandomRoom(request); break; }
				default:
					throw new NotSupportedException(_currentMatch.Request.Type.ToString());
			}
		}

		private void TryJoinRoom(MatchRequest request)
		{
			RoomOptions roomOptions = new RoomOptions();
			roomOptions.Plugins = request.Plugin.HasValue() == true ? new string[] { request.Plugin } : null;

			EnterRoomParams enterRoomParams = new EnterRoomParams();
			enterRoomParams.RoomName          = request.Room;
			enterRoomParams.RoomOptions       = roomOptions;
			enterRoomParams.Lobby             = request.TypedLobby;
			enterRoomParams.PlayerProperties  = GeneratePlayerProperties(request);
			enterRoomParams.ExpectedUsers     = GenerateExpectedUsers(request.ExpectedUserIDs);

			_log.Info(ELogGroup.Matchmaking, "TryJoinRoom(), Room: {0}", request.Room);

			SetNextRoomOperationTime(ROOM_OPERATION_DELAY);

			_network.Client.OpJoinRoom(enterRoomParams);
		}

		private void TryCreateRoom(MatchRequest request)
		{
			RoomOptions roomOptions = new RoomOptions();
			roomOptions.IsOpen                       = request.IsOpen;
			roomOptions.IsVisible                    = request.IsVisible;
			roomOptions.MaxPlayers                   = (byte)(request.ExpectedPlayers + request.ExtraSlots);
			roomOptions.PublishUserId                = true;
			roomOptions.DeleteNullProperties         = true;
			roomOptions.CustomRoomProperties         = GenerateCustomRoomProperties(request);
			roomOptions.CustomRoomPropertiesForLobby = GenerateCustomRoomPropertiesForLobby(request);
			roomOptions.Plugins                      = request.Plugin.HasValue() == true ? new string[] { request.Plugin } : null;

			EnterRoomParams enterRoomParams = new EnterRoomParams();
			enterRoomParams.RoomName          = request.Room;
			enterRoomParams.RoomOptions       = roomOptions;
			enterRoomParams.Lobby             = request.TypedLobby;
			enterRoomParams.PlayerProperties  = GeneratePlayerProperties(request);
			enterRoomParams.ExpectedUsers     = GenerateExpectedUsers(request.ExpectedUserIDs);

			_log.Info(ELogGroup.Matchmaking, "TryCreateRoom(), Room: {0}", enterRoomParams.RoomName);

			SetNextRoomOperationTime(ROOM_OPERATION_DELAY);

			_network.Client.OpCreateRoom(enterRoomParams);
		}

		private void TryJoinOrCreateRoom(MatchRequest request)
		{
			RoomOptions roomOptions = new RoomOptions();
			roomOptions.IsOpen                       = request.IsOpen;
			roomOptions.IsVisible                    = request.IsVisible;
			roomOptions.MaxPlayers                   = (byte)(request.ExpectedPlayers + request.ExtraSlots);
			roomOptions.PublishUserId                = true;
			roomOptions.DeleteNullProperties         = true;
			roomOptions.CustomRoomProperties         = GenerateCustomRoomProperties(request);
			roomOptions.CustomRoomPropertiesForLobby = GenerateCustomRoomPropertiesForLobby(request);
			roomOptions.Plugins                      = request.Plugin.HasValue() == true ? new string[] { request.Plugin } : null;

			EnterRoomParams enterRoomParams = new EnterRoomParams();
			enterRoomParams.RoomName          = request.Room;
			enterRoomParams.RoomOptions       = roomOptions;
			enterRoomParams.Lobby             = request.TypedLobby;
			enterRoomParams.PlayerProperties  = GeneratePlayerProperties(request);
			enterRoomParams.ExpectedUsers     = GenerateExpectedUsers(request.ExpectedUserIDs);

			_log.Info(ELogGroup.Matchmaking, "TryJoinOrCreateRoom(), Room: {0}", enterRoomParams.RoomName);

			SetNextRoomOperationTime(ROOM_OPERATION_DELAY);

			_network.Client.OpJoinOrCreateRoom(enterRoomParams);
		}

		private void TryJoinRandomRoom(MatchRequest request)
		{
			OpJoinRandomRoomParams joinRandomRoomParams = new OpJoinRandomRoomParams();
			joinRandomRoomParams.ExpectedCustomRoomProperties = GenerateExpectedRoomProperties(request);
			joinRandomRoomParams.ExpectedMaxPlayers           = 0;
			joinRandomRoomParams.MatchingType                 = request.MatchmakingMode;
			joinRandomRoomParams.TypedLobby                   = request.TypedLobby;
			joinRandomRoomParams.SqlLobbyFilter               = request.SQLLobbyFilter;
			joinRandomRoomParams.ExpectedUsers                = GenerateExpectedUsers(request.ExpectedUserIDs);

			_log.Info(ELogGroup.Matchmaking, "TryJoinRandomRoom()");

			SetNextRoomOperationTime(ROOM_OPERATION_DELAY);

			_network.Client.OpJoinRandomRoom(joinRandomRoomParams);
		}

		private void TryCreateRandomRoom(MatchRequest request)
		{
			RoomOptions roomOptions = new RoomOptions();
			roomOptions.IsOpen                       = request.IsOpen;
			roomOptions.IsVisible                    = request.IsVisible;
			roomOptions.MaxPlayers                   = (byte)(request.ExpectedPlayers + request.ExtraSlots);
			roomOptions.PublishUserId                = true;
			roomOptions.DeleteNullProperties         = true;
			roomOptions.CustomRoomProperties         = GenerateCustomRoomProperties(request);
			roomOptions.CustomRoomPropertiesForLobby = GenerateCustomRoomPropertiesForLobby(request);
			roomOptions.Plugins                      = request.Plugin.HasValue() == true ? new string[] { request.Plugin } : null;

			EnterRoomParams enterRoomParams = new EnterRoomParams();
			enterRoomParams.RoomName          = GenerateRoomName();
			enterRoomParams.RoomOptions       = roomOptions;
			enterRoomParams.Lobby             = request.TypedLobby;
			enterRoomParams.PlayerProperties  = GeneratePlayerProperties(request);
			enterRoomParams.ExpectedUsers     = GenerateExpectedUsers(request.ExpectedUserIDs);

			_log.Info(ELogGroup.Matchmaking, "TryCreateRandomRoom(), Room: {0}", enterRoomParams.RoomName);

			SetNextRoomOperationTime(ROOM_OPERATION_DELAY);

			_network.Client.OpCreateRoom(enterRoomParams);
		}

		private void TryJoinOrCreateRandomRoom(MatchRequest request)
		{
			OpJoinRandomRoomParams joinRandomRoomParams = new OpJoinRandomRoomParams();
			joinRandomRoomParams.ExpectedCustomRoomProperties = GenerateExpectedRoomProperties(request);
			joinRandomRoomParams.ExpectedMaxPlayers           = 0;
			joinRandomRoomParams.MatchingType                 = request.MatchmakingMode;
			joinRandomRoomParams.TypedLobby                   = request.TypedLobby;
			joinRandomRoomParams.SqlLobbyFilter               = request.SQLLobbyFilter;
			joinRandomRoomParams.ExpectedUsers                = GenerateExpectedUsers(request.ExpectedUserIDs);

			RoomOptions roomOptions = new RoomOptions();
			roomOptions.IsOpen                       = request.IsOpen;
			roomOptions.IsVisible                    = request.IsVisible;
			roomOptions.MaxPlayers                   = (byte)(request.ExpectedPlayers + request.ExtraSlots);
			roomOptions.PublishUserId                = true;
			roomOptions.DeleteNullProperties         = true;
			roomOptions.CustomRoomProperties         = GenerateCustomRoomProperties(request);
			roomOptions.CustomRoomPropertiesForLobby = GenerateCustomRoomPropertiesForLobby(request);
			roomOptions.Plugins                      = request.Plugin.HasValue() == true ? new string[] { request.Plugin } : null;

			EnterRoomParams enterRoomParams = new EnterRoomParams();
			enterRoomParams.RoomName          = GenerateRoomName();
			enterRoomParams.RoomOptions       = roomOptions;
			enterRoomParams.Lobby             = request.TypedLobby;
			enterRoomParams.PlayerProperties  = GeneratePlayerProperties(request);
			enterRoomParams.ExpectedUsers     = GenerateExpectedUsers(request.ExpectedUserIDs);

			_log.Info(ELogGroup.Matchmaking, "TryJoinOrCreateRandomRoom()");

			SetNextRoomOperationTime(ROOM_OPERATION_DELAY);

			_network.Client.OpJoinRandomOrCreateRoom(joinRandomRoomParams, enterRoomParams);
		}

		private void TryLeaveRoom()
		{
			if (_network.Client.InRoom == false)
				return;

			_log.Info(ELogGroup.Matchmaking, "TryLeaveRoom()");

			SetNextRoomOperationTime(ROOM_OPERATION_DELAY);

			_network.Client.OpLeaveRoom(false);
		}

		private void SetNextRoomOperationTime(float delay)
		{
			_nextRoomOperationTime = Time.realtimeSinceStartup + delay;
		}

		private string[] GenerateExpectedUsers(List<string> expectedUserIDs)
		{
			List<string> expectedUsers = new List<string>();
			expectedUsers.Add(_network.Client.UserId);

			if (expectedUserIDs != null)
			{
				foreach (string userID in expectedUserIDs)
				{
					if (userID.HasValue() == true && expectedUsers.Contains(userID) == false)
					{
						expectedUsers.Add(userID);
					}
				}
			}

			return expectedUsers.ToArray();
		}

		private string GenerateRoomName()
		{
			string roomName = CreateRoomName.SafeInvoke();

			if (roomName.HasValue() == false)
			{
				roomName = Guid.NewGuid().ToString();
			}

			return roomName;
		}

		private PhotonHashtable GeneratePlayerProperties(MatchRequest request)
		{
			PhotonHashtable playerProperties = new PhotonHashtable();

			if (request.PlayerProperties != null)
			{
				playerProperties.Merge(request.PlayerProperties);
			}

			playerProperties[KEY_IS_SPECTATOR] = request.IsSpectator == true ? (object)true : null;

			return playerProperties;
		}

		private PhotonHashtable GenerateCustomRoomProperties(MatchRequest request)
		{
			PhotonHashtable roomProperties = new PhotonHashtable();

			if (request.RoomProperties != null)
			{
				roomProperties.Merge(request.RoomProperties);
			}

			roomProperties[KEY_SERVICE]           = true;
			roomProperties[KEY_MIN_START_PLAYERS] = request.MinStartPlayers;
			roomProperties[KEY_EXPECTED_PLAYERS]  = request.ExpectedPlayers;
			roomProperties[KEY_EXTRA_SLOTS]       = request.ExtraSlots;
			roomProperties[KEY_MATCH_STARTED]     = false;

			if (request.FillTimeout > 0)
			{
				roomProperties[KEY_HAS_TIMEOUT]  = true;
				roomProperties[KEY_FILL_TIMEOUT] = request.FillTimeout;
			}

			if (request.AutoStart == true)
			{
				roomProperties[KEY_AUTO_START] = true;
			}

			if (request.Config != null)
			{
				object data = request.Config.GetData();
				if (data != null)
				{
					roomProperties[KEY_MATCH_DATA] = data;
				}
			}

			roomProperties[GetPlayerIgnoreKey(_network.UserID)] = null;

			return roomProperties;
		}

		private string[] GenerateCustomRoomPropertiesForLobby(MatchRequest request)
		{
			List<string> roomPropertiesForLobby = new List<string>();

			if (request.RoomPropertiesForLobby != null)
			{
				roomPropertiesForLobby.AddRange(request.RoomPropertiesForLobby);
			}

			roomPropertiesForLobby.Add(KEY_SERVICE);
			roomPropertiesForLobby.Add(KEY_EXPECTED_PLAYERS);
			roomPropertiesForLobby.Add(KEY_MATCH_STARTED);

			return roomPropertiesForLobby.ToArray();
		}

		private PhotonHashtable GenerateExpectedRoomProperties(MatchRequest request)
		{
			PhotonHashtable expectedRoomProperties = new PhotonHashtable();

			if (request.ExpectedRoomProperties != null)
			{
				expectedRoomProperties.Merge(request.ExpectedRoomProperties);
			}

			expectedRoomProperties[KEY_SERVICE]          = true;
			expectedRoomProperties[KEY_EXPECTED_PLAYERS] = request.ExpectedPlayers;

			return expectedRoomProperties;
		}
	}
}
