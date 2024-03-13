using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Quantum;
using Quantum.Services;
using TowerRush.Core;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using Photon.Deterministic;
using TowerRush;

public class StartMatch : MonoBehaviour
{
   // CONFIGURATION

		[SerializeField] MapAsset[]             m_Maps;
		[SerializeField] GameplaySettingsAsset  m_GameplaySettings;

		// PRIVATE MEMBERS

		private MenuCardInfo[]                  m_Cards;
		private DictionaryFile                  m_CardsSaveFile;
		
		
		[Header("Skills")]
		[Range(0, 1000)] public float HpPorcent = 100;
		[Range(0, 1000)] public float DmgPorcent = 100;

		
		private void Awake()
		{
			var mapNames = new List<string>(m_Maps.Length);
			for (int idx = 0, count = m_Maps.Length; idx < count; idx++)
			{
				mapNames.Add(m_Maps[idx].name);
			}

			LoadCardSelection();
		}

		
		
		// PRIVATE METHODS

		public void OnStartMatch()
		{
			var map = m_Maps[0];

			var config              = new RuntimeConfig();
			config.Map.Id           = map.Settings.Guid;
			config.GameplaySettings = new AssetRefGameplaySettings() { Id = m_GameplaySettings.AssetObject.Guid };
			config.Seed             = Random.Range(int.MinValue, int.MaxValue);

			var param = new QuantumRunner.StartParameters
			{
				RuntimeConfig       = config,
				DeterministicConfig = DeterministicSessionConfigAsset.Instance.Config,
				ReplayProvider      = null,
				GameMode            = Photon.Deterministic.DeterministicGameMode.Multiplayer,
				InitialFrame        = 0,
				PlayerCount         = 2,
				LocalPlayerCount    = 1,
				RecordingFlags      = RecordingFlags.None,
				NetworkClient       = Game.QuantumServices.Network.Client,
			};

			var clientId = Game.QuantumServices.Network.UserID;

			var  cards = m_Cards;
			byte level = 1;

#if ENABLE_CHEAT_MANAGER
			if (CheatManager.Instance.Cards.SafeLength() > 0)
			{
				cards = CheatManager.Instance.Cards.Select(obj => new MenuCardInfo { CardSettings = obj.CardSettings, Level = obj.Level, InDeck = true }).ToArray();
			}

			if (CheatManager.Instance.Level > 0)
			{
				level = CheatManager.Instance.Level;
			}
#endif

			Game.GameplayInfo = new GameplayInfo()
			{ 
				ClientID       = clientId,
				StartParams    = param,
				SceneName      = map.Settings.Scene,
				Level          = level,
				Cards          = m_Cards
									.Where(obj => obj.InDeck == true)
									.Select(obj => new CardInfo 
									{
										CardSettings = obj.CardSettings,
										Level        = obj.Level,
										BaseHealth   = FP.FromFloat_UNSAFE(obj.Hp * (HpPorcent * 0.01f) ),
										Damage       = FP.FromFloat_UNSAFE(obj.Dmg * (DmgPorcent * 0.01f)),
									}).ToArray(),
			};

			
			MatchRequest matchRequest = new MatchRequest();
			matchRequest.Room                 = GlobalGameData.Instance.actualRoom;
			matchRequest.Type                 = EMatchRequestType.JoinOrCreate;
			matchRequest.IsOpen               = true;
			matchRequest.IsVisible            = true;
			matchRequest.AutoStart            = true;
			matchRequest.ExpectedPlayers      = 2;
			matchRequest.FillTimeout          = 1;
			matchRequest.Plugin               = Configuration.QuantumPlugin;
			matchRequest.Config.RuntimeConfig = config;

			var match = Game.QuantumServices.Matchmaking.Run(matchRequest);
			match.Connected    += OnConnectedToMatch;

		}
		
		private void OnConnectedToMatch(Match match)
		{
			Game.CurrentScene.FinishScene();
			match.Connected -= OnConnectedToMatch;
		}

		private void SaveCardSelection()
		{
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				var count = m_Cards.Length;
				writer.Write(count);

				for (int idx = 0; idx < count; idx++)
				{
					var card = m_Cards[idx];
					writer.Write(card.CardSettings.Id.Value);
					writer.Write(card.Level);
					writer.Write(card.InDeck);
					writer.Write(card.Hp);
					writer.Write(card.Dmg);
				}


				writer.Flush();

				var data       = stream.ToArray();
				var stringData = System.Convert.ToBase64String(data);

				m_CardsSaveFile.SetString("Selection", stringData);
				m_CardsSaveFile.Save();
			}
		}

		private void LoadCardSelection()
		{
			if (m_CardsSaveFile == null)
			{
				m_CardsSaveFile = DictionaryFile.Load("Cards");
			}

			var stringData = m_CardsSaveFile.GetString("Selection", "");
			if (stringData.Length == 0)
			{
				var allCards = m_GameplaySettings.Settings.AllCards;
				var count    = allCards.Length;
				m_Cards      = new MenuCardInfo[count];

				for (int idx = 0; idx < count; idx++)
				{
					m_Cards[idx] = new MenuCardInfo
					{
						CardSettings = allCards[idx],
						Level        = 1,
						InDeck       = true,
						Hp           = 100f,
						Dmg          = 20f,
					};
				}

				SaveCardSelection();
				return;
			}

			var data = System.Convert.FromBase64String(stringData);

			using (var stream = new MemoryStream(data))
			using (var reader = new BinaryReader(stream))
			{
				var count = reader.ReadInt32();
				m_Cards = new MenuCardInfo[count];

				for (int idx = 0; idx < count; idx++)
				{
					var id     = new AssetGuid(reader.ReadInt64());
					var level  = reader.ReadByte();
					var inDeck = reader.ReadBoolean();
					var hp = reader.ReadSingle();
					var dmg = reader.ReadSingle();

					if (UnityDB.FindAsset(id) != null)
					{
						m_Cards[idx] = new MenuCardInfo
						{
							CardSettings = new AssetRefCardSettings { Id = id },
							Level        = level,
							InDeck       = inDeck,
							Hp           = hp, 
							Dmg          = dmg, 
						};
					}
				}
			}
		}
    
    
}
