using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Description = System.String;

namespace Cosmicrafts.MainCanister.Models
{
	public class Avatar
	{
		[CandidName("description")]
		public string Description { get; set; }

		[CandidName("id")]
		public UnboundedUInt Id { get; set; }

		public Avatar(string description, UnboundedUInt id)
		{
			this.Description = description;
			this.Id = id;
		}

		public Avatar()
		{
		}
	}
}