using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Cosmicrafts.backend.Models;
using Username1 = System.String;
using Username = System.String;
using Title = System.String;
using RegistrationDate = EdjCase.ICP.Candid.Models.UnboundedInt;
using Playerid1 = EdjCase.ICP.Candid.Models.Principal;
using Level1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Level = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Description1 = System.String;
using Description = System.String;
using Avatarid1 = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.backend.Models
{
	public class Player
	{
		[CandidName("avatar")]
		public Avatarid1 Avatar { get; set; }

		[CandidName("description")]
		public Description1 Description { get; set; }

		[CandidName("elo")]
		public double Elo { get; set; }

		[CandidName("friends")]
		public List<FriendDetails> Friends { get; set; }

		[CandidName("id")]
		public Playerid1 Id { get; set; }

		[CandidName("language")]
		public string Language { get; set; }

		[CandidName("level")]
		public Level1 Level { get; set; }

		[CandidName("registrationDate")]
		public RegistrationDate RegistrationDate { get; set; }

		[CandidName("title")]
		public Title Title { get; set; }

		[CandidName("username")]
		public Username1 Username { get; set; }

		public Player(Avatarid1 avatar, Description1 description, double elo, List<FriendDetails> friends, Playerid1 id, string language, Level1 level, RegistrationDate registrationDate, Title title, Username1 username)
		{
			this.Avatar = avatar;
			this.Description = description;
			this.Elo = elo;
			this.Friends = friends;
			this.Id = id;
			this.Language = language;
			this.Level = level;
			this.RegistrationDate = registrationDate;
			this.Title = title;
			this.Username = username;
		}

		public Player()
		{
		}
	}
}