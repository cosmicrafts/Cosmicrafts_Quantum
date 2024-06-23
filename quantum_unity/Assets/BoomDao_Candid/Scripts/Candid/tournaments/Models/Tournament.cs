using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;

namespace CanisterPK.tournaments.Models
{
	public class Tournament
	{
		[CandidName("expirationDate")]
		public Time ExpirationDate { get; set; }

		[CandidName("id")]
		public UnboundedUInt Id { get; set; }

		[CandidName("isActive")]
		public bool IsActive { get; set; }

		[CandidName("name")]
		public string Name { get; set; }

		[CandidName("participants")]
		public List<Principal> Participants { get; set; }

		[CandidName("prizePool")]
		public string PrizePool { get; set; }

		[CandidName("startDate")]
		public Time StartDate { get; set; }

		public Tournament(Time expirationDate, UnboundedUInt id, bool isActive, string name, List<Principal> participants, string prizePool, Time startDate)
		{
			this.ExpirationDate = expirationDate;
			this.Id = id;
			this.IsActive = isActive;
			this.Name = name;
			this.Participants = participants;
			this.PrizePool = prizePool;
			this.StartDate = startDate;
		}

		public Tournament()
		{
		}
	}
}