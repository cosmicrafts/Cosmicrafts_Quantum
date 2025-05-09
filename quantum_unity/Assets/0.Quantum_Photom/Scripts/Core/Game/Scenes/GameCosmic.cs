﻿using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.MainCanister.Models;
using Cosmicrafts.MainCanister;
using Candid;
using Quantum;
using TowerRush.Core;
using System.Collections;
using Cosmicrafts.Managers;

namespace TowerRush
{
    public class GameCosmic : Scene
    {
        private GameMetrics gmt = new GameMetrics();

        // CONFIGURATION
        [SerializeField] Transform m_AlphaCameraPosition;
        [SerializeField] Transform m_BetaCameraPosition;
        [SerializeField] Light m_AlphaLight;
        [SerializeField] Light m_BetaLight;
        [SerializeField] private GameObject CanvasDamage;
        [SerializeField] private GameObject UILoadingResults;
        [SerializeField] private GameObject UIResults;


        private Dictionary<string, GameObject> targetCache = new Dictionary<string, GameObject>();
        private List<EventOnHealthChanged> healthChangeEvents = new List<EventOnHealthChanged>();
        private const float updateInterval = 0.25f;
        private float timeSinceLastUpdate = 0f;


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
            QuantumEvent.Subscribe<EventCardSpawned>(this, OnSpawnedCard);
            ObjectPoolManager.Instance.CreatePool(CanvasDamage, 10);
        }

        protected override void OnDeinitialize()
        {
            QuantumCallback.UnsubscribeListener<CallbackGameStarted>(this);
            QuantumEvent.UnsubscribeListener<EventGameplayStateChanged>(this);
            QuantumEvent.UnsubscribeListener<EventGameplayResult>(this);
            QuantumEvent.UnsubscribeListener<EventUnitDestroyed>(this);
            QuantumEvent.UnsubscribeListener<EventOnHealthChanged>(this);
            QuantumEvent.UnsubscribeListener<EventCardSpawned>(this);
        }

        protected override void OnActivate()
        {
            Game.Instance.AudioService.ChangeMusicClip("gameplay");

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

            timeSinceLastUpdate += Time.deltaTime;
            if (timeSinceLastUpdate >= updateInterval)
            {
                ProcessHealthChanges();
                timeSinceLastUpdate = 0f;
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
                        Level = Game.GameplayInfo.Level,
                        Cards = Game.GameplayInfo.Cards,
                    });

                    foreach (var card in Game.GameplayInfo.Cards)
                    {
                        Debug.Log(card.CardSettings.Id);
                        Debug.Log(UnityDB.FindAsset<CardSettingsAsset>(card.CardSettings.Id).DisplayName);
                    }
                    Debug.Log("----------------------------------------");
                }
            }

            m_Started = true;
        }

        private void OnGameplayStateChanged(EventGameplayStateChanged stateChanged)
        {
            Debug.Log("ChangeState: " + stateChanged.State);
            if (stateChanged.State == EGameplayState.Deactivate)
            {
            }
        }

        private void OnUnitDestroyed(EventUnitDestroyed e)
        {
            if (e.Owner == Entities.LocalPlayerRef)
            {
                Debug.LogWarning("Destruyeron Mi Nave: " + e.UnitEntity.ToString() + "Killer: " + e.killer.ToString());
            }
            else
            {
                Debug.LogWarning("Destruí una Nave: " + e.killer.ToString() + "ship: " + e.UnitEntity.ToString());
                gmt.AddKills(1);
            }
        }

        private void OnGameplayResult(EventGameplayResult gameplayResult)
        {
            bool isWin;
            if (gameplayResult.Winner < 0)
            {
                Debug.Log("Draw");
                isWin = true;
            }
            else if (gameplayResult.Winner == Entities.LocalPlayer)
            {
                isWin = true;
            }
            else
            {
                isWin = false;
            }

            gmt.SetIsWin(isWin);
            SendStats();
        }

        private void OnHealthChanged(EventOnHealthChanged e)
        {
            healthChangeEvents.Add(e);
        }

        private void ProcessHealthChanges()
        {
            foreach (var e in healthChangeEvents)
            {
                GameObject targetGameObject;
                if (!targetCache.TryGetValue(e.Data.Target.ToString(), out targetGameObject))
                {
                    targetGameObject = GameObject.Find(e.Data.Target.ToString());
                    if (targetGameObject != null)
                    {
                        targetCache[e.Data.Target.ToString()] = targetGameObject;
                    }
                    else
                    {
                        Debug.LogError("Target game object not found.");
                        continue;
                    }
                }

                if (ObjectPoolManager.Instance != null)
                {
                    GameObject canvasDmg = ObjectPoolManager.Instance.GetObject(CanvasDamage, targetGameObject.transform, targetGameObject.transform.position, targetGameObject.transform.rotation);
                    CanvasDamage canvasDamage = canvasDmg.GetComponent<CanvasDamage>();
                    if (canvasDamage != null)
                    {
                        canvasDamage.SetDamage(e.Data.Value.AsFloat, e.Data.AttackMode);
                    }
                    else
                    {
                        Debug.LogError("CanvasDamage component not found on the instantiated object.");
                    }
                }
                else
                {
                    Debug.LogError("ObjectPoolManager instance is not initialized.");
                }
            }

            healthChangeEvents.Clear();
        }




        private void OnSpawnedCard(EventCardSpawned e)
        {
            if (e.Owner == Entities.LocalPlayerRef)
            {
                CardSettingsAsset card = UnityDB.FindAsset<CardSettingsAsset>(e.assetRefCardSettings.Id);
                Debug.Log(e.assetRefCardSettings.Id);
                Debug.Log("CardSpawned I am Owner: " + card.DisplayName + " : " + e.CardTokenID);
                Debug.Log(card.GetEnergyCost());
                gmt.AddDeploys(1);
                gmt.AddEnergyUsed(card.GetEnergyCost());
            }
            else
            {
                Debug.Log("CardSpawned Not Owner" + e.CardTokenID);
            }
        }

