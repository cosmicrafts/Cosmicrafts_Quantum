using EdjCase.ICP.Candid.Mapping;
using CanisterPK.UserGeekICHubCanister.Models;
using System;

namespace CanisterPK.UserGeekICHubCanister.Models
{
	[Variant]
	public class BatchOperationKind
	{
		[VariantTagProperty]
		public BatchOperationKindTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public BatchOperationKind(BatchOperationKindTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected BatchOperationKind()
		{
		}

		public static BatchOperationKind CreateAsset(CreateAssetArguments info)
		{
			return new BatchOperationKind(BatchOperationKindTag.CreateAsset, info);
		}

		public static BatchOperationKind SetAssetContent(SetAssetContentArguments info)
		{
			return new BatchOperationKind(BatchOperationKindTag.SetAssetContent, info);
		}

		public static BatchOperationKind UnsetAssetContent(UnsetAssetContentArguments info)
		{
			return new BatchOperationKind(BatchOperationKindTag.UnsetAssetContent, info);
		}

		public static BatchOperationKind DeleteAsset(DeleteAssetArguments info)
		{
			return new BatchOperationKind(BatchOperationKindTag.DeleteAsset, info);
		}

		public static BatchOperationKind Clear(ClearArguments info)
		{
			return new BatchOperationKind(BatchOperationKindTag.Clear, info);
		}

		public CreateAssetArguments AsCreateAsset()
		{
			this.ValidateTag(BatchOperationKindTag.CreateAsset);
			return (CreateAssetArguments)this.Value!;
		}

		public SetAssetContentArguments AsSetAssetContent()
		{
			this.ValidateTag(BatchOperationKindTag.SetAssetContent);
			return (SetAssetContentArguments)this.Value!;
		}

		public UnsetAssetContentArguments AsUnsetAssetContent()
		{
			this.ValidateTag(BatchOperationKindTag.UnsetAssetContent);
			return (UnsetAssetContentArguments)this.Value!;
		}

		public DeleteAssetArguments AsDeleteAsset()
		{
			this.ValidateTag(BatchOperationKindTag.DeleteAsset);
			return (DeleteAssetArguments)this.Value!;
		}

		public ClearArguments AsClear()
		{
			this.ValidateTag(BatchOperationKindTag.Clear);
			return (ClearArguments)this.Value!;
		}

		private void ValidateTag(BatchOperationKindTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum BatchOperationKindTag
	{
		CreateAsset,
		SetAssetContent,
		UnsetAssetContent,
		DeleteAsset,
		Clear
	}
}