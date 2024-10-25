namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct PlayerArea
	{
		public void RefreshArea(Frame frame)
		{
			var areas = frame.ResolveList(BlockedAreas);

			for (int idx = 0, count = areas.Count; idx < count; idx++)
			{
				var area = areas.GetPointer(idx);
				if (area->Entity == EntityRef.None)
				{
					area->Enabled = true;
				}
				else if (frame.Exists(area->Entity) == true && frame.DestroyPending(area->Entity) == false)
				{
					var areaHealth = frame.Unsafe.GetPointer<Health>(area->Entity);
					area->Enabled  = areaHealth->IsAlive;
				}
				else
				{
					area->Enabled = true;
				}
			}
		}

		public bool IsValidUnitPosition(Frame frame, FPVector2 position)
		{
			if (position.X < MinX || position.X > MaxX)
				return false;
			if (position.Y < MinY || position.Y > MaxY)
				return false;

			var areas = frame.ResolveList(BlockedAreas);

			for (int idx = 0, count = areas.Count; idx < count; idx++)
			{
				var area = areas.GetPointer(idx);
				if (area->Enabled == false)
					continue;

				if (position.X > area->MinX && position.X < area->MaxX &&
				    position.Y > area->MinY && position.Y < area->MaxY)
					return false;
			}

			return true;
		}
	}
}
