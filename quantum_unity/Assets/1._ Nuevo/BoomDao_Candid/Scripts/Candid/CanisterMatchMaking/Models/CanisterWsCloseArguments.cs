using EdjCase.ICP.Candid.Mapping;
using CanisterPK.CanisterMatchMaking.Models;

namespace CanisterPK.CanisterMatchMaking.Models
{
	public class CanisterWsCloseArguments
	{
		[CandidName("client_key")]
		public ClientKey ClientKey { get; set; }

		public CanisterWsCloseArguments(ClientKey clientKey)
		{
			this.ClientKey = clientKey;
		}

		public CanisterWsCloseArguments()
		{
		}
	}
}