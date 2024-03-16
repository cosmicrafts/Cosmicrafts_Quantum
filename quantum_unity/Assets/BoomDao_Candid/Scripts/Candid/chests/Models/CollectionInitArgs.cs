using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using CanisterPK.chests.Models;

namespace CanisterPK.chests.Models
{
	public class CollectionInitArgs
	{
		[CandidName("description")]
		public OptionalValue<string> Description { get; set; }

		[CandidName("image")]
		public OptionalValue<List<byte>> Image { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		[CandidName("royalties")]
		public OptionalValue<ushort> Royalties { get; set; }

		[CandidName("royaltyRecipient")]
		public OptionalValue<Account> RoyaltyRecipient { get; set; }

		[CandidName("supplyCap")]
		public OptionalValue<UnboundedUInt> SupplyCap { get; set; }

		[CandidName("symbol")]
		public string Symbol { get; set; }

		public CollectionInitArgs(OptionalValue<string> description, OptionalValue<List<byte>> image, string name, OptionalValue<ushort> royalties, OptionalValue<Account> royaltyRecipient, OptionalValue<UnboundedUInt> supplyCap, string symbol)
		{
			this.Description = description;
			this.Image = image;
			this.Name = name;
			this.Royalties = royalties;
			this.RoyaltyRecipient = royaltyRecipient;
			this.SupplyCap = supplyCap;
			this.Symbol = symbol;
		}

		public CollectionInitArgs()
		{
		}
	}
}