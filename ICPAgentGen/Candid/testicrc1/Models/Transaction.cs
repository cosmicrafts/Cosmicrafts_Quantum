using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using CanisterPK.testicrc1.Models;
using TxIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Timestamp = System.UInt64;

namespace CanisterPK.testicrc1.Models
{
	public class Transaction
	{
		[CandidName("burn")]
		public OptionalValue<Burn> Burn { get; set; }

		[CandidName("index")]
		public TxIndex Index { get; set; }

		[CandidName("kind")]
		public string Kind { get; set; }

		[CandidName("mint")]
		public OptionalValue<Mint1> Mint { get; set; }

		[CandidName("timestamp")]
		public Timestamp Timestamp { get; set; }

		[CandidName("transfer")]
		public OptionalValue<Transfer> Transfer { get; set; }

		public Transaction(OptionalValue<Burn> burn, TxIndex index, string kind, OptionalValue<Mint1> mint, Timestamp timestamp, OptionalValue<Transfer> transfer)
		{
			this.Burn = burn;
			this.Index = index;
			this.Kind = kind;
			this.Mint = mint;
			this.Timestamp = timestamp;
			this.Transfer = transfer;
		}

		public Transaction()
		{
		}
	}
}