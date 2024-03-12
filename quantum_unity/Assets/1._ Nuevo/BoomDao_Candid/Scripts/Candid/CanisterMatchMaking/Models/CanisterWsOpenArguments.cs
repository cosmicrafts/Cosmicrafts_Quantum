using EdjCase.ICP.Candid.Mapping;

namespace CanisterPK.CanisterMatchMaking.Models
{
	public class CanisterWsOpenArguments
	{
		[CandidName("client_nonce")]
		public ulong ClientNonce { get; set; }

		public CanisterWsOpenArguments(ulong clientNonce)
		{
			this.ClientNonce = clientNonce;
		}

		public CanisterWsOpenArguments()
		{
		}
	}
}