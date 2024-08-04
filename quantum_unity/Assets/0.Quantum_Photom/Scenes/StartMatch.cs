using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using Quantum;
using Quantum.Services;
using TowerRush.Core;
using UnityEngine.UI;
using System.IO;
using Photon.Deterministic;
using TowerRush;
using Cosmicrafts.Data;

public class StartMatch : MonoBehaviour
{
    // CONFIGURATION
    [SerializeField] MapAsset[] m_Maps;
    [SerializeField] GameplaySettingsAsset m_GameplaySettings;

    // PRIVATE MEMBERS
    private NFTCollection.SavedKeys savedKeys = new NFTCollection.SavedKeys();
    private PlayerData playerData;

    [Header("Skills")]
    [Range(0, 1000)] public float HpPercent = 100;
    [Range(0, 1000)] public float DmgPercent = 100;

    private async void Awake()
    {
        playerData = await AsyncDataManager.LoadPlayerDataAsync();

        if (playerData != null)
        {
            savedKeys.listSavedKeys = playerData.DeckNFTsKeyIds;
        }

        var mapNames = new List<string>(m_Maps.Length);
        for (int idx = 0, count = m_Maps.Length; idx < count; idx++)
        {
            mapNames.Add(m_Maps[idx].name);
        }
    }

    // PRIVATE METHODS
    public void OnStartMatch()
    {
        var map = m_Maps[0];

        var config = new RuntimeConfig();
        config.Map.Id = map.Settings.Guid;
        config.GameplaySettings = new AssetRefGameplaySettings() { Id = m_GameplaySettings.AssetObject.Guid };
        config.Seed = Random.Range(int.MinValue, int.MaxValue);

        var param = new QuantumRunner.StartParameters
        {
            RuntimeConfig = config,
            DeterministicConfig = DeterministicSessionConfigAsset.Instance.Config,
            ReplayProvider = null,
            GameMode = Photon.Deterministic.DeterministicGameMode.Multiplayer,
            InitialFrame = 0,
            PlayerCount = 2,
            LocalPlayerCount = 1,
            RecordingFlags = RecordingFlags.None,
            NetworkClient = Game.QuantumServices.Network.Client,
        };

        var clientId = Game.QuantumServices.Network.UserID;

        byte level = 1;

#if ENABLE_CHEAT_MANAGER
        if (CheatManager.Instance.Level > 0)
        {
            level = CheatManager.Instance.Level;
        }
#endif

        Game.GameplayInfo = new GameplayInfo()
        {
            ClientID = clientId,
            StartParams = param,
            SceneName = map.Settings.Scene,
            Level = level,
            Cards = NFTManager.Instance.AllNFTDatas
                    .Where(nft => savedKeys.listSavedKeys.Contains(nft.TokenId))
                    .Select(nftData => new CardInfo
                    {
                        CardSettings = m_GameplaySettings.Settings.AllCards[nftData.General.FirstOrDefault().UnitId],
                        Level = (byte)nftData.BasicStats.FirstOrDefault(s => s.StatName.ToLower() == "level").StatValue,
                        BaseHealth = FP.FromFloat_UNSAFE(
                            nftData.BasicStats.FirstOrDefault(s => s.StatName.ToLower() == "health").StatValue * (HpPercent * 0.01f)),
                        Damage = FP.FromFloat_UNSAFE(
                            nftData.BasicStats.FirstOrDefault(s => s.StatName.ToLower() == "damage").StatValue * (DmgPercent * 0.01f)),
                        TokenID = int.Parse(nftData.TokenId),
                    }).ToArray(),
        };

        MatchRequest matchRequest = new MatchRequest();
        matchRequest.Room = playerData != null ? playerData.config.CurrentMatch.ToString() : "DefaultRoom";
        matchRequest.Type = EMatchRequestType.JoinOrCreate;
        matchRequest.IsOpen = true;
        matchRequest.IsVisible = true;
        matchRequest.AutoStart = true;
        matchRequest.ExpectedPlayers = 2;
        matchRequest.FillTimeout = 1;
        matchRequest.Plugin = Configuration.QuantumPlugin;
        matchRequest.Config.RuntimeConfig = config;

        var match = Game.QuantumServices.Matchmaking.Run(matchRequest);
        match.Connected += OnConnectedToMatch;
    }

    private void OnConnectedToMatch(Match match)
    {
        Game.CurrentScene.FinishScene();
        match.Connected -= OnConnectedToMatch;
    }
}
