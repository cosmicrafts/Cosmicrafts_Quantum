using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.MainCanister.Models;
using EdjCase.ICP.Candid.Models;
using System;

namespace Cosmicrafts.MainCanister.Models
{
	[Variant]
	public class Unit
	{
		[VariantTagProperty]
		public UnitTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Unit(UnitTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Unit()
		{
		}

		public static Unit Spaceship(OptionalValue<SpaceshipMetadata> info)
		{
			return new Unit(UnitTag.Spaceship, info);
		}

		public static Unit Station(OptionalValue<StationMetadata> info)
		{
			return new Unit(UnitTag.Station, info);
		}

		public static Unit Weapon(OptionalValue<WeaponMetadata> info)
		{
			return new Unit(UnitTag.Weapon, info);
		}

		public OptionalValue<SpaceshipMetadata> AsSpaceship()
		{
			this.ValidateTag(UnitTag.Spaceship);
			return (OptionalValue<SpaceshipMetadata>)this.Value!;
		}

		public OptionalValue<StationMetadata> AsStation()
		{
			this.ValidateTag(UnitTag.Station);
			return (OptionalValue<StationMetadata>)this.Value!;
		}

		public OptionalValue<WeaponMetadata> AsWeapon()
		{
			this.ValidateTag(UnitTag.Weapon);
			return (OptionalValue<WeaponMetadata>)this.Value!;
		}

		private void ValidateTag(UnitTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum UnitTag
	{
		[CandidName("spaceship")]
		Spaceship,
		[CandidName("station")]
		Station,
		[CandidName("weapon")]
		Weapon
	}
}