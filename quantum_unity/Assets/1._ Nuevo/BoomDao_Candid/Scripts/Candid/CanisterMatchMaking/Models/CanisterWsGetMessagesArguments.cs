using EdjCase.ICP.Candid.Mapping;

namespace CanisterPK.CanisterMatchMaking.Models
{
	public class CanisterWsGetMessagesArguments
	{
		[CandidName("nonce")]
		public ulong Nonce { get; set; }

		public CanisterWsGetMessagesArguments(ulong nonce)
		{
			this.Nonce = nonce;
		}

		public CanisterWsGetMessagesArguments()
		{
		}
	}
}