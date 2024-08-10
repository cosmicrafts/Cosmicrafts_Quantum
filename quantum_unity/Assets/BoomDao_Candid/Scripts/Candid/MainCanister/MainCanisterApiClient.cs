using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using Cosmicrafts.MainCanister;
using System.Collections.Generic;
using EdjCase.ICP.Agent.Responses;
using EdjCase.ICP.Candid.Mapping;
using Username = System.String;
using Txindex1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;
using TokenID = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using PlayerId = EdjCase.ICP.Candid.Models.Principal;
using MetaDatum = System.ValueTuple<System.String, Cosmicrafts.MainCanister.Models.Value>;
using MatchID = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Level = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Description = System.String;
using Balance1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using AvatarID = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.MainCanister
{
	public class MainCanisterApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public MainCanisterApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> AcceptFriendRequest(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "acceptFriendRequest", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> Admin(Models.AdminFunction arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "admin", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> AdminUpdateMatch(UnboundedUInt arg0, UnboundedUInt arg1, UnboundedUInt arg2, string arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "adminUpdateMatch", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task AssignAchievementsToUser(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "assignAchievementsToUser", arg);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> BlockUser(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "blockUser", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<Models.TransferResult> Burn(Models.BurnArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "burn", arg);
			return reply.ToObjects<Models.TransferResult>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> CancelMatchmaking()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "cancelMatchmaking", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> ClaimAchievementReward(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "claimAchievementReward", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> ClaimGeneralReward(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "claimGeneralReward", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> ClaimUserReward(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "claimUserReward", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1, UnboundedUInt ReturnArg2)> CreateAchievement(string arg0, List<UnboundedUInt> arg1, UnboundedUInt arg2, UnboundedUInt arg3, List<Models.AchievementReward> arg4)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter), CandidTypedValue.FromObject(arg4, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createAchievement", arg);
			return reply.ToObjects<bool, string, UnboundedUInt>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1, UnboundedUInt ReturnArg2)> CreateCategory(string arg0, List<UnboundedUInt> arg1, UnboundedUInt arg2, List<Models.AchievementReward> arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createCategory", arg);
			return reply.ToObjects<bool, string, UnboundedUInt>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1, UnboundedUInt ReturnArg2)> CreateIndividualAchievement(string arg0, Models.AchievementType arg1, UnboundedUInt arg2, List<Models.AchievementReward> arg3, UnboundedUInt arg4)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter), CandidTypedValue.FromObject(arg4, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createIndividualAchievement", arg);
			return reply.ToObjects<bool, string, UnboundedUInt>(this.Converter);
		}

		public async Task<UnboundedUInt> CreateTournament(string arg0, Time arg1, string arg2, Time arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createTournament", arg);
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1, UnboundedUInt ReturnArg2)> CreateUserMission(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createUserMission", arg);
			return reply.ToObjects<bool, string, UnboundedUInt>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> DeclineFriendRequest(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "declineFriendRequest", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> DeleteAllTournaments()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "deleteAllTournaments", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task DepositCycles()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAndWaitAsync(this.CanisterId, "deposit_cycles", arg);
		}

		public async Task<bool> DisputeMatch(UnboundedUInt arg0, UnboundedUInt arg1, string arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "disputeMatch", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<List<(Models.AchievementCategory, List<Models.Achievement>, List<Models.IndividualAchievementProgress>)>> GetAchievements()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getAchievements", arg);
			return reply.ToObjects<List<(Models.AchievementCategory, List<Models.Achievement>, List<Models.IndividualAchievementProgress>)>>(this.Converter);
		}

		public async Task<List<Models.Tournament>> GetActiveTournaments()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getActiveTournaments", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Tournament>>(this.Converter);
		}

		public async Task<List<Models.Player>> GetAllPlayers()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllPlayers", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Player>>(this.Converter);
		}

		public async Task<List<Models.MatchData>> GetAllSearching()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllSearching", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.MatchData>>(this.Converter);
		}

		public async Task<List<Models.Tournament>> GetAllTournaments()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllTournaments", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Tournament>>(this.Converter);
		}

		public async Task<MainCanisterApiClient.GetAvatarsReturnArg0> GetAvatars(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAvatars", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<MainCanisterApiClient.GetAvatarsReturnArg0>(this.Converter);
		}

		public async Task<MainCanisterApiClient.GetBlockedUsersReturnArg0> GetBlockedUsers()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getBlockedUsers", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<MainCanisterApiClient.GetBlockedUsersReturnArg0>(this.Converter);
		}

		public async Task<MainCanisterApiClient.GetCharactersReturnArg0> GetCharacters(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getCharacters", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<MainCanisterApiClient.GetCharactersReturnArg0>(this.Converter);
		}

		public async Task<MainCanisterApiClient.GetChestsReturnArg0> GetChests(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getChests", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<MainCanisterApiClient.GetChestsReturnArg0>(this.Converter);
		}

		public async Task<Models.OverallStats> GetCosmicraftsStats()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getCosmicraftsStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.OverallStats>(this.Converter);
		}

		public async Task<List<Models.FriendRequest>> GetFriendRequests()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getFriendRequests", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.FriendRequest>>(this.Converter);
		}

		public async Task<MainCanisterApiClient.GetFriendsListReturnArg0> GetFriendsList()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getFriendsList", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<MainCanisterApiClient.GetFriendsListReturnArg0>(this.Converter);
		}

		public async Task<OptionalValue<(Models.Player, Models.PlayerGamesStats, Models.AverageStats)>> GetFullUserProfile(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getFullUserProfile", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<(Models.Player, Models.PlayerGamesStats, Models.AverageStats)>>(this.Converter);
		}

		public async Task<OptionalValue<Models.MissionsUser>> GetGeneralMissionProgress(Principal arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getGeneralMissionProgress", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.MissionsUser>>(this.Converter);
		}

		public async Task<List<Models.MissionsUser>> GetGeneralMissions()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getGeneralMissions", arg);
			return reply.ToObjects<List<Models.MissionsUser>>(this.Converter);
		}

		public async Task<List<Models.Tournament>> GetInactiveTournaments()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getInactiveTournaments", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Tournament>>(this.Converter);
		}

		public async Task<Models.TokenInitArgs> GetInitArgs()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getInitArgs", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.TokenInitArgs>(this.Converter);
		}

		public async Task<OptionalValue<(Models.MatchData, Dictionary<Models.Player, Models.PlayerGamesStats>)>> GetMatchDetails(MatchID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMatchDetails", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<(Models.MatchData, Dictionary<Models.Player, Models.PlayerGamesStats>)>>(this.Converter);
		}

		public async Task<MainCanisterApiClient.GetMatchHistoryByPrincipalReturnArg0> GetMatchHistoryByPrincipal(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMatchHistoryByPrincipal", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<MainCanisterApiClient.GetMatchHistoryByPrincipalReturnArg0>(this.Converter);
		}

		public async Task<MainCanisterApiClient.GetMatchIDsByPrincipalReturnArg0> GetMatchIDsByPrincipal(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMatchIDsByPrincipal", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<MainCanisterApiClient.GetMatchIDsByPrincipalReturnArg0>(this.Converter);
		}

		public async Task<OptionalValue<(Principal, OptionalValue<Principal>)>> GetMatchParticipants(MatchID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMatchParticipants", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<(Principal, OptionalValue<Principal>)>>(this.Converter);
		}

		public async Task<(Models.MMSearchStatus ReturnArg0, UnboundedUInt ReturnArg1, string ReturnArg2)> GetMatchSearching()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getMatchSearching", arg);
			return reply.ToObjects<Models.MMSearchStatus, UnboundedUInt, string>(this.Converter);
		}

		public async Task<OptionalValue<Models.BasicStats>> GetMatchStats(MatchID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMatchStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.BasicStats>>(this.Converter);
		}

		public async Task<MainCanisterApiClient.GetMintedInfoReturnArg0> GetMintedInfo(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMintedInfo", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<MainCanisterApiClient.GetMintedInfoReturnArg0>(this.Converter);
		}

		public async Task<(OptionalValue<Models.FullMatchData> ReturnArg0, UnboundedUInt ReturnArg1)> GetMyMatchData()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMyMatchData", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.FullMatchData>, UnboundedUInt>(this.Converter);
		}

		public async Task<Models.PrivacySetting> GetMyPrivacySettings()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMyPrivacySettings", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.PrivacySetting>(this.Converter);
		}

		public async Task<OptionalValue<Models.PlayerGamesStats>> GetMyStats()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMyStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.PlayerGamesStats>>(this.Converter);
		}

		public async Task<MainCanisterApiClient.GetNFTsReturnArg0> GetNFTs(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getNFTs", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<MainCanisterApiClient.GetNFTsReturnArg0>(this.Converter);
		}

		public async Task<List<Models.Notification>> GetNotifications()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getNotifications", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Notification>>(this.Converter);
		}

		public async Task<OptionalValue<Models.Player>> GetPlayer()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getPlayer", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Player>>(this.Converter);
		}

		public async Task<OptionalValue<Models.AverageStats>> GetPlayerAverageStats(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getPlayerAverageStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.AverageStats>>(this.Converter);
		}

		public async Task<MainCanisterApiClient.GetPlayerDeckReturnArg0> GetPlayerDeck(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getPlayerDeck", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<MainCanisterApiClient.GetPlayerDeckReturnArg0>(this.Converter);
		}

		public async Task<double> GetPlayerElo(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getPlayerElo", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<double>(this.Converter);
		}

		public async Task<OptionalValue<Models.PlayerGamesStats>> GetPlayerStats(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getPlayerStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.PlayerGamesStats>>(this.Converter);
		}

		public async Task<OptionalValue<Models.Player>> GetProfile(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getProfile", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Player>>(this.Converter);
		}

		public async Task<List<Principal>> GetRegisteredUsers(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getRegisteredUsers", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Principal>>(this.Converter);
		}

		public async Task<MainCanisterApiClient.GetTournamentBracketReturnArg0> GetTournamentBracket(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTournamentBracket", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<MainCanisterApiClient.GetTournamentBracketReturnArg0>(this.Converter);
		}

		public async Task<List<Models.LogEntry>> GetTransactionLogs(Principal arg0, Models.ItemType arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTransactionLogs", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.LogEntry>>(this.Converter);
		}

		public async Task<MainCanisterApiClient.GetTrophiesReturnArg0> GetTrophies(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTrophies", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<MainCanisterApiClient.GetTrophiesReturnArg0>(this.Converter);
		}

		public async Task<MainCanisterApiClient.GetUnitsReturnArg0> GetUnits(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUnits", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<MainCanisterApiClient.GetUnitsReturnArg0>(this.Converter);
		}

		public async Task<OptionalValue<Models.MissionsUser>> GetUserMissionProgress(PlayerId arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserMissionProgress", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.MissionsUser>>(this.Converter);
		}

		public async Task<List<Models.MissionsUser>> GetUserMissions()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getUserMissions", arg);
			return reply.ToObjects<List<Models.MissionsUser>>(this.Converter);
		}

		public async Task<Models.Account> GetCollectionOwner()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get_collection_owner", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.Account>(this.Converter);
		}

		public async Task<OptionalValue<Models.Transaction2>> GetTransaction(Txindex1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "get_transaction", arg);
			return reply.ToObjects<OptionalValue<Models.Transaction2>>(this.Converter);
		}

		public async Task<Models.GetTransactionsResponse> GetTransactions(Models.GetTransactionsRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get_transactions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.GetTransactionsResponse>(this.Converter);
		}

		public async Task<MainCanisterApiClient.HandleCombatXPReturnArg0> HandleCombatXP(MainCanisterApiClient.HandleCombatXPArg0 arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "handleCombatXP", arg);
			return reply.ToObjects<MainCanisterApiClient.HandleCombatXPReturnArg0>(this.Converter);
		}

		public async Task<Balance1> Icrc1BalanceOf(Models.Account2 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_balance_of", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Balance1>(this.Converter);
		}

		public async Task<byte> Icrc1Decimals()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_decimals", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<byte>(this.Converter);
		}

		public async Task<Balance1> Icrc1Fee()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_fee", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Balance1>(this.Converter);
		}

		public async Task<MainCanisterApiClient.Icrc1MetadataReturnArg0> Icrc1Metadata()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_metadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<MainCanisterApiClient.Icrc1MetadataReturnArg0>(this.Converter);
		}

		public async Task<OptionalValue<Models.Account2>> Icrc1MintingAccount()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_minting_account", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Account2>>(this.Converter);
		}

		public async Task<string> Icrc1Name()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_name", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task<Models.TransferResult> Icrc1PayForTransaction(Models.Transferargs1 arg0, Principal arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "icrc1_pay_for_transaction", arg);
			return reply.ToObjects<Models.TransferResult>(this.Converter);
		}

		public async Task<List<Models.Supportedstandard1>> Icrc1SupportedStandards()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_supported_standards", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Supportedstandard1>>(this.Converter);
		}

		public async Task<string> Icrc1Symbol()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_symbol", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task<Balance1> Icrc1TotalSupply()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_total_supply", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Balance1>(this.Converter);
		}

		public async Task<Models.TransferResult> Icrc1Transfer(Models.Transferargs1 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "icrc1_transfer", arg);
			return reply.ToObjects<Models.TransferResult>(this.Converter);
		}

		public async Task<Models.ApprovalReceipt> Icrc7Approve(Models.ApprovalArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "icrc7_approve", arg);
			return reply.ToObjects<Models.ApprovalReceipt>(this.Converter);
		}

		public async Task<Models.BalanceResult> Icrc7BalanceOf(Models.Account arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_balance_of", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.BalanceResult>(this.Converter);
		}

		public async Task<Models.CollectionMetadata> Icrc7CollectionMetadata()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_collection_metadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.CollectionMetadata>(this.Converter);
		}

		public async Task<OptionalValue<string>> Icrc7Description()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_description", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<string>>(this.Converter);
		}

		public async Task<Models.GetTransactionsResult> Icrc7GetTransactions(Models.GetTransactionsArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "icrc7_get_transactions", arg);
			return reply.ToObjects<Models.GetTransactionsResult>(this.Converter);
		}

		public async Task<OptionalValue<List<byte>>> Icrc7Image()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_image", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<byte>>>(this.Converter);
		}

		public async Task<Models.MetadataResult> Icrc7Metadata(TokenId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_metadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.MetadataResult>(this.Converter);
		}

		public async Task<string> Icrc7Name()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_name", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task<Models.OwnerResult> Icrc7OwnerOf(TokenId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_owner_of", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.OwnerResult>(this.Converter);
		}

		public async Task<OptionalValue<ushort>> Icrc7Royalties()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_royalties", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<ushort>>(this.Converter);
		}

		public async Task<OptionalValue<Models.Account>> Icrc7RoyaltyRecipient()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_royalty_recipient", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.Account>>(this.Converter);
		}

		public async Task<OptionalValue<UnboundedUInt>> Icrc7SupplyCap()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_supply_cap", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<UnboundedUInt>>(this.Converter);
		}

		public async Task<List<Models.SupportedStandard>> Icrc7SupportedStandards()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_supported_standards", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.SupportedStandard>>(this.Converter);
		}

		public async Task<string> Icrc7Symbol()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_symbol", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task<Models.TokensOfResult> Icrc7TokensOf(Models.Account arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_tokens_of", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.TokensOfResult>(this.Converter);
		}

		public async Task<UnboundedUInt> Icrc7TotalSupply()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc7_total_supply", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async Task<Models.TransferReceipt> Icrc7Transfer(Models.TransferArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "icrc7_transfer", arg);
			return reply.ToObjects<Models.TransferReceipt>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> IsGameMatched()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "isGameMatched", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> JoinTournament(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "joinTournament", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<Models.TransferResult> Mint(Models.Mint arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "mint", arg);
			return reply.ToObjects<Models.TransferResult>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> MintChest(Principal arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "mintChest", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1, MainCanisterApiClient.MintDeckReturnArg2 ReturnArg2)> MintDeck()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "mintDeck", arg);
			return reply.ToObjects<bool, string, MainCanisterApiClient.MintDeckReturnArg2>(this.Converter);
		}

		public async Task<Models.MintReceipt> MintNFT(Models.MintArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "mintNFT", arg);
			return reply.ToObjects<Models.MintReceipt>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> OpenChest(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "openChest", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<OptionalValue<Models.RefAccount>> RefAccount()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ref_account", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.RefAccount>>(this.Converter);
		}

		public async Task<Dictionary<string, Principal>> RefAccountAll()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ref_account_all", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Dictionary<string, Principal>>(this.Converter);
		}

		public async Task<OptionalValue<Models.RefAccount>> RefAccountBy(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ref_account_by", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.RefAccount>>(this.Converter);
		}

		public async Task<OptionalValue<Models.RefAccView>> RefAccountView(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "ref_account_view", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.RefAccView>>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> RefClaimTier(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ref_claim_tier", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> RefClaimTop(Principal arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ref_claim_top", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> RefEnroll(OptionalValue<string> arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ref_enroll", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> RefEnrollBy(OptionalValue<string> arg0, Principal arg1, string arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ref_enroll_by", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<Principal> RefIdGen()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "ref_id_gen", arg);
			return reply.ToObjects<Principal>(this.Converter);
		}

		public async Task<(bool ReturnArg0, OptionalValue<Models.Player> ReturnArg1, string ReturnArg2)> RegisterPlayer(Username arg0, AvatarID arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "registerPlayer", arg);
			return reply.ToObjects<bool, OptionalValue<Models.Player>, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> SaveFinishedGame(MatchID arg0, MainCanisterApiClient.SaveFinishedGameArg1 arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "saveFinishedGame", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<List<(Models.AchievementCategory, List<Models.Achievement>, List<Models.IndividualAchievementProgress>)>> SearchActiveAchievements(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "searchActiveAchievements", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<(Models.AchievementCategory, List<Models.Achievement>, List<Models.IndividualAchievementProgress>)>>(this.Converter);
		}

		public async Task<List<Models.MissionsUser>> SearchActiveGeneralMissions(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "searchActiveGeneralMissions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.MissionsUser>>(this.Converter);
		}

		public async Task<List<Models.MissionsUser>> SearchActiveUserMissions(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "searchActiveUserMissions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.MissionsUser>>(this.Converter);
		}

		public async Task<List<Models.Player>> SearchUserByUsername(Username arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "searchUserByUsername", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Player>>(this.Converter);
		}

		public async Task<MainCanisterApiClient.SelectRandomUnitsReturnArg0> SelectRandomUnits(MainCanisterApiClient.SelectRandomUnitsArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "selectRandomUnits", arg);
			return reply.ToObjects<MainCanisterApiClient.SelectRandomUnitsReturnArg0>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> SendFriendRequest(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "sendFriendRequest", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> SetPlayerActive()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "setPlayerActive", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> SetPrivacySetting(Models.PrivacySetting arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "setPrivacySetting", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> StoreCurrentDeck(MainCanisterApiClient.StoreCurrentDeckArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "storeCurrentDeck", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> SubmitFeedback(UnboundedUInt arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "submitFeedback", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> SubmitMatchResult(UnboundedUInt arg0, UnboundedUInt arg1, string arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "submitMatchResult", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<MainCanisterApiClient.TestReturnArg0> Test(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "test", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<MainCanisterApiClient.TestReturnArg0>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UnblockUser(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "unblockUser", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<List<(Models.AchievementCategory, List<Models.Achievement>, List<Models.IndividualAchievementProgress>)>> UpdateAndGetAchievements()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateAndGetAchievements", arg);
			return reply.ToObjects<List<(Models.AchievementCategory, List<Models.Achievement>, List<Models.IndividualAchievementProgress>)>>(this.Converter);
		}

		public async Task<(bool ReturnArg0, PlayerId ReturnArg1, string ReturnArg2)> UpdateAvatar(AvatarID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateAvatar", arg);
			return reply.ToObjects<bool, PlayerId, string>(this.Converter);
		}

		public async Task<bool> UpdateBracket(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateBracket", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task UpdateBracketAfterMatchUpdate(UnboundedUInt arg0, UnboundedUInt arg1, Principal arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "updateBracketAfterMatchUpdate", arg);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UpdateCategoryProgress(PlayerId arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateCategoryProgress", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, PlayerId ReturnArg1, string ReturnArg2)> UpdateDescription(Description arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateDescription", arg);
			return reply.ToObjects<bool, PlayerId, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UpdateGeneralAchievementProgress(PlayerId arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateGeneralAchievementProgress", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UpdateIndividualAchievementProgress(PlayerId arg0, List<Models.AchievementProgress> arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateIndividualAchievementProgress", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<MainCanisterApiClient.UpdateSoulNFTPlayedReturnArg0> UpdateSoulNFTPlayed(MainCanisterApiClient.UpdateSoulNFTPlayedArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateSoulNFTPlayed", arg);
			return reply.ToObjects<MainCanisterApiClient.UpdateSoulNFTPlayedReturnArg0>(this.Converter);
		}

		public async Task<(bool ReturnArg0, PlayerId ReturnArg1, string ReturnArg2)> UpdateUsername(Username arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateUsername", arg);
			return reply.ToObjects<bool, PlayerId, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UpgradeNFT(TokenID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "upgradeNFT", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public class GetAvatarsReturnArg0 : List<MainCanisterApiClient.GetAvatarsReturnArg0.GetAvatarsReturnArg0Element>
		{
			public GetAvatarsReturnArg0()
			{
			}

			public class GetAvatarsReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenId F0 { get; set; }

				[CandidTag(1U)]
				public Models.TokenMetadata F1 { get; set; }

				public GetAvatarsReturnArg0Element(TokenId f0, Models.TokenMetadata f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetAvatarsReturnArg0Element()
				{
				}
			}
		}

		public class GetBlockedUsersReturnArg0 : List<PlayerId>
		{
			public GetBlockedUsersReturnArg0()
			{
			}
		}

		public class GetCharactersReturnArg0 : List<MainCanisterApiClient.GetCharactersReturnArg0.GetCharactersReturnArg0Element>
		{
			public GetCharactersReturnArg0()
			{
			}

			public class GetCharactersReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenId F0 { get; set; }

				[CandidTag(1U)]
				public Models.TokenMetadata F1 { get; set; }

				public GetCharactersReturnArg0Element(TokenId f0, Models.TokenMetadata f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetCharactersReturnArg0Element()
				{
				}
			}
		}

		public class GetChestsReturnArg0 : List<MainCanisterApiClient.GetChestsReturnArg0.GetChestsReturnArg0Element>
		{
			public GetChestsReturnArg0()
			{
			}

			public class GetChestsReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenId F0 { get; set; }

				[CandidTag(1U)]
				public Models.TokenMetadata F1 { get; set; }

				public GetChestsReturnArg0Element(TokenId f0, Models.TokenMetadata f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetChestsReturnArg0Element()
				{
				}
			}
		}

		public class GetFriendsListReturnArg0 : OptionalValue<MainCanisterApiClient.GetFriendsListReturnArg0.GetFriendsListReturnArg0Value>
		{
			public GetFriendsListReturnArg0()
			{
			}

			public GetFriendsListReturnArg0(MainCanisterApiClient.GetFriendsListReturnArg0.GetFriendsListReturnArg0Value value) : base(value)
			{
			}

			public class GetFriendsListReturnArg0Value : List<PlayerId>
			{
				public GetFriendsListReturnArg0Value()
				{
				}
			}
		}

		public class GetMatchHistoryByPrincipalReturnArg0 : List<MainCanisterApiClient.GetMatchHistoryByPrincipalReturnArg0.GetMatchHistoryByPrincipalReturnArg0Element>
		{
			public GetMatchHistoryByPrincipalReturnArg0()
			{
			}

			public class GetMatchHistoryByPrincipalReturnArg0Element
			{
				[CandidTag(0U)]
				public MatchID F0 { get; set; }

				[CandidTag(1U)]
				public OptionalValue<Models.BasicStats> F1 { get; set; }

				public GetMatchHistoryByPrincipalReturnArg0Element(MatchID f0, OptionalValue<Models.BasicStats> f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetMatchHistoryByPrincipalReturnArg0Element()
				{
				}
			}
		}

		public class GetMatchIDsByPrincipalReturnArg0 : List<MatchID>
		{
			public GetMatchIDsByPrincipalReturnArg0()
			{
			}
		}

		public class GetMintedInfoReturnArg0
		{
			[CandidName("chests")]
			public MainCanisterApiClient.GetMintedInfoReturnArg0.ChestsInfo Chests { get; set; }

			[CandidName("gameNFTs")]
			public MainCanisterApiClient.GetMintedInfoReturnArg0.GameNFTsInfo GameNFTs { get; set; }

			[CandidName("stardust")]
			public UnboundedUInt Stardust { get; set; }

			public GetMintedInfoReturnArg0(MainCanisterApiClient.GetMintedInfoReturnArg0.ChestsInfo chests, MainCanisterApiClient.GetMintedInfoReturnArg0.GameNFTsInfo gameNFTs, UnboundedUInt stardust)
			{
				this.Chests = chests;
				this.GameNFTs = gameNFTs;
				this.Stardust = stardust;
			}

			public GetMintedInfoReturnArg0()
			{
			}

			public class ChestsInfo
			{
				[CandidName("quantity")]
				public UnboundedUInt Quantity { get; set; }

				[CandidName("tokenIDs")]
				public MainCanisterApiClient.GetMintedInfoReturnArg0.ChestsInfo.TokenIDsInfo TokenIDs { get; set; }

				public ChestsInfo(UnboundedUInt quantity, MainCanisterApiClient.GetMintedInfoReturnArg0.ChestsInfo.TokenIDsInfo tokenIDs)
				{
					this.Quantity = quantity;
					this.TokenIDs = tokenIDs;
				}

				public ChestsInfo()
				{
				}

				public class TokenIDsInfo : List<TokenID>
				{
					public TokenIDsInfo()
					{
					}
				}
			}

			public class GameNFTsInfo
			{
				[CandidName("quantity")]
				public UnboundedUInt Quantity { get; set; }

				[CandidName("tokenIDs")]
				public MainCanisterApiClient.GetMintedInfoReturnArg0.GameNFTsInfo.TokenIDsInfo TokenIDs { get; set; }

				public GameNFTsInfo(UnboundedUInt quantity, MainCanisterApiClient.GetMintedInfoReturnArg0.GameNFTsInfo.TokenIDsInfo tokenIDs)
				{
					this.Quantity = quantity;
					this.TokenIDs = tokenIDs;
				}

				public GameNFTsInfo()
				{
				}

				public class TokenIDsInfo : List<TokenID>
				{
					public TokenIDsInfo()
					{
					}
				}
			}
		}

		public class GetNFTsReturnArg0 : List<MainCanisterApiClient.GetNFTsReturnArg0.GetNFTsReturnArg0Element>
		{
			public GetNFTsReturnArg0()
			{
			}

			public class GetNFTsReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenId F0 { get; set; }

				[CandidTag(1U)]
				public Models.TokenMetadata F1 { get; set; }

				public GetNFTsReturnArg0Element(TokenId f0, Models.TokenMetadata f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetNFTsReturnArg0Element()
				{
				}
			}
		}

		public class GetPlayerDeckReturnArg0 : OptionalValue<MainCanisterApiClient.GetPlayerDeckReturnArg0.GetPlayerDeckReturnArg0Value>
		{
			public GetPlayerDeckReturnArg0()
			{
			}

			public GetPlayerDeckReturnArg0(MainCanisterApiClient.GetPlayerDeckReturnArg0.GetPlayerDeckReturnArg0Value value) : base(value)
			{
			}

			public class GetPlayerDeckReturnArg0Value : List<TokenId>
			{
				public GetPlayerDeckReturnArg0Value()
				{
				}
			}
		}

		public class GetTournamentBracketReturnArg0
		{
			[CandidName("matches")]
			public List<Models.Match> Matches { get; set; }

			public GetTournamentBracketReturnArg0(List<Models.Match> matches)
			{
				this.Matches = matches;
			}

			public GetTournamentBracketReturnArg0()
			{
			}
		}

		public class GetTrophiesReturnArg0 : List<MainCanisterApiClient.GetTrophiesReturnArg0.GetTrophiesReturnArg0Element>
		{
			public GetTrophiesReturnArg0()
			{
			}

			public class GetTrophiesReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenId F0 { get; set; }

				[CandidTag(1U)]
				public Models.TokenMetadata F1 { get; set; }

				public GetTrophiesReturnArg0Element(TokenId f0, Models.TokenMetadata f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetTrophiesReturnArg0Element()
				{
				}
			}
		}

		public class GetUnitsReturnArg0 : List<MainCanisterApiClient.GetUnitsReturnArg0.GetUnitsReturnArg0Element>
		{
			public GetUnitsReturnArg0()
			{
			}

			public class GetUnitsReturnArg0Element
			{
				[CandidTag(0U)]
				public TokenId F0 { get; set; }

				[CandidTag(1U)]
				public Models.TokenMetadata F1 { get; set; }

				public GetUnitsReturnArg0Element(TokenId f0, Models.TokenMetadata f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetUnitsReturnArg0Element()
				{
				}
			}
		}

		public class HandleCombatXPArg0 : List<TokenId>
		{
			public HandleCombatXPArg0()
			{
			}
		}

		public class HandleCombatXPReturnArg0 : List<TokenId>
		{
			public HandleCombatXPReturnArg0()
			{
			}
		}

		public class Icrc1MetadataReturnArg0 : List<MetaDatum>
		{
			public Icrc1MetadataReturnArg0()
			{
			}
		}

		public class MintDeckReturnArg2 : List<TokenId>
		{
			public MintDeckReturnArg2()
			{
			}
		}

		public class SaveFinishedGameArg1
		{
			[CandidName("botDifficulty")]
			public UnboundedUInt BotDifficulty { get; set; }

			[CandidName("botMode")]
			public UnboundedUInt BotMode { get; set; }

			[CandidName("characterID")]
			public UnboundedUInt CharacterID { get; set; }

			[CandidName("damageCritic")]
			public UnboundedUInt DamageCritic { get; set; }

			[CandidName("damageDealt")]
			public UnboundedUInt DamageDealt { get; set; }

			[CandidName("damageEvaded")]
			public UnboundedUInt DamageEvaded { get; set; }

			[CandidName("damageTaken")]
			public UnboundedUInt DamageTaken { get; set; }

			[CandidName("deploys")]
			public UnboundedUInt Deploys { get; set; }

			[CandidName("energyChargeRate")]
			public UnboundedUInt EnergyChargeRate { get; set; }

			[CandidName("energyGenerated")]
			public UnboundedUInt EnergyGenerated { get; set; }

			[CandidName("energyUsed")]
			public UnboundedUInt EnergyUsed { get; set; }

			[CandidName("energyWasted")]
			public UnboundedUInt EnergyWasted { get; set; }

			[CandidName("faction")]
			public UnboundedUInt Faction { get; set; }

			[CandidName("gameMode")]
			public UnboundedUInt GameMode { get; set; }

			[CandidName("kills")]
			public UnboundedUInt Kills { get; set; }

			[CandidName("secRemaining")]
			public UnboundedUInt SecRemaining { get; set; }

			[CandidName("wonGame")]
			public bool WonGame { get; set; }

			[CandidName("xpEarned")]
			public UnboundedUInt XpEarned { get; set; }

			public SaveFinishedGameArg1(UnboundedUInt botDifficulty, UnboundedUInt botMode, UnboundedUInt characterID, UnboundedUInt damageCritic, UnboundedUInt damageDealt, UnboundedUInt damageEvaded, UnboundedUInt damageTaken, UnboundedUInt deploys, UnboundedUInt energyChargeRate, UnboundedUInt energyGenerated, UnboundedUInt energyUsed, UnboundedUInt energyWasted, UnboundedUInt faction, UnboundedUInt gameMode, UnboundedUInt kills, UnboundedUInt secRemaining, bool wonGame, UnboundedUInt xpEarned)
			{
				this.BotDifficulty = botDifficulty;
				this.BotMode = botMode;
				this.CharacterID = characterID;
				this.DamageCritic = damageCritic;
				this.DamageDealt = damageDealt;
				this.DamageEvaded = damageEvaded;
				this.DamageTaken = damageTaken;
				this.Deploys = deploys;
				this.EnergyChargeRate = energyChargeRate;
				this.EnergyGenerated = energyGenerated;
				this.EnergyUsed = energyUsed;
				this.EnergyWasted = energyWasted;
				this.Faction = faction;
				this.GameMode = gameMode;
				this.Kills = kills;
				this.SecRemaining = secRemaining;
				this.WonGame = wonGame;
				this.XpEarned = xpEarned;
			}

			public SaveFinishedGameArg1()
			{
			}
		}

		public class SelectRandomUnitsArg0 : List<TokenId>
		{
			public SelectRandomUnitsArg0()
			{
			}
		}

		public class SelectRandomUnitsReturnArg0 : List<TokenId>
		{
			public SelectRandomUnitsReturnArg0()
			{
			}
		}

		public class StoreCurrentDeckArg0 : List<TokenId>
		{
			public StoreCurrentDeckArg0()
			{
			}
		}

		public class TestReturnArg0 : OptionalValue<MainCanisterApiClient.TestReturnArg0.TestReturnArg0Value>
		{
			public TestReturnArg0()
			{
			}

			public TestReturnArg0(MainCanisterApiClient.TestReturnArg0.TestReturnArg0Value value) : base(value)
			{
			}

			public class TestReturnArg0Value
			{
				[CandidName("elo")]
				public double Elo { get; set; }

				[CandidName("gamesLost")]
				public UnboundedUInt GamesLost { get; set; }

				[CandidName("gamesWon")]
				public UnboundedUInt GamesWon { get; set; }

				[CandidName("level")]
				public Level Level { get; set; }

				[CandidName("username")]
				public Username Username { get; set; }

				[CandidName("xp")]
				public UnboundedUInt Xp { get; set; }

				public TestReturnArg0Value(double elo, UnboundedUInt gamesLost, UnboundedUInt gamesWon, Level level, Username username, UnboundedUInt xp)
				{
					this.Elo = elo;
					this.GamesLost = gamesLost;
					this.GamesWon = gamesWon;
					this.Level = level;
					this.Username = username;
					this.Xp = xp;
				}

				public TestReturnArg0Value()
				{
				}
			}
		}

		public class UpdateSoulNFTPlayedArg0 : List<TokenId>
		{
			public UpdateSoulNFTPlayedArg0()
			{
			}
		}

		public class UpdateSoulNFTPlayedReturnArg0 : List<TokenId>
		{
			public UpdateSoulNFTPlayedReturnArg0()
			{
			}
		}
	}
}