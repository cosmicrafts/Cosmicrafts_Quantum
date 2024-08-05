using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace CanisterPK.CanisterLogin.Models
{
	public class RPlayer
	{
		[CandidName("balance")]
		public double Balance { get; set; }

		[CandidName("id")]
		public Principal Id { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		public RPlayer(double balance, Principal id, string name)
		{
			this.Balance = balance;
			this.Id = id;
			this.Name = name;
		}

		public RPlayer()
		{
		}
	}
}