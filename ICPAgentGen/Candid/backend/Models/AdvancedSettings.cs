using EdjCase.ICP.Candid.Mapping;
using Timestamp = System.UInt64;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.backend.Models
{
	public class AdvancedSettings
	{
		[CandidName("burned_tokens")]
		public Balance BurnedTokens { get; set; }

		[CandidName("permitted_drift")]
		public Timestamp PermittedDrift { get; set; }

		[CandidName("transaction_window")]
		public Timestamp TransactionWindow { get; set; }

		public AdvancedSettings(Balance burnedTokens, Timestamp permittedDrift, Timestamp transactionWindow)
		{
			this.BurnedTokens = burnedTokens;
			this.PermittedDrift = permittedDrift;
			this.TransactionWindow = transactionWindow;
		}

		public AdvancedSettings()
		{
		}
	}
}