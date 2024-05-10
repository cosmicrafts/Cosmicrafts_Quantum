namespace Quantum
{
	using Photon.Deterministic;

	unsafe class CardManagerSystem : SystemMainThread
	{
		public override void Update(Frame frame)
		{
			foreach (var pair in frame.Unsafe.GetComponentBlockIterator<CardManager>())
			{
				pair.Component->Update(frame, pair.Entity);
			}

			var gameplay = frame.Unsafe.GetPointerSingleton<Gameplay>();
			if (gameplay->IsActive == false)
				return;

			foreach (var pair in frame.Unsafe.GetComponentBlockIterator<Player>())
			{
				var command = frame.GetPlayerCommand(pair.Component->PlayerRef);
				switch (command)
				{
					case UseCardCommand useCard:
						ProcessCommand(frame, pair.Entity, pair.Component->PlayerRef, useCard);
						break;
				}
			}
		}

		// PRIVATE METHODS

		private void ProcessCommand(Frame frame, EntityRef entity, PlayerRef playerRef, UseCardCommand useCard)
		{
			var cardManager = frame.Unsafe.GetPointer<CardManager>(entity);
			cardManager->UseCard(frame, entity, playerRef, useCard.CardIndex, useCard.Position, FP.Rad_180 + (playerRef * FP.Rad_180) );
		}
	}
}
