using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using Cosmicrafts.MainCanister.Models;
using EdjCase.ICP.Candid.Models;
using PlayerId = EdjCase.ICP.Candid.Models.Principal;

namespace Cosmicrafts.MainCanister.Models
{
	public class PlayerView
	{
		[CandidName("allPlayers")]
		public List<Player> AllPlayers { get; set; }

		[CandidName("friendRequests")]
		public List<FriendRequest> FriendRequests { get; set; }

		[CandidName("friendsList")]
		public OptionalValue<List<PlayerId>> FriendsList { get; set; }

		[CandidName("fullProfile")]
		public OptionalValue<(Player, PlayerGamesStats, AverageStats)> FullProfile { get; set; }

		[CandidName("notifications")]
		public List<Notification> Notifications { get; set; }

		[CandidName("privacySettings")]
		public PrivacySetting PrivacySettings { get; set; }

		public PlayerView(List<Player> allPlayers, List<FriendRequest> friendRequests, OptionalValue<List<PlayerId>> friendsList, OptionalValue<(Player, PlayerGamesStats, AverageStats)> fullProfile, List<Notification> notifications, PrivacySetting privacySettings)
		{
			this.AllPlayers = allPlayers;
			this.FriendRequests = friendRequests;
			this.FriendsList = friendsList;
			this.FullProfile = fullProfile;
			this.Notifications = notifications;
			this.PrivacySettings = privacySettings;
		}

		public PlayerView()
		{
		}
	}
}