public async void SendStats()
{
    // Collect player stats
    PlayerStats playerStats = new PlayerStats
    {
        EnergyUsed = UnboundedUInt.FromBigInteger(new BigInteger((int)gmt.GetEnergyUsed())),
        EnergyGenerated = UnboundedUInt.FromBigInteger(new BigInteger((int)gmt.GetEnergyGenerated())),
        EnergyWasted = UnboundedUInt.FromBigInteger(new BigInteger((int)gmt.GetEnergyWasted())),
        EnergyChargeRate = UnboundedUInt.FromBigInteger(new BigInteger((int)gmt.GetEnergyChargeRatePerSec())),
        XpEarned = UnboundedUInt.FromBigInteger(new BigInteger((int)gmt.GetScore())),
        DamageDealt = UnboundedUInt.FromBigInteger(new BigInteger((int)gmt.GetDamage())),
        DamageTaken = UnboundedUInt.FromBigInteger(new BigInteger((int)gmt.GetDamageReceived())),
        DamageCritic = UnboundedUInt.FromBigInteger(new BigInteger((int)gmt.GetDamageCritic())),
        DamageEvaded = UnboundedUInt.FromBigInteger(new BigInteger((int)gmt.GetDamageEvaded())),
        Kills = UnboundedUInt.FromBigInteger(new BigInteger(gmt.GetKills())),
        Deploys = UnboundedUInt.FromBigInteger(new BigInteger(gmt.GetDeploys())),
        SecRemaining = UnboundedUInt.FromBigInteger(new BigInteger(gmt.GetSecRemaining())),
        WonGame = gmt.GetIsWin(),
        Faction = UnboundedUInt.FromBigInteger(BigInteger.Zero),
        CharacterID = UnboundedUInt.FromBigInteger(BigInteger.Parse("0")),
        GameMode = UnboundedUInt.FromBigInteger(BigInteger.Zero),
        BotMode = UnboundedUInt.FromBigInteger(BigInteger.Zero),
        BotDifficulty = UnboundedUInt.FromBigInteger(BigInteger.Zero)
    };

    // Create basic stats
    BasicStats basicStats = new BasicStats
    {
        PlayerStats = new List<PlayerStats> { playerStats }
    };

    // Prepare save finished game argument
    MainCanisterApiClient.SaveFinishedGameArg1 saveFinishedGameArg1 = new MainCanisterApiClient.SaveFinishedGameArg1
    {
        BotDifficulty = playerStats.BotDifficulty,
        BotMode = playerStats.BotMode,
        CharacterID = playerStats.CharacterID,
        DamageCritic = playerStats.DamageCritic,
        DamageDealt = playerStats.DamageDealt,
        DamageEvaded = playerStats.DamageEvaded,
        DamageTaken = playerStats.DamageTaken,
        Deploys = playerStats.Deploys,
        EnergyChargeRate = playerStats.EnergyChargeRate,
        EnergyGenerated = playerStats.EnergyGenerated,
        EnergyUsed = playerStats.EnergyUsed,
        EnergyWasted = playerStats.EnergyWasted,
        Faction = playerStats.Faction,
        GameMode = playerStats.GameMode,
        Kills = playerStats.Kills,
        SecRemaining = playerStats.SecRemaining,
        WonGame = playerStats.WonGame,
        XpEarned = playerStats.XpEarned
    };

    // Display loading panel and call API
    LoadingPanel.Instance.ActiveLoadingPanel();

    var statsSend = await CandidApiManager.Instance.MainCanister.SaveFinishedGame(GameDataManager.Instance.playerData.actualNumberRoom, saveFinishedGameArg1);
    Debug.Log("StatSend: " + statsSend.ReturnArg0);
    Debug.Log("Res: " + statsSend.ReturnArg1);

    LoadingPanel.Instance.DesactiveLoadingPanel();

    // Display results UI and wait for button press
    UIResults.SetActive(true);
        FinishScene();
}


    }
}
