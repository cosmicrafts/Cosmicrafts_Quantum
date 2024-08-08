using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.MainCanister.Models;
using Description = System.String;

namespace Cosmicrafts.MainCanister.Models
{
	public class GeneralMetadata
	{
		[CandidName("category")]
		public OptionalValue<Category> Category { get; set; }

		[CandidName("description")]
		public string Description { get; set; }

		[CandidName("faction")]
		public OptionalValue<Faction> Faction { get; set; }

		[CandidName("id")]
		public UnboundedUInt Id { get; set; }

		[CandidName("image")]
		public string Image { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		[CandidName("rarity")]
		public OptionalValue<UnboundedUInt> Rarity { get; set; }

		public GeneralMetadata(OptionalValue<Category> category, string description, OptionalValue<Faction> faction, UnboundedUInt id, string image, string name, OptionalValue<UnboundedUInt> rarity)
		{
			this.Category = category;
			this.Description = description;
			this.Faction = faction;
			this.Id = id;
			this.Image = image;
			this.Name = name;
			this.Rarity = rarity;
		}

		public GeneralMetadata()
		{
		}
	}
}