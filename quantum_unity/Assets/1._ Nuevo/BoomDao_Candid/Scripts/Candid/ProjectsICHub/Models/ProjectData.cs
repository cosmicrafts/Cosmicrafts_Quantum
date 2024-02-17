using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using CanisterPK.ProjectsICHub.Models;

namespace CanisterPK.ProjectsICHub.Models
{
	public class ProjectData
	{
		[CandidName("appCategoryIndex")]
		public UnboundedUInt AppCategoryIndex { get; set; }

		[CandidName("banner")]
		public string Banner { get; set; }

		[CandidName("blockchain")]
		public string Blockchain { get; set; }

		[CandidName("catalyze")]
		public string Catalyze { get; set; }

		[CandidName("currentVersion")]
		public string CurrentVersion { get; set; }

		[CandidName("description")]
		public string Description { get; set; }

		[CandidName("distrikt")]
		public string Distrikt { get; set; }

		[CandidName("dscvrPortal")]
		public string DscvrPortal { get; set; }

		[CandidName("id")]
		public UnboundedUInt Id { get; set; }

		[CandidName("launchLink")]
		public string LaunchLink { get; set; }

		[CandidName("logo")]
		public string Logo { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		[CandidName("newVersion")]
		public string NewVersion { get; set; }

		[CandidName("nftCollections")]
		public NFTsCollections NftCollections { get; set; }

		[CandidName("openChat")]
		public string OpenChat { get; set; }

		[CandidName("patchNotes")]
		public string PatchNotes { get; set; }

		[CandidName("twitter")]
		public string Twitter { get; set; }

		public ProjectData(UnboundedUInt appCategoryIndex, string banner, string blockchain, string catalyze, string currentVersion, string description, string distrikt, string dscvrPortal, UnboundedUInt id, string launchLink, string logo, string name, string newVersion, NFTsCollections nftCollections, string openChat, string patchNotes, string twitter)
		{
			this.AppCategoryIndex = appCategoryIndex;
			this.Banner = banner;
			this.Blockchain = blockchain;
			this.Catalyze = catalyze;
			this.CurrentVersion = currentVersion;
			this.Description = description;
			this.Distrikt = distrikt;
			this.DscvrPortal = dscvrPortal;
			this.Id = id;
			this.LaunchLink = launchLink;
			this.Logo = logo;
			this.Name = name;
			this.NewVersion = newVersion;
			this.NftCollections = nftCollections;
			this.OpenChat = openChat;
			this.PatchNotes = patchNotes;
			this.Twitter = twitter;
		}

		public ProjectData()
		{
		}
	}
}