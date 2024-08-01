using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using EdjCase.ICP.Candid.Models;
using CanisterPK.CanisterLogin.Models;
using CanisterPK.CanisterLogin;
using Candid;
using Quantum;
using TowerRush.Core;
using System.Collections;

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
                SendStats();
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
        }

        private void OnHealthChanged(EventOnHealthChanged e)
        {
            void InstanceCanvasDamage()
            {
                GameObject targetGameObject = GameObject.Find(e.Data.Target.ToString());
                if (targetGameObject != null)
                {
                    GameObject canvasDmg = Instantiate(CanvasDamage, targetGameObject.transform.position, targetGameObject.transform.rotation);
                    canvasDmg.GetComponent<CanvasDamage>().SetDamage(e.Data.Value.AsFloat, e.Data.AttackMode);
                }
            }

            if (e.Data.HideToStats)
            {
                Debug.Log($"HideToStats: {e.Data.Value} a la entidad {e.Data.Target}");
                return;
            }

            if (e.Data.TargetOwner == Entities.LocalPlayerRef && e.Data.Action == EHealthAction.Remove)
            {
                if (e.Data.Action == EHealthAction.Add)
                {
                    Debug.Log($"Salud añadida: {e.Data.Value} a la entidad {e.Data.Target}");
                }
                else if (e.Data.Action == EHealthAction.Remove)
                {
                    Debug.Log($"[Mi Nave] Salud removida: {e.Data.Value} de la entidad {e.Data.Target}");
                    InstanceCanvasDamage();
                    gmt.AddDamageReceived(e.Data.Value.AsFloat);
                    if (e.Data.AttackMode == EAttackMode.Evasion)
                    {
                        gmt.AddDamageEvaded(e.Data.ValueRefOriginal.AsFloat);
                    }
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
                    InstanceCanvasDamage();
                    gmt.AddDamage(e.Data.Value.AsFloat);
                    if (e.Data.AttackMode == EAttackMode.Critic)
                    {
                        gmt.AddDamageCritic(e.Data.ValueRefOriginal.AsFloat);
                    }
                }
            }
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
            // Create a new instance of PlayerStats and populate it
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

            // Create a new instance of BasicStats and populate it with the playerStats
            BasicStats basicStats = new BasicStats
            {
                PlayerStats = new List<PlayerStats> { playerStats }
            };

            // Create a new instance of SaveFinishedGameArg1 and populate it with the values from playerStats
            CanisterLoginApiClient.SaveFinishedGameArg1 saveFinishedGameArg1 = new CanisterLoginApiClient.SaveFinishedGameArg1
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
            

            // Display loading panel
            LoadingPanel.Instance.ActiveLoadingPanel();



            // Call the API to save the finished game
            var statsSend = await CandidApiManager.Instance.CanisterLogin.SaveFinishedGame(GlobalGameData.Instance.actualNumberRoom, saveFinishedGameArg1);
            Debug.Log("StatSend: " + statsSend.ReturnArg0);
            Debug.Log("Res: " + statsSend.ReturnArg1);

            LoadingPanel.Instance.DesactiveLoadingPanel();

            // Finish the scene
            FinishScene();
        }
    }
}
