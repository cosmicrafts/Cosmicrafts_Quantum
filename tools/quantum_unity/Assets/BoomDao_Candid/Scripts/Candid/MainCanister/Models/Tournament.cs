using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;

namespace Cosmicrafts.MainCanister.Models
{
	public class Tournament
	{
		[CandidName("bracketCreated")]
		public bool BracketCreated { get; set; }

		[CandidName("expirationDate")]
		public Time ExpirationDate { get; set; }

		[CandidName("id")]
		public UnboundedUInt Id { get; set; }

		[CandidName("isActive")]
		public bool IsActive { get; set; }

		[CandidName("matchCounter")]
		public UnboundedUInt MatchCounter { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		[CandidName("participants")]
		public List<Principal> Participants { get; set; }

		[CandidName("prizePool")]
		public string PrizePool { get; set; }

		[CandidName("registeredParticipants")]
		public List<Principal> RegisteredParticipants { get; set; }

		[CandidName("startDate")]
		public Time StartDate { get; set; }

		public Tournament(bool bracketCreated, Time expirationDate, UnboundedUInt id, bool isActive, UnboundedUInt matchCounter, string name, List<Principal> participants, string prizePool, List<Principal> registeredParticipants, Time startDate)
		{
			this.BracketCreated = bracketCreated;
			this.ExpirationDate = expirationDate;
			this.Id = id;
			this.IsActive = isActive;
			this.MatchCounter = matchCounter;
			this.Name = name;
			this.Participants = participants;
			this.PrizePool = prizePool;
			this.RegisteredParticipants = registeredParticipants;
			this.StartDate = startDate;
		}

		public Tournament()
		{
		}
	}
}