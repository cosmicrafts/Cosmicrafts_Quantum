using EdjCase.ICP.Candid.Mapping;
using CanisterPK.CanisterLogin.Models;
using EdjCase.ICP.Candid.Models;
using System;

namespace CanisterPK.CanisterLogin.Models
{
	[Variant]
	public class AdminFunction
	{
		[VariantTagProperty]
		public AdminFunctionTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public AdminFunction(AdminFunctionTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected AdminFunction()
		{
		}

		public static AdminFunction CreateMission((string, Missiontype1, RewardType, UnboundedUInt, UnboundedUInt, ulong) info)
		{
			return new AdminFunction(AdminFunctionTag.CreateMission, info);
		}

		public static AdminFunction CreateMissionsPeriodically()
		{
			return new AdminFunction(AdminFunctionTag.CreateMissionsPeriodically, null);
		}

		public static AdminFunction MintChest((Principal, UnboundedUInt) info)
		{
			return new AdminFunction(AdminFunctionTag.MintChest, info);
		}

		public (string, Missiontype1, RewardType, UnboundedUInt, UnboundedUInt, ulong) AsCreateMission()
		{
			this.ValidateTag(AdminFunctionTag.CreateMission);
			return ((string, Missiontype1, RewardType, UnboundedUInt, UnboundedUInt, ulong))this.Value!;
		}

		public (Principal, UnboundedUInt) AsMintChest()
		{
			this.ValidateTag(AdminFunctionTag.MintChest);
			return ((Principal, UnboundedUInt))this.Value!;
		}

		private void ValidateTag(AdminFunctionTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum AdminFunctionTag
	{
		CreateMission,
		CreateMissionsPeriodically,
		MintChest
	}
}