using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace CanisterPK.ProjectsICHub.Models
{
	public class VersionData
	{
		[CandidName("blockchain")]
		public string Blockchain { get; set; }

		[CandidName("currentVersion")]
		public string CurrentVersion { get; set; }

		[CandidName("linkDapp")]
		public string LinkDapp { get; set; }

		[CandidName("projectName")]
		public string ProjectName { get; set; }

		[CandidName("versionID")]
		public UnboundedUInt VersionID { get; set; }

		public VersionData(string blockchain, string currentVersion, string linkDapp, string projectName, UnboundedUInt versionID)
		{
			this.Blockchain = blockchain;
			this.CurrentVersion = currentVersion;
			this.LinkDapp = linkDapp;
			this.ProjectName = projectName;
			this.VersionID = versionID;
		}

		public VersionData()
		{
		}
	}
}