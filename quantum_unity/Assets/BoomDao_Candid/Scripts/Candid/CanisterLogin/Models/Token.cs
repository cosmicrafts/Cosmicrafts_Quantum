using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace CanisterPK.CanisterLogin.Models
{
	public class Token
	{
		[CandidName("amount")]
		public UnboundedUInt Amount { get; set; }

		[CandidName("title")]
		public string Title { get; set; }

		public Token(UnboundedUInt amount, string title)
		{
			this.Amount = amount;
			this.Title = title;
		}

		public Token()
		{
		}
	}
}