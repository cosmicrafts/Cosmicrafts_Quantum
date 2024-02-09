namespace Quantum
{
	using Photon.Deterministic;

	public unsafe partial struct Target
	{
		public void Initialize(UnitSettings settings, PlayerRef owner)
		{
			OwnerPlayerRef = owner;
			Type           = settings.TargetType;
			Size           = settings.UnitSize / FP._2;
		}
	}

	public static class ETargetExtensions
	{
		public static ETargetTypeFlags ConvertToFlags(this ETargetType @this)
		{
			switch (@this)
			{
				case ETargetType.None:       return ETargetTypeFlags.None;
				case ETargetType.Building:   return ETargetTypeFlags.Building;
				case ETargetType.UnitGround: return ETargetTypeFlags.UnitGround;
				case ETargetType.UnitAir:    return ETargetTypeFlags.UnitAir;
				default:
					throw new System.NotImplementedException();
			}
		}
	}
}
