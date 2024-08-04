using System;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using Cosmicrafts.Data;

namespace TowerRush
{
	using TowerRush.Core;

	public class MainScene : Scene, IConnectionCallbacks
	{
		[SerializeField] private StartMatch _startMatch;

		protected override async void OnInitialize()
		{
			// Load PlayerData asynchronously
			var playerData = await AsyncDataManager.LoadPlayerDataAsync();
			string userID = playerData?.PrincipalId;

			Debug.Log("My PrincipalId is: " + userID);
			if (string.IsNullOrEmpty(userID)) {
				userID = System.Guid.NewGuid().ToString().ToLowerInvariant();
			}

			Game.QuantumServices.Network.Connect(Configuration.NetworkAppID, Configuration.Version, null, userID);
			Game.QuantumServices.Network.Client.ConnectionCallbackTargets.Add(this);
		}

		protected override void OnDeinitialize()
		{
			Game.QuantumServices.Network.Client.ConnectionCallbackTargets.Remove(this);
		}

		void IConnectionCallbacks.OnConnected()
		{
			Debug.Log("OnConnected");
			// _log.Info(ELogGroup.Network, "OnConnected");
		}

		void IConnectionCallbacks.OnConnectedToMaster()
		{
			Debug.Log("Connected to master");
			_startMatch.OnStartMatch();
			// _log.Info(ELogGroup.Network, "OnConnectedToMaster");
		}

		void IConnectionCallbacks.OnDisconnected(DisconnectCause cause)
		{
			Debug.Log("OnDisconnected: " + cause);
			// _log.Info(ELogGroup.Network, "OnDisconnected {0}", cause);
		}

		void IConnectionCallbacks.OnRegionListReceived(RegionHandler regionHandler)
		{
			Debug.Log("OnRegionListReceived: " + regionHandler.SummaryToCache);
			// _log.Info(ELogGroup.Network, "OnRegionListReceived {0}", regionHandler.SummaryToCache);
		}

		void IConnectionCallbacks.OnCustomAuthenticationResponse(Dictionary<string, object> data)
		{
			Debug.Log("OnCustomAuthenticationResponse: " + data.ToStringFull());
			// _log.Info(ELogGroup.Network, "OnCustomAuthenticationResponse {0}", data.ToStringFull());
		}

		void IConnectionCallbacks.OnCustomAuthenticationFailed(string debugMessage)
		{
			Debug.Log("OnCustomAuthenticationFailed: " + debugMessage);
			// _log.Info(ELogGroup.Network, "OnCustomAuthenticationFailed {0}", debugMessage);
		}
	}
}
