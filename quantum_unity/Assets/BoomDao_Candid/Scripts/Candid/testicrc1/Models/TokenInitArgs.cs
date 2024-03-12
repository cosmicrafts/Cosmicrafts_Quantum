using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using CanisterPK.testicrc1.Models;
using System.Collections.Generic;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.testicrc1.Models
{
	public class TokenInitArgs
	{
		[CandidName("advanced_settings")]
		public OptionalValue<AdvancedSettings> AdvancedSettings { get; set; }

		[CandidName("decimals")]
		public byte Decimals { get; set; }

		[CandidName("fee")]
		public Balance Fee { get; set; }

		[CandidName("initial_balances")]
		public Dictionary<Account, Balance> InitialBalances { get; set; }

		[CandidName("max_supply")]
		public Balance MaxSupply { get; set; }

		[CandidName("min_burn_amount")]
		public Balance MinBurnAmount { get; set; }

		[CandidName("minting_account")]
		public OptionalValue<Account> MintingAccount { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		[CandidName("symbol")]
		public string Symbol { get; set; }

		public TokenInitArgs(OptionalValue<AdvancedSettings> advancedSettings, byte decimals, Balance fee, Dictionary<Account, Balance> initialBalances, Balance maxSupply, Balance minBurnAmount, OptionalValue<Account> mintingAccount, string name, string symbol)
		{
			this.AdvancedSettings = advancedSettings;
			this.Decimals = decimals;
			this.Fee = fee;
			this.InitialBalances = initialBalances;
			this.MaxSupply = maxSupply;
			this.MinBurnAmount = minBurnAmount;
			this.MintingAccount = mintingAccount;
			this.Name = name;
			this.Symbol = symbol;
		}

		public TokenInitArgs()
		{
		}
	}
}