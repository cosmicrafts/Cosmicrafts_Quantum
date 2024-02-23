﻿using System.Collections.Generic;
using Candid;
using CanisterPK.CanisterStats.Models;
using EdjCase.ICP.Candid.Models;
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
		
		private GameMetrics gmt = new GameMetrics();
		
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
			
			gmt.InitMetrics();
			QuantumEvent.Subscribe<EventGameplayResult>(this, OnGameplayResult);
			QuantumEvent.Subscribe<EventUnitDestroyed>(this, OnUnitDestroyed);
			QuantumEvent.Subscribe<EventOnHealthChanged>(this, OnHealthChanged);
		}

		protected override void OnDeinitialize()
		{
			QuantumCallback.UnsubscribeListener<CallbackGameStarted>(this);
			QuantumEvent.UnsubscribeListener<EventGameplayStateChanged>(this);
			
			QuantumEvent.UnsubscribeListener<EventGameplayResult>(this);
			QuantumEvent.UnsubscribeListener<EventUnitDestroyed>(this);
			QuantumEvent.UnsubscribeListener<EventOnHealthChanged>(this);
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
		
		private void OnUnitDestroyed(EventUnitDestroyed e)
		{
			Debug.LogWarning("Se destruyo una nave" + e.killer.ToString());
			// Aquí puedes rastrear qué tropa destruyó a qué otra tropa
			// e.Unit es la entidad destruida
			// e.Destroyer es la entidad que causó la destrucción

			// Por ejemplo, podrías incrementar un contador o actualizar una estadística
			// que rastree las unidades destruidas por cada tipo de tropa
		}
		private void OnGameplayResult(EventGameplayResult gameplayResult)
		{
			bool isWin;
			if (gameplayResult.Winner < 0) { Debug.Log("Draw"); isWin = true; }
			else if (gameplayResult.Winner == Entities.LocalPlayer) { isWin = true; }
			else { isWin = false; }
			SendStats(isWin);
		}
		private void OnHealthChanged(EventOnHealthChanged e)
		{
			
			if (e.Data.HideToStats)
			{
				Debug.Log($"HideToStats: {e.Data.Value} a la entidad {e.Data.Target}");
				return;
			}
			
			if (e.Data.TargetOwner == Entities.LocalPlayerRef)
			{
				if (e.Data.Action == EHealthAction.Add)
				{
					Debug.Log($"Salud añadida: {e.Data.Value} a la entidad {e.Data.Target}");
				}
				else if (e.Data.Action == EHealthAction.Remove)
				{
					Debug.Log($"[Mi Nave] Salud removida: {e.Data.Value} de la entidad {e.Data.Target}");
					gmt.AddDamageReceived(e.Data.Value.AsFloat);
				}
			}
			else
			{
				if (e.Data.Action == EHealthAction.Add)
				{
					Debug.Log($"Salud añadida: {e.Data.Value} a la entidad {e.Data.Target}");
				}
				else if (e.Data.Action == EHealthAction.Remove)
				{
					Debug.Log($"[Otra Nave] Salud removida: {e.Data.Value} de la entidad {e.Data.Target}");
					gmt.AddDamage(e.Data.Value.AsFloat);
				}
			}
			
		}

		public async void SendStats(bool isWin)
		{
			BasicStats basicStats = new BasicStats();
			basicStats.EnergyUsed = gmt.GetEnergyUsed();
			basicStats.EnergyGenerated = gmt.GetEnergyGenerated();
			basicStats.EnergyWasted = gmt.GetEnergyWasted();
			basicStats.EnergyChargeRate = gmt.GetEnergyChargeRatePerSec();
			basicStats.XpEarned = gmt.GetScore();
			basicStats.DamageDealt = gmt.GetDamage();
			basicStats.DamageTaken = gmt.GetDamageReceived(); 
			basicStats.DamageCritic = gmt.GetDamageCritic();
			basicStats.DamageEvaded = gmt.GetDamageEvaded();
			basicStats.Kills = gmt.GetKills();
			basicStats.Deploys = gmt.GetDeploys();
			basicStats.SecRemaining = gmt.GetSecRemaining();
			basicStats.WonGame = isWin;
			basicStats.Faction = (UnboundedUInt) 0;
			basicStats.CharacterID = "0";
			basicStats.GameMode = (UnboundedUInt)0;
			basicStats.BotMode = (UnboundedUInt) 0;
			basicStats.BotDifficulty = (UnboundedUInt) 0;
	    
	    
			var statsSend = await CandidApiManager.Instance.CanisterStats.SaveFinishedGame(GlobalGameData.Instance.actualNumberRoom, basicStats);
			Debug.Log(statsSend);
		}



	}
}