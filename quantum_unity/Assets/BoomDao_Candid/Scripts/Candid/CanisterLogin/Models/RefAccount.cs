using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using CanisterPK.CanisterLogin.Models;
using UUID = System.String;

namespace CanisterPK.CanisterLogin.Models
{
	public class RefAccount
	{
		[CandidName("alias")]
		public string Alias { get; set; }

		[CandidName("playerID")]
		public Principal PlayerID { get; set; }

		[CandidName("refByUUID")]
		public UUID RefByUUID { get; set; }

		[CandidName("tiers")]
		public List<Tier> Tiers { get; set; }

		[CandidName("tokens")]
		public List<Token> Tokens { get; set; }

		[CandidName("uuid")]
		public UUID Uuid { get; set; }

		public RefAccount(string alias, Principal playerID, UUID refByUUID, List<Tier> tiers, List<Token> tokens, UUID uuid)
		{
			this.Alias = alias;
			this.PlayerID = playerID;
			this.RefByUUID = refByUUID;
			this.Tiers = tiers;
			this.Tokens = tokens;
			this.Uuid = uuid;
		}

		public RefAccount()
		{
		}
	}
}