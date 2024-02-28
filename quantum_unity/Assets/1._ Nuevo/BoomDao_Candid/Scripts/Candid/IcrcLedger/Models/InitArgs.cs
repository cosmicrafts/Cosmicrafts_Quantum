using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Candid.IcrcLedger.Models;
using System.Collections.Generic;
using TextAccountIdentifier = System.String;

namespace Candid.IcrcLedger.Models
{
	public class InitArgs
	{
		[CandidName("minting_account")]
		public TextAccountIdentifier MintingAccount { get; set; }

		[CandidName("icrc1_minting_account")]
		public OptionalValue<Account> Icrc1MintingAccount { get; set; }

		[CandidName("initial_values")]
		public InitArgs.InitialValuesInfo InitialValues { get; set; }

		[CandidName("max_message_size_bytes")]
		public OptionalValue<ulong> MaxMessageSizeBytes { get; set; }

		[CandidName("transaction_window")]
		public OptionalValue<Duration> TransactionWindow { get; set; }

		[CandidName("archive_options")]
		public OptionalValue<ArchiveOptions> ArchiveOptions { get; set; }

		[CandidName("send_whitelist")]
		public List<Principal> SendWhitelist { get; set; }

		[CandidName("transfer_fee")]
		public OptionalValue<Tokens> TransferFee { get; set; }

		[CandidName("token_symbol")]
		public OptionalValue<string> TokenSymbol { get; set; }

		[CandidName("token_name")]
		public OptionalValue<string> TokenName { get; set; }

		[CandidName("feature_flags")]
		public OptionalValue<FeatureFlags> FeatureFlags { get; set; }

		[CandidName("maximum_number_of_accounts")]
		public OptionalValue<ulong> MaximumNumberOfAccounts { get; set; }

		[CandidName("accounts_overflow_trim_quantity")]
		public OptionalValue<ulong> AccountsOverflowTrimQuantity { get; set; }

		public InitArgs(TextAccountIdentifier mintingAccount, OptionalValue<Account> icrc1MintingAccount, InitArgs.InitialValuesInfo initialValues, OptionalValue<ulong> maxMessageSizeBytes, OptionalValue<Duration> transactionWindow, OptionalValue<ArchiveOptions> archiveOptions, List<Principal> sendWhitelist, OptionalValue<Tokens> transferFee, OptionalValue<string> tokenSymbol, OptionalValue<string> tokenName, OptionalValue<FeatureFlags> featureFlags, OptionalValue<ulong> maximumNumberOfAccounts, OptionalValue<ulong> accountsOverflowTrimQuantity)
		{
			this.MintingAccount = mintingAccount;
			this.Icrc1MintingAccount = icrc1MintingAccount;
			this.InitialValues = initialValues;
			this.MaxMessageSizeBytes = maxMessageSizeBytes;
			this.TransactionWindow = transactionWindow;
			this.ArchiveOptions = archiveOptions;
			this.SendWhitelist = sendWhitelist;
			this.TransferFee = transferFee;
			this.TokenSymbol = tokenSymbol;
			this.TokenName = tokenName;
			this.FeatureFlags = featureFlags;
			this.MaximumNumberOfAccounts = maximumNumberOfAccounts;
			this.AccountsOverflowTrimQuantity = accountsOverflowTrimQuantity;
		}

		public InitArgs()
		{
		}

		public class InitialValuesInfo : List<InitArgs.InitialValuesInfo.InitialValuesInfoElement>
		{
			public InitialValuesInfo()
			{
			}

			public class InitialValuesInfoElement
			{
				[CandidTag(0U)]
				public TextAccountIdentifier F0 { get; set; }

				[CandidTag(1U)]
				public Tokens F1 { get; set; }

				public InitialValuesInfoElement(TextAccountIdentifier f0, Tokens f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public InitialValuesInfoElement()
				{
				}
			}
		}
	}
}