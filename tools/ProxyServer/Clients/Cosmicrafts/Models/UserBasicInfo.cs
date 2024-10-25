using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using UserID = EdjCase.ICP.Candid.Models.Principal;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using RegistrationDate = EdjCase.ICP.Candid.Models.UnboundedInt;

namespace ProxyServer.Cosmicrafts.Models
{
	public class UserBasicInfo
	{
		[CandidName("avatarId")]
		public UnboundedUInt AvatarId { get; set; }

		[CandidName("country")]
		public OptionalValue<string> Country { get; set; }

		[CandidName("description")]
		public OptionalValue<string> Description { get; set; }

		[CandidName("id")]
		public UserID Id { get; set; }

		[CandidName("level")]
		public UnboundedUInt Level { get; set; }

		[CandidName("registrationDate")]
		public Time RegistrationDate { get; set; }

		[CandidName("title")]
		public OptionalValue<string> Title { get; set; }

		[CandidName("username")]
		public string Username { get; set; }

		[CandidName("verificationBadge")]
		public bool VerificationBadge { get; set; }

		public UserBasicInfo(UnboundedUInt avatarId, OptionalValue<string> country, OptionalValue<string> description, UserID id, UnboundedUInt level, Time registrationDate, OptionalValue<string> title, string username, bool verificationBadge)
		{
			this.AvatarId = avatarId;
			this.Country = country;
			this.Description = description;
			this.Id = id;
			this.Level = level;
			this.RegistrationDate = registrationDate;
			this.Title = title;
			this.Username = username;
			this.VerificationBadge = verificationBadge;
		}

		public UserBasicInfo()
		{
		}
	}
}