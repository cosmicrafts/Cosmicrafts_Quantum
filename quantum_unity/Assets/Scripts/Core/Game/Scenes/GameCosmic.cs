using System.Collections.Generic;
using Quantum.Services;

namespace TowerRush
{
	using TowerRush.Core;
	using Quantum;
	using System.Collections;
	using UnityEngine;
	using Photon.Realtime;

	public class GameCosmic : Scene
	{
		// CONFIGURATION

		[SerializeField] Transform m_AlphaCameraPosition;
		[SerializeField] Transform m_BetaCameraPosition;
		[SerializeField] Light     m_AlphaLight;
		[SerializeField] Light     m_BetaLight;

		// PRIVATE MEMBERS

		private bool m_Started;

		// Scene INTERFACE

		protected override void OnInitialize()
		{
			QuantumCallback.Subscribe<CallbackGameStarted>(this, OnGameStarted);
			QuantumEvent.Subscribe<EventGameplayStateChanged>(this, OnGameplayStateChanged);
			UIMatchLoading.Instance.OnInitMatch();
		}

		protected override void OnDeinitialize()
		{
			QuantumCallback.UnsubscribeListener<CallbackGameStarted>(this);
			QuantumEvent.UnsubscribeListener<EventGameplayStateChanged>(this);
		}

		protected override void OnActivate()
		{
			if (Game.GameplayInfo != null)
			{
				StartCoroutine(Activate_Coroutine());
				
			}
			else if (QuantumRunner.Default == null)
			{
				StartCoroutine(ActivateOffline_Coroutine());
			}
			else
			{
				m_State = EState.Active;
			}
		}

		protected override void OnDeactivate()
		{
			QuantumRunner.ShutdownAll(true);
			Game.QuantumServices.Matchmaking.Leave();
			Game.QuantumServices.Network.Disconnect();

			base.OnDeactivate();
		}

		protected override void OnUpdate()
		{
			if (Entities.LocalPlayer == 0)
			{
				Game.Instance.MainCamera.transform.SetPositionAndRotation(m_AlphaCameraPosition.position, m_AlphaCameraPosition.rotation);
				m_AlphaLight.SetActive(true);
				m_BetaLight.SetActive(false);
			}
			else
			{
				Game.Instance.MainCamera.transform.SetPositionAndRotation(m_BetaCameraPosition.position, m_BetaCameraPosition.rotation);
				m_AlphaLight.SetActive(false);
				m_BetaLight.SetActive(true);
			}
		}

		protected override bool CanUpdateComponents(SceneContext context)
		{
			return context.Frame != null;
		}

		// PRIVATE METHODS

		private IEnumerator Activate_Coroutine()
		{
			var match = Game.QuantumServices.Matchmaking.CurrentMatch;

			while (match.HasStarted == false)
				yield return null;

			QuantumRunner.StartGame(Game.GameplayInfo.ClientID, Game.GameplayInfo.StartParams);

			while (m_Started == false)
				yield return null;

			m_State = EState.Active;
		}

		private IEnumerator ActivateOffline_Coroutine()
		{
			var debugRunner = GetComponentInChildren<QuantumRunnerLocalDebug>();
			if (debugRunner == null || debugRunner.enabled == true)
			{
				m_State = EState.Active;
				yield break;
			}

			debugRunner.enabled = true;

			while (m_Started == false)
				yield return null;

			m_State = EState.Active;
		}

		private void OnGameStarted(CallbackGameStarted started)
		{
			if (Game.GameplayInfo != null)
			{
				foreach (var player in started.Game.GetLocalPlayers())
				{
					started.Game.SendPlayerData(player, new RuntimePlayer()
					{
						Level  = Game.GameplayInfo.Level,
						Cards  = Game.GameplayInfo.Cards,
					});
				}
			}

			m_Started = true;
		}

		private void OnGameplayStateChanged(EventGameplayStateChanged stateChanged)
		{
			Debug.Log("ChageState: " + stateChanged.State);
			if (stateChanged.State == EGameplayState.Deactivate)
			{
				FinishScene();
			}
			
		}
	}
}