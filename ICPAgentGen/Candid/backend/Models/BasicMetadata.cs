using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Level = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.backend.Models
{
	public class BasicMetadata
	{
		[CandidName("damage")]
		public UnboundedUInt Damage { get; set; }

		[CandidName("health")]
		public UnboundedUInt Health { get; set; }

		[CandidName("level")]
		public UnboundedUInt Level { get; set; }

		public BasicMetadata(UnboundedUInt damage, UnboundedUInt health, UnboundedUInt level)
		{
			this.Damage = damage;
			this.Health = health;
			this.Level = level;
		}

		public BasicMetadata()
		{
		}
	}
}