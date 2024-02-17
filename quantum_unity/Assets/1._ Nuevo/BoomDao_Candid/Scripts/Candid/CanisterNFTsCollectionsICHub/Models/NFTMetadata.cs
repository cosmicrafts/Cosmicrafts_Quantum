using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace CanisterPK.CanisterNFTsCollectionsICHub.Models
{
	public class NFTMetadata
	{
		[CandidName("addedBy")]
		public Principal AddedBy { get; set; }

		[CandidName("avatarURL")]
		public string AvatarURL { get; set; }

		[CandidName("canisterID")]
		public Principal CanisterID { get; set; }

		[CandidName("marketplace")]
		public OptionalValue<string> Marketplace { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		[CandidName("standard")]
		public string Standard { get; set; }

		public NFTMetadata(Principal addedBy, string avatarURL, Principal canisterID, OptionalValue<string> marketplace, string name, string standard)
		{
			this.AddedBy = addedBy;
			this.AvatarURL = avatarURL;
			this.CanisterID = canisterID;
			this.Marketplace = marketplace;
			this.Name = name;
			this.Standard = standard;
		}

		public NFTMetadata()
		{
		}
	}
}