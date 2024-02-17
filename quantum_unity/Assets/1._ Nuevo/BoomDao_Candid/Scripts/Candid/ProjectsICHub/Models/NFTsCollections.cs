using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;

namespace CanisterPK.ProjectsICHub.Models
{
	public class NFTsCollections
	{
		[CandidName("collections")]
		public List<string> Collections { get; set; }

		public NFTsCollections(List<string> collections)
		{
			this.Collections = collections;
		}

		public NFTsCollections()
		{
		}
	}
}