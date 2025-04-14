using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Cosmicrafts.backend.Models;
using Description = System.String;

namespace Cosmicrafts.backend.Models
{
	public class GeneralMetadata
	{
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

		public GeneralMetadata(string description, OptionalValue<Faction> faction, UnboundedUInt id, string image, string name, OptionalValue<UnboundedUInt> rarity)
		{
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