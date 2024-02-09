using System;
using System.Collections.Generic;
using Photon.Realtime;

namespace TowerRush
{
	using TowerRush.Core;
	using UnityEngine;

	public class MainScene : Scene, IConnectionCallbacks
	{
		[SerializeField] private StartMatch _startMatch;
		
		protected override void OnInitialize()
		{
			var userID = PlayerPrefs.GetString("UserID", "");
			if (userID.IsNullOrEmpty() == true)
			{
				userID = System.Guid.NewGuid().ToString().ToLowerInvariant();
				PlayerPrefs.SetString("UserID", userID);
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
			
			Debug.Log("Onconnected12312312");
			//_log.Info(ELogGroup.Network, "OnConnected");
		}

		
		void IConnectionCallbacks.OnConnectedToMaster()
		{
			Debug.Log("Me he conectado al master");
			_startMatch.OnStartMatch();
			//_log.Info(ELogGroup.Network, "OnConnectedToMaster");
		}

		void IConnectionCallbacks.OnDisconnected(DisconnectCause cause)
		{
			//_log.Info(ELogGroup.Network, "OnDisconnected {0}", cause);
		}

		void IConnectionCallbacks.OnRegionListReceived(RegionHandler regionHandler)
		{
			//_log.Info(ELogGroup.Network, "OnRegionListReceived {0}", regionHandler.SummaryToCache);
		}

		void IConnectionCallbacks.OnCustomAuthenticationResponse(Dictionary<string, object> data)
		{
			//_log.Info(ELogGroup.Network, "OnCustomAuthenticationResponse {0}", data.ToStringFull());
		}

		void IConnectionCallbacks.OnCustomAuthenticationFailed(string debugMessage)
		{
			//_log.Info(ELogGroup.Network, "OnCustomAuthenticationFailed {0}", debugMessage);
		}

		
		
		
		
		
	}
}