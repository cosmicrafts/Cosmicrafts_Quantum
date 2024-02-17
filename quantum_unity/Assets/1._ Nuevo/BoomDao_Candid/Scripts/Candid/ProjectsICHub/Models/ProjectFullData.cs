using EdjCase.ICP.Candid.Mapping;
using CanisterPK.ProjectsICHub.Models;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using UserID = EdjCase.ICP.Candid.Models.Principal;

namespace CanisterPK.ProjectsICHub.Models
{
	public class ProjectFullData
	{
		[CandidName("data")]
		public ProjectData Data { get; set; }

		[CandidName("news")]
		public OptionalValue<List<ProjectNews>> News { get; set; }

		[CandidName("user")]
		public UserID User { get; set; }

		public ProjectFullData(ProjectData data, OptionalValue<List<ProjectNews>> news, UserID user)
		{
			this.Data = data;
			this.News = news;
			this.User = user;
		}

		public ProjectFullData()
		{
		}
	}
}