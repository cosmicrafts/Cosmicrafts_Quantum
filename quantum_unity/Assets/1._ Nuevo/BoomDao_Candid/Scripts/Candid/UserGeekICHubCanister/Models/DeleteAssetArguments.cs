using EdjCase.ICP.Candid.Mapping;
using Key = System.String;

namespace CanisterPK.UserGeekICHubCanister.Models
{
	public class DeleteAssetArguments
	{
		[CandidName("key")]
		public Key Key { get; set; }

		public DeleteAssetArguments(Key key)
		{
			this.Key = key;
		}

		public DeleteAssetArguments()
		{
		}
	}
}