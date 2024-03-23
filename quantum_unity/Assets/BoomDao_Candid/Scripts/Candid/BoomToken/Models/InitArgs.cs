using EdjCase.ICP.Candid.Mapping;
using CanisterPK.BoomToken.Models;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;

namespace CanisterPK.BoomToken.Models
{
	public class InitArgs
	{
		[CandidName("minting_account")]
		public Account MintingAccount { get; set; }

		[CandidName("fee_collector_account")]
		public OptionalValue<Account> FeeCollectorAccount { get; set; }

		[CandidName("transfer_fee")]
		public UnboundedUInt TransferFee { get; set; }

		[CandidName("decimals")]
		public OptionalValue<byte> Decimals { get; set; }

		[CandidName("max_memo_length")]
		public OptionalValue<ushort> MaxMemoLength { get; set; }

		[CandidName("token_symbol")]
		public string TokenSymbol { get; set; }

		[CandidName("token_name")]
		public string TokenName { get; set; }

		[CandidName("metadata")]
		public Dictionary<string, MetadataValue> Metadata { get; set; }

		[CandidName("initial_balances")]
		public Dictionary<Account, UnboundedUInt> InitialBalances { get; set; }

		[CandidName("archive_options")]
		public InitArgs.ArchiveOptionsInfo ArchiveOptions { get; set; }

		[CandidName("feature_flags")]
		public OptionalValue<FeatureFlags> FeatureFlags { get; set; }

		public InitArgs(Account mintingAccount, OptionalValue<Account> feeCollectorAccount, UnboundedUInt transferFee, OptionalValue<byte> decimals, OptionalValue<ushort> maxMemoLength, string tokenSymbol, string tokenName, Dictionary<string, MetadataValue> metadata, Dictionary<Account, UnboundedUInt> initialBalances, InitArgs.ArchiveOptionsInfo archiveOptions, OptionalValue<FeatureFlags> featureFlags)
		{
			this.MintingAccount = mintingAccount;
			this.FeeCollectorAccount = feeCollectorAccount;
			this.TransferFee = transferFee;
			this.Decimals = decimals;
			this.MaxMemoLength = maxMemoLength;
			this.TokenSymbol = tokenSymbol;
			this.TokenName = tokenName;
			this.Metadata = metadata;
			this.InitialBalances = initialBalances;
			this.ArchiveOptions = archiveOptions;
			this.FeatureFlags = featureFlags;
		}

		public InitArgs()
		{
		}

		public class ArchiveOptionsInfo
		{
			[CandidName("num_blocks_to_archive")]
			public ulong NumBlocksToArchive { get; set; }

			[CandidName("trigger_threshold")]
			public ulong TriggerThreshold { get; set; }

			[CandidName("max_message_size_bytes")]
			public OptionalValue<ulong> MaxMessageSizeBytes { get; set; }

			[CandidName("cycles_for_archive_creation")]
			public OptionalValue<ulong> CyclesForArchiveCreation { get; set; }

			[CandidName("node_max_memory_size_bytes")]
			public OptionalValue<ulong> NodeMaxMemorySizeBytes { get; set; }

			[CandidName("controller_id")]
			public Principal ControllerId { get; set; }

			public ArchiveOptionsInfo(ulong numBlocksToArchive, ulong triggerThreshold, OptionalValue<ulong> maxMessageSizeBytes, OptionalValue<ulong> cyclesForArchiveCreation, OptionalValue<ulong> nodeMaxMemorySizeBytes, Principal controllerId)
			{
				this.NumBlocksToArchive = numBlocksToArchive;
				this.TriggerThreshold = triggerThreshold;
				this.MaxMessageSizeBytes = maxMessageSizeBytes;
				this.CyclesForArchiveCreation = cyclesForArchiveCreation;
				this.NodeMaxMemorySizeBytes = nodeMaxMemorySizeBytes;
				this.ControllerId = controllerId;
			}

			public ArchiveOptionsInfo()
			{
			}
		}
	}
}