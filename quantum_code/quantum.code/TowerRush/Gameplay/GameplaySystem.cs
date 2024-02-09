namespace Quantum
{
	unsafe class GameplaySystem : SystemMainThread, ISignalOnMapChanged, ISignalOnPlayerDataSet, ISignalOnDeath
	{
		// SystemMainThread INTERFACE

		public override void OnInit(Frame frame)
		{
			(this as ISignalOnMapChanged).OnMapChanged(frame, null);

			frame.Unsafe.GetPointerSingleton<Gameplay>()->Initialize(frame);
		}

		public override void Update(Frame frame)
		{
			frame.Unsafe.GetPointerSingleton<Gameplay>()->Update(frame);
		}

		// ISignalOnMapChanged INTERFACE

		void ISignalOnMapChanged.OnMapChanged(Frame frame, AssetRefMap previousMap)
		{
			if (frame.Map != null)
			{
				foreach (var item in frame.Map.NavMeshes)
				{
					frame.Context.CurrentNavMesh = item.Value;
					break;
				}
			}
		}

		// ISignalOnPlayerDataSet INTERFACE

		void ISignalOnPlayerDataSet.OnPlayerDataSet(Frame frame, PlayerRef player)
		{
			frame.Unsafe.GetPointerSingleton<Gameplay>()->PlayerDataSet(frame, player);
		}

		// ISignalOnDeath INTERFACE

		void ISignalOnDeath.OnDeath(Frame frame, EntityRef entity, EntityRef killer)
		{
			frame.Unsafe.GetPointerSingleton<Gameplay>()->OnDeath(frame, entity);
		}
	}
}
