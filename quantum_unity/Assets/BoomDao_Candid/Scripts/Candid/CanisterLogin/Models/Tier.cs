using EdjCase.ICP.Candid.Mapping;
using CanisterPK.CanisterLogin.Models;
using TierID = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.CanisterLogin.Models
{
	public class Tier
	{
		[CandidName("desc")]
		public string Desc { get; set; }

		[CandidName("id")]
		public TierID Id { get; set; }

		[CandidName("status")]
		public string Status { get; set; }

		[CandidName("title")]
		public string Title { get; set; }

		[CandidName("token")]
		public Token Token { get; set; }

		public Tier(string desc, TierID id, string status, string title, Token token)
		{
			this.Desc = desc;
			this.Id = id;
			this.Status = status;
			this.Title = title;
			this.Token = token;
		}

		public Tier()
		{
		}
	}
}