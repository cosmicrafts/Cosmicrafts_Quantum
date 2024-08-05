using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using CanisterPK.CanisterLogin.Models;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;

namespace CanisterPK.CanisterLogin.Models
{
	public class RMatch
	{
		[CandidName("date")]
		public Time Date { get; set; }

		[CandidName("entry")]
		public double Entry { get; set; }

		[CandidName("fee")]
		public double Fee { get; set; }

		[CandidName("id")]
		public string Id { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		[CandidName("player1")]
		public Principal Player1 { get; set; }

		[CandidName("player2")]
		public OptionalValue<Principal> Player2 { get; set; }

		[CandidName("price")]
		public double Price { get; set; }

		[CandidName("status")]
		public Status Status { get; set; }

		[CandidName("winner")]
		public OptionalValue<Principal> Winner { get; set; }

		public RMatch(Time date, double entry, double fee, string id, string name, Principal player1, OptionalValue<Principal> player2, double price, Status status, OptionalValue<Principal> winner)
		{
			this.Date = date;
			this.Entry = entry;
			this.Fee = fee;
			this.Id = id;
			this.Name = name;
			this.Player1 = player1;
			this.Player2 = player2;
			this.Price = price;
			this.Status = status;
			this.Winner = winner;
		}

		public RMatch()
		{
		}
	}
}