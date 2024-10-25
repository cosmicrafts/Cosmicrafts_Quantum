using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.MainCanister.Models;
using System.Collections.Generic;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Timestamp = System.UInt64;

namespace Cosmicrafts.MainCanister.Models
{
	public class Transaction
	{
		[CandidName("icrc7_approve")]
		public OptionalValue<Transaction.Icrc7ApproveValue> Icrc7Approve { get; set; }

		[CandidName("icrc7_transfer")]
		public OptionalValue<Transaction.Icrc7TransferValue> Icrc7Transfer { get; set; }

		[CandidName("kind")]
		public string Kind { get; set; }

		[CandidName("mint")]
		public OptionalValue<Transaction.MintValue> Mint { get; set; }

		[CandidName("timestamp")]
		public ulong Timestamp { get; set; }

		[CandidName("upgrade")]
		public OptionalValue<Transaction.UpgradeValue> Upgrade { get; set; }

		public Transaction(OptionalValue<Transaction.Icrc7ApproveValue> icrc7Approve, OptionalValue<Transaction.Icrc7TransferValue> icrc7Transfer, string kind, OptionalValue<Transaction.MintValue> mint, ulong timestamp, OptionalValue<Transaction.UpgradeValue> upgrade)
		{
			this.Icrc7Approve = icrc7Approve;
			this.Icrc7Transfer = icrc7Transfer;
			this.Kind = kind;
			this.Mint = mint;
			this.Timestamp = timestamp;
			this.Upgrade = upgrade;
		}

		public Transaction()
		{
		}

		public class Icrc7ApproveValue
		{
			[CandidName("created_at_time")]
			public OptionalValue<ulong> CreatedAtTime { get; set; }

			[CandidName("expires_at")]
			public OptionalValue<ulong> ExpiresAt { get; set; }

			[CandidName("from")]
			public Account From { get; set; }

			[CandidName("memo")]
			public OptionalValue<List<byte>> Memo { get; set; }

			[CandidName("spender")]
			public Account Spender { get; set; }

			[CandidName("token_ids")]
			public OptionalValue<List<TokenId>> TokenIds { get; set; }

			public Icrc7ApproveValue(OptionalValue<ulong> createdAtTime, OptionalValue<ulong> expiresAt, Account from, OptionalValue<List<byte>> memo, Account spender, OptionalValue<List<TokenId>> tokenIds)
			{
				this.CreatedAtTime = createdAtTime;
				this.ExpiresAt = expiresAt;
				this.From = from;
				this.Memo = memo;
				this.Spender = spender;
				this.TokenIds = tokenIds;
			}

			public Icrc7ApproveValue()
			{
			}
		}

		public class Icrc7TransferValue
		{
			[CandidName("created_at_time")]
			public OptionalValue<ulong> CreatedAtTime { get; set; }

			[CandidName("from")]
			public Account From { get; set; }

			[CandidName("memo")]
			public OptionalValue<List<byte>> Memo { get; set; }

			[CandidName("spender")]
			public OptionalValue<Account> Spender { get; set; }

			[CandidName("to")]
			public Account To { get; set; }

			[CandidName("token_ids")]
			public List<TokenId> TokenIds { get; set; }

			public Icrc7TransferValue(OptionalValue<ulong> createdAtTime, Account from, OptionalValue<List<byte>> memo, OptionalValue<Account> spender, Account to, List<TokenId> tokenIds)
			{
				this.CreatedAtTime = createdAtTime;
				this.From = from;
				this.Memo = memo;
				this.Spender = spender;
				this.To = to;
				this.TokenIds = tokenIds;
			}

			public Icrc7TransferValue()
			{
			}
		}

		public class MintValue
		{
			[CandidName("to")]
			public Account To { get; set; }

			[CandidName("token_ids")]
			public List<TokenId> TokenIds { get; set; }

			public MintValue(Account to, List<TokenId> tokenIds)
			{
				this.To = to;
				this.TokenIds = tokenIds;
			}

			public MintValue()
			{
			}
		}

		public class UpgradeValue
		{
			[CandidName("memo")]
			public OptionalValue<List<byte>> Memo { get; set; }

			[CandidName("new_metadata")]
			public Dictionary<string, Metadata> NewMetadata { get; set; }

			[CandidName("prev_metadata")]
			public Dictionary<string, Metadata> PrevMetadata { get; set; }

			[CandidName("token_id")]
			public OptionalValue<TokenId> TokenId { get; set; }

			[CandidName("upgraded_at")]
			public OptionalValue<ulong> UpgradedAt { get; set; }

			public UpgradeValue(OptionalValue<List<byte>> memo, Dictionary<string, Metadata> newMetadata, Dictionary<string, Metadata> prevMetadata, OptionalValue<TokenId> tokenId, OptionalValue<ulong> upgradedAt)
			{
				this.Memo = memo;
				this.NewMetadata = newMetadata;
				this.PrevMetadata = prevMetadata;
				this.TokenId = tokenId;
				this.UpgradedAt = upgradedAt;
			}

			public UpgradeValue()
			{
			}
		}
	}
}