using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using CanisterPK.CanisterMatchMaking.Models;

namespace CanisterPK.CanisterMatchMaking.Models
{
	public class CanisterOutputCertifiedMessages
	{
		[CandidName("cert")]
		public List<byte> Cert { get; set; }

		[CandidName("messages")]
		public List<CanisterOutputMessage> Messages { get; set; }

		[CandidName("tree")]
		public List<byte> Tree { get; set; }

		public CanisterOutputCertifiedMessages(List<byte> cert, List<CanisterOutputMessage> messages, List<byte> tree)
		{
			this.Cert = cert;
			this.Messages = messages;
			this.Tree = tree;
		}

		public CanisterOutputCertifiedMessages()
		{
		}
	}
}