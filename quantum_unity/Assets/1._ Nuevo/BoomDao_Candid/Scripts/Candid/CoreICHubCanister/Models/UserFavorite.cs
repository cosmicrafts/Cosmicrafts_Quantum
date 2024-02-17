using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using UserID = EdjCase.ICP.Candid.Models.Principal;

namespace CanisterPK.CoreICHubCanister.Models
{
	public class UserFavorite
	{
		[CandidName("order")]
		public UnboundedUInt Order { get; set; }

		[CandidName("project")]
		public UserID Project { get; set; }

		public UserFavorite(UnboundedUInt order, UserID project)
		{
			this.Order = order;
			this.Project = project;
		}

		public UserFavorite()
		{
		}
	}
}