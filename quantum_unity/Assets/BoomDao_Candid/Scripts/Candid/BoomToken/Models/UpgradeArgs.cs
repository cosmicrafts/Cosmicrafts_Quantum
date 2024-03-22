using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using Boom.BoomToken.Models;

namespace Boom.BoomToken.Models
{
	public class UpgradeArgs
	{
		[CandidName("metadata")]
		public OptionalValue<Dictionary<string, MetadataValue>> Metadata { get; set; }

		[CandidName("token_symbol")]
		public OptionalValue<string> TokenSymbol { get; set; }

		[CandidName("token_name")]
		public OptionalValue<string> TokenName { get; set; }

		[CandidName("transfer_fee")]
		public OptionalValue<ulong> TransferFee { get; set; }

		[CandidName("change_fee_collector")]
		public OptionalValue<ChangeFeeCollector> ChangeFeeCollector { get; set; }

		[CandidName("max_memo_length")]
		public OptionalValue<ushort> MaxMemoLength { get; set; }

		[CandidName("feature_flags")]
		public OptionalValue<FeatureFlags> FeatureFlags { get; set; }

		public UpgradeArgs(OptionalValue<Dictionary<string, MetadataValue>> metadata, OptionalValue<string> tokenSymbol, OptionalValue<string> tokenName, OptionalValue<ulong> transferFee, OptionalValue<ChangeFeeCollector> changeFeeCollector, OptionalValue<ushort> maxMemoLength, OptionalValue<FeatureFlags> featureFlags)
		{
			this.Metadata = metadata;
			this.TokenSymbol = tokenSymbol;
			this.TokenName = tokenName;
			this.TransferFee = transferFee;
			this.ChangeFeeCollector = changeFeeCollector;
			this.MaxMemoLength = maxMemoLength;
			this.FeatureFlags = featureFlags;
		}

		public UpgradeArgs()
		{
		}
	}
}