using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Candid.IcrcLedger.Models;

namespace Candid.IcrcLedger.Models
{
	public class UpgradeArgs
	{
		[CandidName("maximum_number_of_accounts")]
		public OptionalValue<ulong> MaximumNumberOfAccounts { get; set; }

		[CandidName("icrc1_minting_account")]
		public OptionalValue<Account> Icrc1MintingAccount { get; set; }

		[CandidName("feature_flags")]
		public OptionalValue<FeatureFlags> FeatureFlags { get; set; }

		public UpgradeArgs(OptionalValue<ulong> maximumNumberOfAccounts, OptionalValue<Account> icrc1MintingAccount, OptionalValue<FeatureFlags> featureFlags)
		{
			this.MaximumNumberOfAccounts = maximumNumberOfAccounts;
			this.Icrc1MintingAccount = icrc1MintingAccount;
			this.FeatureFlags = featureFlags;
		}

		public UpgradeArgs()
		{
		}
	}
}