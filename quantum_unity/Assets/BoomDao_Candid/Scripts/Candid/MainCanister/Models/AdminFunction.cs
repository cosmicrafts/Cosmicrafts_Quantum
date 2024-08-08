using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.MainCanister.Models;
using EdjCase.ICP.Candid.Models;
using System;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.MainCanister.Models
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

		public static AdminFunction BurnToken(AdminFunction.BurnTokenInfo info)
		{
			return new AdminFunction(AdminFunctionTag.BurnToken, info);
		}

		public static AdminFunction CreateMission((string, Missiontype1, RewardType, UnboundedUInt, UnboundedUInt, ulong) info)
		{
			return new AdminFunction(AdminFunctionTag.CreateMission, info);
		}

		public static AdminFunction CreateMissionsPeriodically()
		{
			return new AdminFunction(AdminFunctionTag.CreateMissionsPeriodically, null);
		}

		public static AdminFunction GetCollectionOwner(Account info)
		{
			return new AdminFunction(AdminFunctionTag.GetCollectionOwner, info);
		}

		public static AdminFunction GetInitArgs(CollectionInitArgs info)
		{
			return new AdminFunction(AdminFunctionTag.GetInitArgs, info);
		}

		public static AdminFunction MintChest((Principal, UnboundedUInt) info)
		{
			return new AdminFunction(AdminFunctionTag.MintChest, info);
		}

		public AdminFunction.BurnTokenInfo AsBurnToken()
		{
			this.ValidateTag(AdminFunctionTag.BurnToken);
			return (AdminFunction.BurnTokenInfo)this.Value!;
		}

		public (string, Missiontype1, RewardType, UnboundedUInt, UnboundedUInt, ulong) AsCreateMission()
		{
			this.ValidateTag(AdminFunctionTag.CreateMission);
			return ((string, Missiontype1, RewardType, UnboundedUInt, UnboundedUInt, ulong))this.Value!;
		}

		public Account AsGetCollectionOwner()
		{
			this.ValidateTag(AdminFunctionTag.GetCollectionOwner);
			return (Account)this.Value!;
		}

		public CollectionInitArgs AsGetInitArgs()
		{
			this.ValidateTag(AdminFunctionTag.GetInitArgs);
			return (CollectionInitArgs)this.Value!;
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

		public class BurnTokenInfo
		{
			[CandidTag(0U)]
			public OptionalValue<Account> F0 { get; set; }

			[CandidTag(1U)]
			public Account F1 { get; set; }

			[CandidTag(2U)]
			public TokenId F2 { get; set; }

			[CandidTag(3U)]
			public ulong F3 { get; set; }

			public BurnTokenInfo(OptionalValue<Account> f0, Account f1, TokenId f2, ulong f3)
			{
				this.F0 = f0;
				this.F1 = f1;
				this.F2 = f2;
				this.F3 = f3;
			}

			public BurnTokenInfo()
			{
			}
		}
	}

	public enum AdminFunctionTag
	{
		BurnToken,
		CreateMission,
		CreateMissionsPeriodically,
		GetCollectionOwner,
		GetInitArgs,
		MintChest
	}
}