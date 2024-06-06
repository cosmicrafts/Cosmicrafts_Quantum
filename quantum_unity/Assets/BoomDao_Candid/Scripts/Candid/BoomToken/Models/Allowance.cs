using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using CanisterPK.BoomToken.Models;
using Timestamp = System.UInt64;

namespace CanisterPK.BoomToken.Models
{
	public class Allowance
	{
		[CandidName("allowance")]
		public UnboundedUInt Allowance_ { get; set; }

		[CandidName("expires_at")]
		public Allowance.ExpiresAtInfo ExpiresAt { get; set; }

		public Allowance(UnboundedUInt allowance, Allowance.ExpiresAtInfo expiresAt)
		{
			this.Allowance_ = allowance;
			this.ExpiresAt = expiresAt;
		}

		public Allowance()
		{
		}

		public class ExpiresAtInfo : OptionalValue<Timestamp>
		{
			public ExpiresAtInfo()
			{
			}

			public ExpiresAtInfo(Timestamp value) : base(value)
			{
			}
		}
	}
}