using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using CanisterPK.NFTsICHub.Models;

namespace CanisterPK.NFTsICHub.Models
{
	public class MintRequest
	{
		[CandidName("metadata")]
		public List<byte> Metadata { get; set; }

		[CandidName("to")]
		public User To { get; set; }

		public MintRequest(List<byte> metadata, User to)
		{
			this.Metadata = metadata;
			this.To = to;
		}

		public MintRequest()
		{
		}
	}
}