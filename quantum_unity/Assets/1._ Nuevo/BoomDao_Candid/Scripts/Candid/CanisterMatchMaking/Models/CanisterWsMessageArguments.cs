using EdjCase.ICP.Candid.Mapping;
using CanisterPK.CanisterMatchMaking.Models;

namespace CanisterPK.CanisterMatchMaking.Models
{
	public class CanisterWsMessageArguments
	{
		[CandidName("msg")]
		public WebsocketMessage Msg { get; set; }

		public CanisterWsMessageArguments(WebsocketMessage msg)
		{
			this.Msg = msg;
		}

		public CanisterWsMessageArguments()
		{
		}
	}
}