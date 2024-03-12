using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Candid.IcrcLedger.Models;

namespace Candid.IcrcLedger.Models
{
	public class Archives
	{
		[CandidName("archives")]
		public List<Archive> Archives_ { get; set; }

		public Archives(List<Archive> archives)
		{
			this.Archives_ = archives;
		}

		public Archives()
		{
		}
	}
}