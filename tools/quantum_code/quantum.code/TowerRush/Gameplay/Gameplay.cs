namespace Quantum
{
	using Photon.Deterministic;
	using Quantum.Collections;

	public unsafe partial struct Gameplay
	{
		// PUBLIC MEMBERS

		public bool IsActive { get { return State == EGameplayState.Match || State == EGameplayState.Overtime; } }

		// PUBLIC METHODS

		public void Initialize(Frame frame)
		{
			SetState(frame, EGameplayState.WaitingForPlayers);
		}

		public void Update(Frame frame)
		{
			StateTime -= frame.DeltaTime;

			switch (State)
			{
				case EGameplayState.WaitingForPlayers: UpdateState_WaitingForPlayers(frame); break;
				case EGameplayState.Warmup:            UpdateState_Warmup(frame);            break;
				case EGameplayState.Match:             UpdateState_Match(frame);             break;
				case EGameplayState.Overtime:          UpdateState_Overtime(frame);          break;
				case EGameplayState.MatchEnd:          UpdateState_MatchEnd(frame);          break;
				case EGameplayState.Deactivate:        break;

				default:
					Log.Error($"Missing implementation of state update: {State}");
					break;
			}
		}

		public void PlayerDataSet(Frame frame, PlayerRef playerRef)
		{
			var playerData   = frame.GetPlayerData(playerRef);
			var playerEntity = GetPlayerEntity(frame, playerRef);

			frame.AddOrGet<Player>(playerEntity, out var player);
			frame.AddOrGet<CardManager>(playerEntity, out var cardManager);

			var gameplaySettings = frame.FindAsset<GameplaySettings>(frame.RuntimeConfig.GameplaySettings.Id);
			var castleSettings   = frame.FindAsset<UnitSettings>(gameplaySettings.CastleSettings.Id);
			var towerSettings    = frame.FindAsset<UnitSettings>(gameplaySettings.TowerSettings.Id);

			foreach (var card in playerData.Cards)
			{
				Log.Debug("D: " + card.Damage +" L: " + card.Level +" HP: " + card.BaseHealth );
				Log.Debug("Token: " + card.TokenID );
			}

			player->PlayerRef = playerRef;
			cardManager->Initialize(frame, playerData.Cards, gameplaySettings);

			if (playerRef == 0)
			{
				SetBuilding(frame,  playerRef, AlphaCastle, castleSettings, playerData.Level);
				SetBuildings(frame, playerRef, AlphaTowers, towerSettings,  playerData.Level);

				BetaArea.RefreshArea(frame);
			}
			else if (playerRef == 1)
			{
				SetBuilding(frame,  playerRef, BetaCastle, castleSettings, playerData.Level);
				SetBuildings(frame, playerRef, BetaTowers, towerSettings,  playerData.Level);
			
				AlphaArea.RefreshArea(frame);
			}
		}

		public void OnDeath(Frame frame, EntityRef entity, EntityRef killer)
		{
			if (frame.IsVerified == false)
				return;

			if (entity == AlphaCastle)
			{
				DestroyTowers(frame, AlphaTowers);
				AddScore(frame, 1);

				BetaArea.RefreshArea(frame);
			}
			else if (entity == BetaCastle)
			{
				DestroyTowers(frame, BetaTowers);
				AddScore(frame, 0);

				AlphaArea.RefreshArea(frame);
			}
			else
			{
				if (CheckTower(frame, entity, BetaTowers,  0))
				{
					AlphaArea.RefreshArea(frame);
				}

				if (CheckTower(frame, entity, AlphaTowers, 1))
				{
					BetaArea.RefreshArea(frame);
				}
			}

			PlayerRef owner = default;
			int unitTokenID = default;
			if (frame.Unsafe.TryGetPointer<Unit>(entity, out var unit))
			{
				owner = unit-> Owner;
				unitTokenID = unit-> TokenID;
			}
			
			int killerTokenID = default;
			if (frame.Unsafe.TryGetPointer<Unit>(killer, out var unitKiller))
			{
				killerTokenID = unitKiller-> TokenID;
			}
			
			frame.Events.UnitDestroyed(owner, entity, unitTokenID, killer, killerTokenID);
			
		}

		public bool IsValidUnitPosition(Frame frame, PlayerRef playerRef, FPVector2 position)
		{
			if (playerRef == 0)
			{
				return AlphaArea.IsValidUnitPosition(frame, position);
			}
			else if (playerRef == 1)
			{
				return BetaArea.IsValidUnitPosition(frame, position);
			}

			return false;
		}

		// PRIVATE METHODS

		private EntityRef GetPlayerEntity(Frame frame, PlayerRef playerRef)
		{
			foreach (var pair in frame.Unsafe.GetComponentBlockIterator<Player>())
			{
				if (pair.Component->PlayerRef == playerRef)
					return pair.Entity;
			}

			return frame.Create();
		}

		private void UpdateState_WaitingForPlayers(Frame frame)
		{
			var playerCount = 0;

			foreach (var _ in frame.Unsafe.GetComponentBlockIterator<Player>())
			{
				playerCount += 1;
			}

			if (playerCount == 2)
			{
				SetState(frame, EGameplayState.Warmup);
			}
		}

		private void UpdateState_Warmup(Frame frame)
		{
			if (StateTime <= FP._0)
			{
				SetState(frame, EGameplayState.Match);
			}
		}

		private void UpdateState_Match(Frame frame)
		{
			if (AlphaScore == 3 && BetaScore == 3)
			{
				GameplayEnd(frame, -1);
				return;
			}
			else if (AlphaScore == 3)
			{
				GameplayEnd(frame, 0);
				return;
			}
			else if (BetaScore == 3)
			{
				GameplayEnd(frame, 1);
				return;
			}

			if (StateTime > FP._0)
				return;

			if (AlphaScore > BetaScore)
			{
				GameplayEnd(frame, 0);
			}
			else if (BetaScore > AlphaScore)
			{
				GameplayEnd(frame, 1);
			}
			else
			{
				SetState(frame, EGameplayState.Overtime);
			}
		}

		private void UpdateState_Overtime(Frame frame)
		{
			if (AlphaScore > BetaScore)
			{
				GameplayEnd(frame, 0);
			}
			else if (BetaScore > AlphaScore)
			{
				GameplayEnd(frame, 1);
			}
			else if (StateTime <= FP._0)
			{
				GameplayEnd(frame, -1);
			}
		}

		private void UpdateState_MatchEnd(Frame frame)
		{
			if (StateTime > FP._0)
				return;

			SetState(frame, EGameplayState.Deactivate);
		}

		private void SetBuildings(Frame frame, PlayerRef owner, QListPtr<EntityRef> entities, UnitSettings settings, byte level)
		{
			var buildings = frame.ResolveList(entities);

			for (int idx = 0; idx < buildings.Count; idx++)
			{
				SetBuilding(frame, owner, buildings[idx], settings, level);
			}
		}

		private void SetBuilding(Frame frame, PlayerRef owner, EntityRef entity, UnitSettings settings, byte level)
		{
			frame.Unsafe.GetPointer<Unit>(entity)->Initialize(frame, owner, entity, settings, level, 0);
		}

		private bool CheckTower(Frame frame, EntityRef unitRef, QListPtr<EntityRef> towersPtr, PlayerRef killerPlayer)
		{
			var towers = frame.ResolveList(towersPtr);

			for (int idx = 0; idx < towers.Count; idx++)
			{
				var tower = towers[idx];
				if (tower != unitRef)
					continue;

				AddScore(frame, killerPlayer);
				return true;
			}

			return false;
		}

		private void DestroyTowers(Frame frame, QListPtr<EntityRef> towersPtr)
		{
			var towers = frame.ResolveList(towersPtr);

			for (int idx = 0; idx < towers.Count; idx++)
			{
				var tower  = towers[idx];
				var health = frame.Unsafe.GetPointer<Health>(tower);
				if (health->IsAlive == true)
				{
					health->ForceKill(frame, tower);
				}
			}
		}

		private void AddScore(Frame frame, PlayerRef playerRef)
		{
			if (playerRef == 0)
			{
				AlphaScore += 1;

				frame.Events.ScoreGained(playerRef, AlphaScore, 1);
			}
			else if (playerRef == 1)
			{
				BetaScore += 1;

				frame.Events.ScoreGained(playerRef, BetaScore, 1);
			}
		}

		private void GameplayEnd(Frame frame, int winner)
		{
			SetState(frame, EGameplayState.MatchEnd);
			frame.Events.GameplayResult(winner);

			frame.SystemDisable<UnitAISystem>();
			frame.SystemDisable<MovementSystem>();
			frame.SystemDisable<WeaponSystem>();
			frame.SystemDisable<Core.NavigationSystem>();
		}

		private void SetState(Frame frame, EGameplayState state)
		{
			State = state;

			var settings      = frame.FindAsset<GameplaySettings>(frame.RuntimeConfig.GameplaySettings.Id);
			var stateSettings = settings.States.Find(obj => obj.State == state);

			if (stateSettings != null)
			{
				StateTime = stateSettings.DurationSeconds;

				foreach (var pair in frame.Unsafe.GetComponentBlockIterator<CardManager>())
				{
					pair.Component->SetFillRate(stateSettings.EnergyFillRate);
				}
			}
			else
			{
				Log.Error($"Missing StateSettings for state {state}");
			}

			frame.Events.GameplayStateChanged(state);
		}
	}
}
