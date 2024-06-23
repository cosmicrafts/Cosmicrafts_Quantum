using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using CanisterPK.tournaments.Models;

namespace CanisterPK.tournaments.Models
{
	public class Match
	{
		[CandidName("id")]
		public UnboundedUInt Id { get; set; }

		[CandidName("participants")]
		public List<Principal> Participants { get; set; }

		[CandidName("result")]
		public OptionalValue<Match.ResultValue> Result { get; set; }

		[CandidName("status")]
		public string Status { get; set; }

		public Match(UnboundedUInt id, List<Principal> participants, OptionalValue<Match.ResultValue> result, string status)
		{
			this.Id = id;
			this.Participants = participants;
			this.Result = result;
			this.Status = status;
		}

		public Match()
		{
		}

		public class ResultValue
		{
			[CandidName("score")]
			public string Score { get; set; }

			[CandidName("winner")]
			public Principal Winner { get; set; }

			public ResultValue(string score, Principal winner)
			{
				this.Score = score;
				this.Winner = winner;
			}

			public ResultValue()
			{
			}
		}
	}
}