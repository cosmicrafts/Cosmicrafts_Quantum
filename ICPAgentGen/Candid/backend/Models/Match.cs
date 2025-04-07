using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using Cosmicrafts.backend.Models;

namespace Cosmicrafts.backend.Models
{
	public class Match
	{
		[CandidName("id")]
		public UnboundedUInt Id { get; set; }

		[CandidName("nextMatchId")]
		public OptionalValue<UnboundedUInt> NextMatchId { get; set; }

		[CandidName("participants")]
		public List<Principal> Participants { get; set; }

		[CandidName("result")]
		public OptionalValue<Match.ResultValue> Result { get; set; }

		[CandidName("status")]
		public string Status { get; set; }

		[CandidName("tournamentId")]
		public UnboundedUInt TournamentId { get; set; }

		public Match(UnboundedUInt id, OptionalValue<UnboundedUInt> nextMatchId, List<Principal> participants, OptionalValue<Match.ResultValue> result, string status, UnboundedUInt tournamentId)
		{
			this.Id = id;
			this.NextMatchId = nextMatchId;
			this.Participants = participants;
			this.Result = result;
			this.Status = status;
			this.TournamentId = tournamentId;
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