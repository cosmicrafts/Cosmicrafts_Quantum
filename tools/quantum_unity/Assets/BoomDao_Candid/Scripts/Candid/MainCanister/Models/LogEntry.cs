using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.MainCanister.Models;
using TokenID = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Timestamp = System.UInt64;

namespace Cosmicrafts.MainCanister.Models
{
	public class LogEntry
	{
		[CandidName("amount")]
		public OptionalValue<UnboundedUInt> Amount { get; set; }

		[CandidName("itemType")]
		public ItemType ItemType { get; set; }

		[CandidName("timestamp")]
		public ulong Timestamp { get; set; }

		[CandidName("tokenID")]
		public LogEntry.TokenIDInfo TokenID { get; set; }

		[CandidName("user")]
		public Principal User { get; set; }

		public LogEntry(OptionalValue<UnboundedUInt> amount, ItemType itemType, ulong timestamp, LogEntry.TokenIDInfo tokenID, Principal user)
		{
			this.Amount = amount;
			this.ItemType = itemType;
			this.Timestamp = timestamp;
			this.TokenID = tokenID;
			this.User = user;
		}

		public LogEntry()
		{
		}

		public class TokenIDInfo : OptionalValue<TokenID>
		{
			public TokenIDInfo()
			{
			}

			public TokenIDInfo(TokenID value) : base(value)
			{
			}
		}
	}
}