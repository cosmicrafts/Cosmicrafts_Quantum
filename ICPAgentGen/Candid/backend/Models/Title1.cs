using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Title = System.String;
using Description = System.String;

namespace Cosmicrafts.backend.Models
{
	public class Title1
	{
		[CandidName("description")]
		public string Description { get; set; }

		[CandidName("id")]
		public UnboundedUInt Id { get; set; }

		[CandidName("title")]
		public string Title { get; set; }

		public Title1(string description, UnboundedUInt id, string title)
		{
			this.Description = description;
			this.Id = id;
			this.Title = title;
		}

		public Title1()
		{
		}
	}
}