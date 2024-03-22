using EdjCase.ICP.Candid.Mapping;
using Boom.BoomToken.Models;
using EdjCase.ICP.Candid.Models;
using Timestamp = System.UInt64;
using Tokens = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Boom.BoomToken.Models
{
	public class Allowance
	{
		[CandidName("allowance")]
		public Tokens Allowance_ { get; set; }

		[CandidName("expires_at")]
		public Allowance.ExpiresAtInfo ExpiresAt { get; set; }

		public Allowance(Tokens allowance, Allowance.ExpiresAtInfo expiresAt)
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