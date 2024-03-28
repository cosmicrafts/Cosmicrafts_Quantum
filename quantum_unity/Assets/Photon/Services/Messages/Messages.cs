namespace Quantum.Services
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using ExitGames.Client.Photon;
	using Photon.Chat;

	public sealed class Messages : MonoBehaviour, IService, IChatClientListener
	{
		//========== CONSTANTS ========================================================================================

		private const float TIMEOUT_CONNECT_UDP = 3.0f;
		private const float TIMEOUT_CONNECT_TCP = 5.0f;
		private const float TIMEOUT_DISCONNECT  = 5.0f;
		private const float TIMEOUT_RECONNECT   = 5.0f;

		//========== PUBLIC MEMBERS ===================================================================================

		public event Action                                     Connected;
		public event Action                                     Disconnected;
		public event Action<IMessage>                           MessageSent;
		public event Action<IMessage>                           MessageReceived;
		public event Action<string, string>                     UserSubscribed;
		public event Action<string, string>                     UserUnsubscribed;
		public event Action<string, bool>                       ChannelSubscribed;
		public event Action<string>                             ChannelUnsubscribed;
		public event Action<string, Dictionary<object, object>> ChannelPropertiesChanged;

		public string     AppID       { get { return _appID;   } }
		public string     Version     { get { return _version; } }
		public string     Region      { get { return _region;  } }
		public string     UserID      { get { return _userID;  } }
		public ChatClient Client      { get { return _client;  } }
		public bool       IsConnected { get { return _client != null && _client.CanChat == true; } }

		//========== PRIVATE MEMBERS ==================================================================================

		private string       _appID;
		private string       _version;
		private string       _region;
		private string       _userID;
		private ChatClient   _client;
		private ChatListener _listener;
		private Coroutine    _connectCoroutine;
		private float        _reconnectTime;
		private bool         _isPaused;
		private Log          _log;

		//========== PUBLIC METHODS ===================================================================================

		public void Connect(string appID, string version, string region, string userID)
		{
			if (appID.HasValue()   == false) { _log.Error(ELogGroup.Messages, "[Connect] Invalid appID {0}!", appID);     return; }
			if (version.HasValue() == false) { _log.Error(ELogGroup.Messages, "[Connect] Invalid version {0}!", version); return; }
			if (userID.HasValue()  == false) { _log.Error(ELogGroup.Messages, "[Connect] Invalid userID {0}!", userID);   return; }

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

			if (_client != null && _client.State != ChatState.Uninitialized && _client.State != ChatState.Disconnected)
			{
				_log.Warning(ELogGroup.Messages, "Disconnecting from Chat");

				_client.Disconnect();
				_client.chatPeer.SendOutgoingCommands();
			}

			DisposeClient();

			_client  = default;
			_userID  = default;
			_region  = default;
			_version = default;
			_appID   = default;
		}

		public bool Subscribe(string channel, int lastMessageID = 0, int messagesFromHistory = -1, ChannelCreationOptions createOptions = null)
		{
			return _client != null && _client.Subscribe(channel, lastMessageID, messagesFromHistory, createOptions);
		}

		public bool Unsubscribe(string channel)
		{
			return _client != null && _client.Unsubscribe(new string[] { channel });
		}

		public bool SetProperties(string channel, Dictionary<object, object> properties, Dictionary<object, object> expectedProperties = null)
		{
			return _client != null && _client.SetProperties(channel, properties, expectedProperties);
		}

		public bool AddFriends(string[] userIDs)
		{
			return _client != null && _client.AddFriends(userIDs);
		}

		public bool RemoveFriends(string[] userIDs)
		{
			return _client != null && _client.RemoveFriends(userIDs);
		}

		public bool CanChatInChannel(string name)
		{
			return _client != null && _client.CanChatInChannel(name);
		}

		public bool TryGetChannel(string name, out ChatChannel channel)
		{
			if (_client == null)
			{
				channel = null;
				return false;
			}

			return _client.TryGetChannel(name, out channel);
		}

		public void SendMessage(IMessage message, string receiver)
		{
			if (_client != null)
			{
				message.Send(_client, receiver);
				_log.Info(ELogGroup.Messages, "Sent {0} message to {1}", message.GetType().FullName, receiver);

				MessageSent.SafeInvoke(message);
			}
		}

		public void Flush()
		{
			if (_client != null)
			{
				_client.chatPeer.SendOutgoingCommands();
			}
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

			Connected                = null;
			Disconnected             = null;
			MessageReceived          = null;
			UserSubscribed           = null;
			UserUnsubscribed         = null;
			ChannelSubscribed        = null;
			ChannelUnsubscribed      = null;
			ChannelPropertiesChanged = null;
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

		//========== IChatClientListener INTERFACE ====================================================================

		void IChatClientListener.DebugReturn(DebugLevel level, string message)
		{
			switch (level)
			{
				case DebugLevel.OFF:
					break;
				case DebugLevel.ERROR:
					_log.Error(ELogGroup.Chat, message);
					break;
				case DebugLevel.WARNING:
					_log.Warning(ELogGroup.Chat, message);
					break;
				case DebugLevel.INFO:
					_log.Info(ELogGroup.Chat, message);
					break;
				case DebugLevel.ALL:
					_log.Info(ELogGroup.Chat, message);
					break;
			}
		}

		void IChatClientListener.OnDisconnected()
		{
			_log.Warning(ELogGroup.Messages, "Disconnected from Chat");

			Disconnected.SafeInvoke();
		}

		void IChatClientListener.OnConnected()
		{
			_log.Warning(ELogGroup.Messages, "Connected to Chat. App ID: {0}, Version: {1}, Region: {2}, Protocol: {3}", _client.AppId, _client.AppVersion, _client.ChatRegion, _client.chatPeer.UsedProtocol);

			Connected.SafeInvoke();
		}

		void IChatClientListener.OnChatStateChange(ChatState state)
		{
			_log.Info(ELogGroup.Messages, "Chat state changed {0}", state);
		}

		void IChatClientListener.OnGetMessages(string channelName, string[] senders, object[] messages)
		{
			if (_connectCoroutine != null)
				return;

			for (int i = 0; i < messages.Length; ++i)
			{
				string sender = senders[i];
				if (sender == _userID)
					continue;

				try
				{
					Message message = Message.Get(sender, channelName, messages[i]);
					if (message != null)
					{
						_log.Info(ELogGroup.Messages, "Received {0} message from {1}, channel {2}", message.GetType().FullName, message.Sender, message.Channel);

						if (MessageReceived != null)
						{
							MessageReceived(message);
						}
					}
				}
				catch (Exception exception)
				{
					_log.Exception(ELogGroup.Messages, exception);
				}
			}
		}

		void IChatClientListener.OnPrivateMessage(string sender, object chatMessage, string channelName)
		{
			if (sender == _userID)
				return;
			if (_connectCoroutine != null)
				return;

			try
			{
				Message message = Message.Get(sender, channelName, chatMessage);
				if (message != null)
				{
					_log.Info(ELogGroup.Messages, "Received {0} message from {1}, channel {2}", message.GetType().FullName, message.Sender, message.Channel);

					if (MessageReceived != null)
					{
						MessageReceived(message);
					}
				}
			}
			catch (Exception exception)
			{
				_log.Exception(ELogGroup.Messages, exception);
			}
		}

		void IChatClientListener.OnSubscribed(string[] channels, bool[] results)
		{
			for (int i = 0; i < channels.Length; ++i)
			{
				_log.Info(ELogGroup.Messages, "Subscribe to channel {0}. Result: {1}", channels[i], results[i]);

				ChannelSubscribed.SafeInvoke(channels[i], results[i]);
			}
		}

		void IChatClientListener.OnUnsubscribed(string[] channels)
		{
			for (int i = 0; i < channels.Length; ++i)
			{
				_log.Info(ELogGroup.Messages, "Unubscribe from channel {0}", channels[i]);

				ChannelUnsubscribed.SafeInvoke(channels[i]);
			}
		}

		void IChatClientListener.OnStatusUpdate(string user, int status, bool gotMessage, object message)
		{
		}

		void IChatClientListener.OnUserSubscribed(string channel, string user)
		{
			_log.Info(ELogGroup.Messages, "User {0} subscribed to channel {1}", user, channel);

			UserSubscribed.SafeInvoke(channel, user);
		}

		void IChatClientListener.OnUserUnsubscribed(string channel, string user)
		{
			_log.Info(ELogGroup.Messages, "User {0} unsubscribed from channel {1}", user, channel);

			UserUnsubscribed.SafeInvoke(channel, user);
		}

		void IChatClientListener.OnPropertiesChanged(string channel, Dictionary<object, object> properties)
		{
			ChannelPropertiesChanged.SafeInvoke(channel, properties);
		}

		//========== PRIVATE METHODS ==================================================================================

		private void Stop()
		{
			if (this != null && _connectCoroutine != null)
			{
				StopCoroutine(_connectCoroutine);
			}

			_connectCoroutine = null;
		}

		private bool SendAcksOnly()
		{
			if (_client != null)
			{
				_client.chatPeer.SendAcksOnly();
			}

			return _isPaused;
		}

		private IEnumerator ReconnectCoroutine()
		{
			// 1. Disconnect existing connection

			if (_client != null && _client.State != ChatState.Uninitialized && _client.State != ChatState.Disconnected)
			{
				_log.Warning(ELogGroup.Messages, "Disconnecting from Chat");

				_client.Disconnect();
				_client.chatPeer.SendOutgoingCommands();

				float disconnectTimeout = Time.realtimeSinceStartup + TIMEOUT_DISCONNECT;
				while (_client != null && _client.State != ChatState.Disconnected && Time.realtimeSinceStartup < disconnectTimeout)
				{
					yield return null;
				}

				if (_client.State != ChatState.Disconnected)
				{
					_log.Error(ELogGroup.Messages, "Failed to disconnect from Chat!");
				}
			}

			DisposeClient();

			// 2. Try to connect with UDP

			ConnectionProtocol connectionProtocol = ConnectionProtocol.Udp;

			CreateClient(connectionProtocol);

			_log.Warning(ELogGroup.Messages, "Connecting to Chat. AppID: {0}, Version: {1}, Region: {2}, UserID: {3}, Protocol: {4}", _appID, _version, _client.ChatRegion, _userID, connectionProtocol);

			if (_client.Connect(_appID, _version, new AuthenticationValues(_userID)) == true)
			{
				float connectTimeout = Time.realtimeSinceStartup + TIMEOUT_CONNECT_UDP;
				while (IsConnected == false && Time.realtimeSinceStartup < connectTimeout)
				{
					yield return null;
				}

				if (IsConnected == true)
				{
					_log.Warning(ELogGroup.Messages, "Connected to Chat. AppID: {0}, Version: {1}, Region: {2}, UserID: {3}, Protocol: {4}, State: {5}", _appID, _version, _client.ChatRegion, _userID, connectionProtocol, _client.State);

					// Connect coroutine extended to silently ignore messages from history after connect
					yield return new WaitForSecondsRealtime(1.0f);

					_connectCoroutine = null;
					yield break;
				}
				else
				{
					_log.Error(ELogGroup.Messages, "Failed to connect to Chat! AppID: {0}, Version: {1}, Region: {2}, UserID: {3}, Protocol: {4}, State: {5}", _appID, _version, _client.ChatRegion, _userID, connectionProtocol, _client.State);
				}
			}
			else
			{
				_log.Error(ELogGroup.Messages, "Failed to connect to Chat! AppID: {0}, Version: {1}, Region: {2}, UserID: {3}, Protocol: {4}", _appID, _version, _client.ChatRegion, _userID, connectionProtocol);
			}

			_client.Disconnect();
			_client.chatPeer.SendOutgoingCommands();

			DisposeClient();

			// 3. Try to connect with TCP

			connectionProtocol = ConnectionProtocol.Tcp;

			CreateClient(connectionProtocol);

			_log.Warning(ELogGroup.Messages, "Connecting to Chat. AppID: {0}, Version: {1}, Region: {2}, UserID: {3}, Protocol: {4}", _appID, _version, _client.ChatRegion, _userID, connectionProtocol);

			if (_client.Connect(_appID, _version, new AuthenticationValues(_userID)) == true)
			{
				float connectTimeout = Time.realtimeSinceStartup + TIMEOUT_CONNECT_TCP;
				while (IsConnected == false && Time.realtimeSinceStartup < connectTimeout)
				{
					yield return null;
				}

				if (IsConnected == true)
				{
					_log.Warning(ELogGroup.Messages, "Connected to Chat. AppID: {0}, Version: {1}, Region: {2}, UserID: {3}, Protocol: {4}, State: {5}", _appID, _version, _client.ChatRegion, _userID, connectionProtocol, _client.State);

					// Connect coroutine extended to silently ignore messages from history after connect
					yield return new WaitForSecondsRealtime(1.0f);

					_connectCoroutine = null;
					yield break;
				}
				else
				{
					_log.Error(ELogGroup.Messages, "Failed to connect to Chat! AppID: {0}, Version: {1}, Region: {2}, UserID: {3}, Protocol: {4}, State: {5}", _appID, _version, _client.ChatRegion, _userID, connectionProtocol, _client.State);
				}
			}
			else
			{
				_log.Error(ELogGroup.Messages, "Failed to connect to Chat! AppID: {0}, Version: {1}, Region: {2}, UserID: {3}, Protocol: {4}", _appID, _version, _client.ChatRegion, _userID, connectionProtocol);
			}

			_client.Disconnect();
			_client.chatPeer.SendOutgoingCommands();

			DisposeClient();

			_connectCoroutine = null;
		}

		private void CreateClient(ConnectionProtocol connectionProtocol)
		{
			_listener = new ChatListener(this);

			_client = new ChatClient(_listener);
			_client.TransportProtocol = connectionProtocol;

			if (_region.HasValue() == true)
			{
				_client.ChatRegion = _region;
			}

			_client.chatPeer.DebugOut = DebugLevel.ERROR;
		}

		private void DisposeClient()
		{
			if (_listener != null)
			{
				_listener.Dispose();
				_listener = null;
			}

			if (_client != null)
			{
				_client = null;
			}
		}

		private sealed class ChatListener : IChatClientListener
		{
			private IChatClientListener _listener;

			public ChatListener(IChatClientListener listener)
			{
				_listener = listener;
			}

			public void Dispose()
			{
				_listener = null;
			}

			void IChatClientListener.DebugReturn(DebugLevel level, string message)                              { if (_listener != null) { _listener.DebugReturn(level, message);                       } }
			void IChatClientListener.OnChatStateChange(ChatState state)                                         { if (_listener != null) { _listener.OnChatStateChange(state);                          } }
			void IChatClientListener.OnConnected()                                                              { if (_listener != null) { _listener.OnConnected();                                     } }
			void IChatClientListener.OnDisconnected()                                                           { if (_listener != null) { _listener.OnDisconnected();                                  } }
			void IChatClientListener.OnGetMessages(string channelName, string[] senders, object[] messages)     { if (_listener != null) { _listener.OnGetMessages(channelName, senders, messages);     } }
			void IChatClientListener.OnPrivateMessage(string sender, object message, string channelName)        { if (_listener != null) { _listener.OnPrivateMessage(sender, message, channelName);    } }
			void IChatClientListener.OnPropertiesChanged(string channel, Dictionary<object, object> properties) { if (_listener != null) { _listener.OnPropertiesChanged(channel, properties);          } }
			void IChatClientListener.OnStatusUpdate(string user, int status, bool gotMessage, object message)   { if (_listener != null) { _listener.OnStatusUpdate(user, status, gotMessage, message); } }
			void IChatClientListener.OnSubscribed(string[] channels, bool[] results)                            { if (_listener != null) { _listener.OnSubscribed(channels, results);                   } }
			void IChatClientListener.OnUnsubscribed(string[] channels)                                          { if (_listener != null) { _listener.OnUnsubscribed(channels);                          } }
			void IChatClientListener.OnUserSubscribed(string channel, string user)                              { if (_listener != null) { _listener.OnUserSubscribed(channel, user);                   } }
			void IChatClientListener.OnUserUnsubscribed(string channel, string user)                            { if (_listener != null) { _listener.OnUserUnsubscribed(channel, user);                 } }
		}
	}
}
