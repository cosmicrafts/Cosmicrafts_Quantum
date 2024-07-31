using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using CanisterPK.CanisterLogin;
using System.Collections.Generic;
using EdjCase.ICP.Agent.Responses;
using EdjCase.ICP.Candid.Mapping;
using Username = System.String;
using TokenID = EdjCase.ICP.Candid.Models.UnboundedUInt;
using PlayerId = EdjCase.ICP.Candid.Models.Principal;
using MatchID = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Level = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Description = System.String;
using AvatarID = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace CanisterPK.CanisterLogin
{
	public class CanisterLoginApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public CanisterLoginApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> AddFriend(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "addFriend", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> AdminManagement(Models.AdminFunction arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "adminManagement", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task AssignAchievementsToUser(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "assignAchievementsToUser", arg);
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

		public async Task<(bool ReturnArg0, string ReturnArg1, UnboundedUInt ReturnArg2)> CreateUserMission(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createUserMission", arg);
			return reply.ToObjects<bool, string, UnboundedUInt>(this.Converter);
		}

		public async Task<List<(Models.AchievementCategory, List<Models.Achievement>, List<Models.IndividualAchievementProgress>)>> GetAchievements()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getAchievements", arg);
			return reply.ToObjects<List<(Models.AchievementCategory, List<Models.Achievement>, List<Models.IndividualAchievementProgress>)>>(this.Converter);
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

		public async Task<Models.OverallStats> GetCosmicraftsStats()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getCosmicraftsStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.OverallStats>(this.Converter);
		}

		public async Task<CanisterLoginApiClient.GetFriendsListReturnArg0> GetFriendsList()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getFriendsList", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<CanisterLoginApiClient.GetFriendsListReturnArg0>(this.Converter);
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

		public async Task<OptionalValue<(Models.MatchData, Dictionary<Models.Player, Models.PlayerGamesStats>)>> GetMatchDetails(MatchID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMatchDetails", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<(Models.MatchData, Dictionary<Models.Player, Models.PlayerGamesStats>)>>(this.Converter);
		}

		public async Task<CanisterLoginApiClient.GetMatchHistoryByPrincipalReturnArg0> GetMatchHistoryByPrincipal(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMatchHistoryByPrincipal", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<CanisterLoginApiClient.GetMatchHistoryByPrincipalReturnArg0>(this.Converter);
		}

		public async Task<CanisterLoginApiClient.GetMatchIDsByPrincipalReturnArg0> GetMatchIDsByPrincipal(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMatchIDsByPrincipal", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<CanisterLoginApiClient.GetMatchIDsByPrincipalReturnArg0>(this.Converter);
		}

		public async Task<OptionalValue<(Principal, OptionalValue<Principal>)>> GetMatchParticipants(MatchID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMatchParticipants", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<(Principal, OptionalValue<Principal>)>>(this.Converter);
		}

		public async Task<(Models.MMSearchStatus ReturnArg0, UnboundedUInt ReturnArg1, string ReturnArg2)> GetMatchSearching(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
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

		public async Task<CanisterLoginApiClient.GetMintedInfoReturnArg0> GetMintedInfo(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMintedInfo", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<CanisterLoginApiClient.GetMintedInfoReturnArg0>(this.Converter);
		}

		public async Task<(OptionalValue<Models.FullMatchData> ReturnArg0, UnboundedUInt ReturnArg1)> GetMyMatchData()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMyMatchData", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.FullMatchData>, UnboundedUInt>(this.Converter);
		}

		public async Task<OptionalValue<Models.Player>> GetMyProfile()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getMyProfile", arg);
			return reply.ToObjects<OptionalValue<Models.Player>>(this.Converter);
		}

		public async Task<OptionalValue<Models.PlayerGamesStats>> GetMyStats()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMyStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.PlayerGamesStats>>(this.Converter);
		}

		public async Task<OptionalValue<Models.Player>> GetPlayer()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getPlayer", arg);
			return reply.ToObjects<OptionalValue<Models.Player>>(this.Converter);
		}

		public async Task<OptionalValue<Models.AverageStats>> GetPlayerAverageStats(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getPlayerAverageStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.AverageStats>>(this.Converter);
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

		public async Task<List<Models.LogEntry>> GetTransactionLogs(Principal arg0, Models.ItemType arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTransactionLogs", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.LogEntry>>(this.Converter);
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

		public async Task<(bool ReturnArg0, string ReturnArg1)> IsGameMatched()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "isGameMatched", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> MintDeck(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "mintDeck", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> OpenChest(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "openChest", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, PlayerId ReturnArg1, bool ReturnArg2, string ReturnArg3, UnboundedUInt ReturnArg4)> RegisterPlayer(Username arg0, AvatarID arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "registerPlayer", arg);
			return reply.ToObjects<bool, PlayerId, bool, string, UnboundedUInt>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> SaveFinishedGame(MatchID arg0, CanisterLoginApiClient.SaveFinishedGameArg1 arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "saveFinishedGame", arg);
			return reply.ToObjects<bool, string>(this.Converter);
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

		public async Task<bool> SetPlayerActive()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "setPlayerActive", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<CanisterLoginApiClient.TestReturnArg0> Test(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "test", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<CanisterLoginApiClient.TestReturnArg0>(this.Converter);
		}

		public async Task<List<(Models.AchievementCategory, List<Models.Achievement>, List<Models.IndividualAchievementProgress>)>> UpdateAndGetAchievements()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateAndGetAchievements", arg);
			return reply.ToObjects<List<(Models.AchievementCategory, List<Models.Achievement>, List<Models.IndividualAchievementProgress>)>>(this.Converter);
		}

		public async Task<(bool ReturnArg0, PlayerId ReturnArg1)> UpdateAvatar(AvatarID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateAvatar", arg);
			return reply.ToObjects<bool, PlayerId>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UpdateCategoryProgress(PlayerId arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateCategoryProgress", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, PlayerId ReturnArg1)> UpdateDescription(Description arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateDescription", arg);
			return reply.ToObjects<bool, PlayerId>(this.Converter);
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

		public async Task<(bool ReturnArg0, PlayerId ReturnArg1)> UpdateUsername(Username arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateUsername", arg);
			return reply.ToObjects<bool, PlayerId>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UpgradeNFT(TokenID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "upgradeNFT", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public class GetFriendsListReturnArg0 : OptionalValue<CanisterLoginApiClient.GetFriendsListReturnArg0.GetFriendsListReturnArg0Value>
		{
			public GetFriendsListReturnArg0()
			{
			}

			public GetFriendsListReturnArg0(CanisterLoginApiClient.GetFriendsListReturnArg0.GetFriendsListReturnArg0Value value) : base(value)
			{
			}

			public class GetFriendsListReturnArg0Value : List<PlayerId>
			{
				public GetFriendsListReturnArg0Value()
				{
				}
			}
		}

		public class GetMatchHistoryByPrincipalReturnArg0 : List<CanisterLoginApiClient.GetMatchHistoryByPrincipalReturnArg0.GetMatchHistoryByPrincipalReturnArg0Element>
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
			public CanisterLoginApiClient.GetMintedInfoReturnArg0.ChestsInfo Chests { get; set; }

			[CandidName("flux")]
			public UnboundedUInt Flux { get; set; }

			[CandidName("gameNFTs")]
			public CanisterLoginApiClient.GetMintedInfoReturnArg0.GameNFTsInfo GameNFTs { get; set; }

			[CandidName("shards")]
			public UnboundedUInt Shards { get; set; }

			public GetMintedInfoReturnArg0(CanisterLoginApiClient.GetMintedInfoReturnArg0.ChestsInfo chests, UnboundedUInt flux, CanisterLoginApiClient.GetMintedInfoReturnArg0.GameNFTsInfo gameNFTs, UnboundedUInt shards)
			{
				this.Chests = chests;
				this.Flux = flux;
				this.GameNFTs = gameNFTs;
				this.Shards = shards;
			}

			public GetMintedInfoReturnArg0()
			{
			}

			public class ChestsInfo
			{
				[CandidName("quantity")]
				public UnboundedUInt Quantity { get; set; }

				[CandidName("tokenIDs")]
				public CanisterLoginApiClient.GetMintedInfoReturnArg0.ChestsInfo.TokenIDsInfo TokenIDs { get; set; }

				public ChestsInfo(UnboundedUInt quantity, CanisterLoginApiClient.GetMintedInfoReturnArg0.ChestsInfo.TokenIDsInfo tokenIDs)
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
				public CanisterLoginApiClient.GetMintedInfoReturnArg0.GameNFTsInfo.TokenIDsInfo TokenIDs { get; set; }

				public GameNFTsInfo(UnboundedUInt quantity, CanisterLoginApiClient.GetMintedInfoReturnArg0.GameNFTsInfo.TokenIDsInfo tokenIDs)
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

		public class TestReturnArg0 : OptionalValue<CanisterLoginApiClient.TestReturnArg0.TestReturnArg0Value>
		{
			public TestReturnArg0()
			{
			}

			public TestReturnArg0(CanisterLoginApiClient.TestReturnArg0.TestReturnArg0Value value) : base(value)
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
	}
}