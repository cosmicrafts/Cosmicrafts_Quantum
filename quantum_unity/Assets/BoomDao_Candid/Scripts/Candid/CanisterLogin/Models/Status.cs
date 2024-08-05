using EdjCase.ICP.Candid.Mapping;

namespace CanisterPK.CanisterLogin.Models
{
	public enum Status
	{
		[CandidName("complete")]
		Complete,
		[CandidName("progress")]
		Progress,
		[CandidName("waiting")]
		Waiting
	}
}