namespace Quantum
{
	using Photon.Deterministic;

	unsafe partial struct CardManager
	{
		// CONSTANTS

		public const byte AVAILABLE_CARDS_COUNT = 4;
	
		// PUBLIC METHODS

		public void Initialize(Frame frame, CardInfo[] cards, GameplaySettings settings)
		{
			var availableCards = frame.AllocateList<CardInfo>(AVAILABLE_CARDS_COUNT);
			var cardQueue      = frame.AllocateList<CardInfo>(cards.Length);

			CardQueue      = cardQueue;
			AvailableCards = availableCards;

			for (int idx = 0, count = cards.Length; idx < count; idx++)
			{
				cardQueue.Add(cards[idx]);
			}

			cardQueue.Shuffle(frame.RNG);

			for (int idx = 0; idx < AVAILABLE_CARDS_COUNT; idx++)
			{
				availableCards.Add(cardQueue[idx]);
			}

			QueueHeadIndex = AVAILABLE_CARDS_COUNT;

			CurrentEnergy = (int)settings.StartEnergy;
			MaxEnergy     = (int)settings.MaxEnergy;
		}

		public void SetFillRate(FP energyFillRate)
		{
			EnergyFillRate = energyFillRate;
		}

		public void Deinitialize(Frame frame)
		{
			frame.FreeList(AvailableCards);
			frame.FreeList(CardQueue);

			AvailableCards = default;
			CardQueue      = default;
		}

		public void Update(Frame frame, EntityRef entity)
		{
			if (CardQueue.Ptr.Offset == 0)
				return;

			CurrentEnergy  = FPMath.Clamp(CurrentEnergy + frame.DeltaTime * EnergyFillRate, FP._0, MaxEnergy);
			NextFillTime  -= frame.DeltaTime;

			if (EmptySlots > 0 && NextFillTime <= FP._0)
			{
				FillCardSlot(frame, entity);
			}
		}

		public void UseCard(Frame frame, EntityRef entity, PlayerRef owner, byte cardIndex, FPVector2 position, FP rotation)
		{
			if (cardIndex < 0 || cardIndex >= AVAILABLE_CARDS_COUNT)
			{
				Log.Error($"Out of range card use request Index: {cardIndex}");
				return;
			}

			var availableCards = frame.ResolveList(AvailableCards);
			var card           = availableCards[cardIndex];

			if (card.CardSettings.Id.IsValid == false)
			{
				Log.Error($"Invalid card use request. Index: {cardIndex}");
				return;
			}

			var settings = frame.FindAsset<CardSettings>(card.CardSettings.Id);
			if (settings.EnergyCost > CurrentEnergy)
			{
				Log.Error($"Not enough energy card request. Index: {cardIndex} Cost: {settings.EnergyCost} Current: {CurrentEnergy}");
				return;
			}

			if (settings is UnitSettings)
			{
				var gameplay = frame.Unsafe.GetPointerSingleton<Gameplay>();
				if (gameplay->IsValidUnitPosition(frame, owner, position) == false)
				{
					Log.Error($"Invalid unit spawn position. Position: {position}");
					return;
				}
			}

			frame.SpawnCard(settings, owner, position, rotation, card.Level, card);
			frame.Events.CardSpawned(owner, card.TokenID, card.CardSettings);
				
			AddCardToQueue(frame, card);

			CurrentEnergy -= settings.EnergyCost;
			availableCards[cardIndex] = default;
			EmptySlots += 1;

			frame.Events.CardsChanged(entity);
		}

		// PRIVATE METHODS

		private void FillCardSlot(Frame frame, EntityRef entity)
		{
			var availableCards = frame.ResolveList(AvailableCards);

			for (int idx = 0; idx < AVAILABLE_CARDS_COUNT; idx++)
			{
				if (availableCards[idx].CardSettings.Id.IsValid == true)
					continue;

				var cardQueue = frame.ResolveList(CardQueue);

				availableCards[idx] = cardQueue[QueueHeadIndex];
				QueueHeadIndex      = (byte)((QueueHeadIndex + 1) % cardQueue.Count);
				EmptySlots         -= 1;
				NextFillTime        = FP._2;

				frame.Events.CardsChanged(entity);
				break;
			}
		}

		private void AddCardToQueue(Frame frame, CardInfo card)
		{
			var cardQueue = frame.ResolveList(CardQueue);

			cardQueue[QueueTailIndex] = card;
			QueueTailIndex            = (byte)((QueueTailIndex + 1) % cardQueue.Count);
		}

		private FPVector2 TransformPosition(FPVector2 position, int unitCount, int unitIndex)
		{
			if (unitCount == 1)
				return position;

			if (unitCount <= 4)
			{
				var rotationOffset = FP.Rad_180 * FP._2 / unitCount * unitIndex;
				position += FPVector2.Rotate(FPVector2.Right * FP._0_50, rotationOffset);
			}

			if (unitCount <= 12)
			{
				if (unitIndex < 4)
				{
					var rotationOffset = FP.Rad_180 * FP._2 / 4 * unitIndex;
					position += FPVector2.Rotate(FPVector2.Right * FP._0_50, rotationOffset);
				}
				else
				{
					var rotationOffset = FP.Rad_180 * FP._2 / (unitCount - 4) * unitIndex;
					position += FPVector2.Rotate(FPVector2.Right, rotationOffset);
				}
			}

			return position;
		}
	}

	[System.Serializable]
	unsafe partial struct CardInfo
	{
	}
}
