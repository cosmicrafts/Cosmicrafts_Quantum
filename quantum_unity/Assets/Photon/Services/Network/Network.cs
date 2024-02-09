using TowerRush;

namespace Quantum.Services
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using ExitGames.Client.Photon;
	using Photon.Realtime;

	public sealed class Network : MonoBehaviour, IService, IConnectionCallbacks, IMatchmakingCallbacks, IErrorInfoCallback
	{
		//========== CONSTANTS ========================================================================================

		private const float TIMEOUT_CONNECT_UDP = 3.0f;
		private const float TIMEOUT_CONNECT_TCP = 5.0f;
		private const float TIMEOUT_DISCONNECT  = 5.0f;
		private const float TIMEOUT_RECONNECT   = 5.0f;

		//========== PUBLIC MEMBERS ===================================================================================

		public string        AppID       { get { return _appID;   } }
		public string        Version     { get { return _version; } }
		public string        Region      { get { return _region;  } }
		public string        UserID      { get { return _userID;  } }
		public NetworkClient Client      { get { return _client;  } }
		public bool          IsConnected { get { return _client != null && _client.IsConnected; } }

		//========== PRIVATE MEMBERS ==================================================================================

		private string        _appID;
		private string        _version;
		private string        _region;
		private string        _userID;
		private NetworkClient _client;
		private Coroutine     _connectCoroutine;
		private float         _reconnectTime;
		private bool          _isPaused;
		private Log           _log;

		//========== PUBLIC METHODS ===================================================================================

		public void Connect(string appID, string version, string region, string userID)
		{
			if (appID.HasValue()   == false) { _log.Error(ELogGroup.Network, "[Connect] Invalid appID {0}!", appID);     return; }
			if (version.HasValue() == false) { _log.Error(ELogGroup.Network, "[Connect] Invalid version {0}!", version); return; }
			if (userID.HasValue()  == false) { _log.Error(ELogGroup.Network, "[Connect] Invalid userID {0}!", userID);   return; }

			if (IsConnected == true && _appID == appID && _version == version && _region == region && _userID == userID)
				return;

			Stop();

			_appID   = appID;
			_version = version;
			_region  = region;
			_userID  = userID;

			_reconnectTime = 0.0f;
			_isPaused      = false;

			_connectCoroutine = StartCoroutine(ReconnectCoroutine());
		}

		public void Disconnect()
		{
			Stop();

			if (_client != null && _client.State != ClientState.PeerCreated && _client.State != ClientState.Disconnected)
			{
				_log.Warning(ELogGroup.Network, "Disconnecting from Realtime");

				_client.Disconnect(DisconnectCause.DisconnectByClientLogic);
				_client.LoadBalancingPeer.SendOutgoingCommands();
			}

			DisposeClient();

			_client  = default;
			_userID  = default;
			_region  = default;
			_version = default;
			_appID   = default;
		}

		public void Pause()
		{
			if (_isPaused == true)
				return;

			_isPaused = true;

			SupportClass.StartBackgroundCalls(SendAcksOnly, 100);
		}

		public void Unpause()
		{
			_isPaused = false;
		}

		//========== IService INTERFACE ===============================================================================

		void IService.Initialize(IServiceProvider serviceProvider)
		{
			_log = serviceProvider.GetService<Log>();
		}

		void IService.Deinitialize()
		{
			Disconnect();

			_log = null;
		}

		void IService.Tick()
		{
			if (_isPaused == true)
				return;

			if (_client != null)
			{
				_client.Service();
			}

			if (_userID.HasValue() == false)
				return;

			if (IsConnected == true || _connectCoroutine != null)
			{
				_reconnectTime = 0.0f;
				return;
			}

			_reconnectTime += Time.unscaledDeltaTime;
			if (_reconnectTime > TIMEOUT_RECONNECT)
			{
				_connectCoroutine = StartCoroutine(ReconnectCoroutine());
			}
		}

		//========== IConnectionCallbacks INTERFACE ===================================================================

		void IConnectionCallbacks.OnConnected()
		{
			_log.Info(ELogGroup.Network, "OnConnected");
		}

		
		void IConnectionCallbacks.OnConnectedToMaster()
		{
			_log.Info(ELogGroup.Network, "OnConnectedToMaster");
			
		}

		void IConnectionCallbacks.OnDisconnected(DisconnectCause cause)
		{
			_log.Info(ELogGroup.Network, "OnDisconnected {0}", cause);
		}

		void IConnectionCallbacks.OnRegionListReceived(RegionHandler regionHandler)
		{
			_log.Info(ELogGroup.Network, "OnRegionListReceived {0}", regionHandler.SummaryToCache);
		}

		void IConnectionCallbacks.OnCustomAuthenticationResponse(Dictionary<string, object> data)
		{
			_log.Info(ELogGroup.Network, "OnCustomAuthenticationResponse {0}", data.ToStringFull());
		}

		void IConnectionCallbacks.OnCustomAuthenticationFailed(string debugMessage)
		{
			_log.Info(ELogGroup.Network, "OnCustomAuthenticationFailed {0}", debugMessage);
		}

		//========== IMatchmakingCallbacks INTERFACE ==================================================================

		void IMatchmakingCallbacks.OnFriendListUpdate(List<FriendInfo> friendList)
		{
			_log.Info(ELogGroup.Network, "OnFriendListUpdate {0}", friendList.ToStringFull());
		}

		void IMatchmakingCallbacks.OnCreatedRoom()
		{
			_log.Info(ELogGroup.Network, "OnCreatedRoom");
		}

		void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message)
		{
			_log.Info(ELogGroup.Network, "OnCreateRoomFailed {0} {1}", returnCode, message);
		}

		void IMatchmakingCallbacks.OnJoinedRoom()
		{
			_log.Info(ELogGroup.Network, "OnJoinedRoom");
		}

		void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message)
		{
			_log.Info(ELogGroup.Network, "OnJoinRoomFailed {0} {1}", returnCode, message);
		}

		void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message)
		{
			_log.Info(ELogGroup.Network, "OnJoinRandomFailed {0} {1}", returnCode, message);
		}

		void IMatchmakingCallbacks.OnLeftRoom()
		{
			_log.Info(ELogGroup.Network, "OnLeftRoom");
		}

		//========== IErrorInfoCallback INTERFACE =====================================================================

		void IErrorInfoCallback.OnErrorInfo(ErrorInfo errorInfo)
		{
			_log.Error(ELogGroup.Realtime, errorInfo.ToString());
		}

		//========== PRIVATE METHODS ==================================================================================

		private void OnStateChanged(ClientState previousState, ClientState currentState)
		{
			_log.Info(ELogGroup.Network, "OnStateChanged {0} => {1}", previousState, currentState);
		}

		private void Stop()
		{
			if (_connectCoroutine != null)
			{
				StopCoroutine(_connectCoroutine);
			}

			_connectCoroutine = null;
		}

		private bool SendAcksOnly()
		{
			if (_client != null)
			{
				_client.LoadBalancingPeer.SendAcksOnly();
			}

			return _isPaused;
		}

		private IEnumerator ReconnectCoroutine()
		{
			// 1. Disconnect existing connection

			if (_client != null && _client.State != ClientState.PeerCreated && _client.State != ClientState.Disconnected)
			{
				_log.Warning(ELogGroup.Network, "Disconnecting from Realtime");

				_client.Disconnect(DisconnectCause.DisconnectByClientLogic);
				_client.LoadBalancingPeer.SendOutgoingCommands();

				float disconnectTimeout = Time.realtimeSinceStartup + TIMEOUT_DISCONNECT;
				while (_client != null && _client.State != ClientState.Disconnected && Time.realtimeSinceStartup < disconnectTimeout)
				{
					yield return null;
				}

				if (_client.State != ClientState.Disconnected)
				{
					_log.Error(ELogGroup.Network, "Failed to disconnect from Realtime!");
				}
			}

			DisposeClient();

			// 2. Try to connect with UDP

			ConnectionProtocol connectionProtocol = ConnectionProtocol.Udp;

			CreateClient(connectionProtocol);

			_log.Warning(ELogGroup.Network, "Connecting to Realtime. AppID: {0}, Version: {1}, Region: {2}, UserID: {3}, Protocol: {4}", _appID, _version, _region, _userID, connectionProtocol);

			if (_client.ConnectUsingSettings(GetAppSettings(connectionProtocol)) == true)
			{
				float connectTimeout = Time.realtimeSinceStartup + TIMEOUT_CONNECT_UDP;
				while (IsConnected == false && Time.realtimeSinceStartup < connectTimeout)
				{
					yield return null;
				}

				if (IsConnected == true)
				{
					_log.Warning(ELogGroup.Network, "Connected to Realtime. AppID: {0}, Version: {1}, Region: {2}, UserID: {3}, Protocol: {4}, State: {5}", _appID, _version, _region, _userID, connectionProtocol, _client.State);

					_connectCoroutine = null;
					yield break;
				}
				else
				{
					_log.Error(ELogGroup.Network, "Failed to connect to Realtime! AppID: {0}, Version: {1}, Region: {2}, UserID: {3}, Protocol: {4}, State: {5}", _appID, _version, _region, _userID, connectionProtocol, _client.State);
				}
			}
			else
			{
				_log.Error(ELogGroup.Network, "Failed to connect to Realtime! AppID: {0}, Version: {1}, Region: {2}, UserID: {3}, Protocol: {4}", _appID, _version, _region, _userID, connectionProtocol);
			}

			_client.Disconnect(DisconnectCause.DisconnectByClientLogic);
			_client.LoadBalancingPeer.SendOutgoingCommands();

			DisposeClient();

			// 3. Try to connect with TCP

			connectionProtocol = ConnectionProtocol.Tcp;

			CreateClient(connectionProtocol);

			_log.Warning(ELogGroup.Network, "Connecting to Realtime. AppID: {0}, Version: {1}, Region: {2}, UserID: {3}, Protocol: {4}", _appID, _version, _region, _userID, connectionProtocol);

			if (_client.ConnectUsingSettings(GetAppSettings(connectionProtocol)) == true)
			{
				float connectTimeout = Time.realtimeSinceStartup + TIMEOUT_CONNECT_TCP;
				while (IsConnected == false && Time.realtimeSinceStartup < connectTimeout)
				{
					yield return null;
				}

				if (IsConnected == true)
				{
					_log.Warning(ELogGroup.Network, "Connected to Realtime. AppID: {0}, Version: {1}, Region: {2}, UserID: {3}, Protocol: {4}, State: {5}", _appID, _version, _region, _userID, connectionProtocol, _client.State);

					_connectCoroutine = null;
					yield break;
				}
				else
				{
					_log.Error(ELogGroup.Network, "Failed to connect to Realtime! AppID: {0}, Version: {1}, Region: {2}, UserID: {3}, Protocol: {4}, State: {5}", _appID, _version, _region, _userID, connectionProtocol, _client.State);
				}
			}
			else
			{
				_log.Error(ELogGroup.Network, "Failed to connect to Realtime! AppID: {0}, Version: {1}, Region: {2}, UserID: {3}, Protocol: {4}", _appID, _version, _region, _userID, connectionProtocol);
			}

			_client.Disconnect(DisconnectCause.DisconnectByClientLogic);
			_client.LoadBalancingPeer.SendOutgoingCommands();

			DisposeClient();

			_connectCoroutine = null;
		}

		private void CreateClient(ConnectionProtocol connectionProtocol)
		{
			_client = new NetworkClient(connectionProtocol);

			_client.LoadBalancingPeer.DebugOut = DebugLevel.ERROR;

			_client.StateChanged += OnStateChanged;

			_client.AddCallbackTarget(this);

			_client.AppId      = _appID;
			_client.AppVersion = _version;
			_client.AuthValues = new AuthenticationValues(_userID);
		}

		private void DisposeClient()
		{
			if (_client != null)
			{
				_client.StateChanged -= OnStateChanged;

				_client.RemoveCallbackTarget(this);

				_client = null;
			}
		}

		private AppSettings GetAppSettings(ConnectionProtocol connectionProtocol)
		{
			AppSettings appSettings = new AppSettings();
			appSettings.AppIdRealtime = _appID;
			appSettings.AppVersion    = _version;
			appSettings.Protocol      = connectionProtocol;
			appSettings.FixedRegion   = _region.HasValue() == true ? _region : default;

			return appSettings;
		}
	}
}
