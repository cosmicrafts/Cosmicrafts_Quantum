using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using Cosmicrafts.backend;
using System.Collections.Generic;
using EdjCase.ICP.Agent.Responses;
using EdjCase.ICP.Candid.Mapping;
using Username = System.String;
using Txindex1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using TokenId = EdjCase.ICP.Candid.Models.UnboundedUInt;
using TokenID = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Timestamp = System.UInt64;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;
using ReferralCode = System.String;
using Playerid1 = EdjCase.ICP.Candid.Models.Principal;
using PlayerId = EdjCase.ICP.Candid.Models.Principal;
using MetaDatum = System.ValueTuple<System.String, Cosmicrafts.backend.Models.Value>;
using MatchID = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Level = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Description = System.String;
using Balance1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using AvatarID = EdjCase.ICP.Candid.Models.UnboundedUInt;

namespace Cosmicrafts.backend
{
	public class BackendApiClient
	{
		public IAgent Agent { get; }
		public Principal CanisterId { get; }
		public CandidConverter? Converter { get; }

		public BackendApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> AcceptFriendRequest(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "acceptFriendRequest", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<UnboundedUInt> Add(UnboundedUInt arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "add", arg);
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> AddAvatarToUser(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "addAvatarToUser", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> AddProgressToIndividualAchievement(PlayerId arg0, UnboundedUInt arg1, UnboundedUInt arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "addProgressToIndividualAchievement", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> AddTitleToUser(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "addTitleToUser", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> Admin(Models.AdminFunction arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "admin", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> AdminUpdateMatch(UnboundedUInt arg0, UnboundedUInt arg1, UnboundedUInt arg2, string arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "adminUpdateMatch", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task AssignAchievementsToUser(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAsync(this.CanisterId, "assignAchievementsToUser", arg);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> BlockUser(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "blockUser", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<Models.TransferResult> Burn(Models.BurnArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "burn", arg);
			return reply.ToObjects<Models.TransferResult>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> CancelMatchmaking()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "cancelMatchmaking", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> ClaimAchievementLineReward(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "claimAchievementLineReward", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> ClaimCategoryAchievementReward(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "claimCategoryAchievementReward", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> ClaimGeneralReward(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "claimGeneralReward", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> ClaimIndividualAchievementReward(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "claimIndividualAchievementReward", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> ClaimUserReward(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "claimUserReward", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1, UnboundedUInt ReturnArg2)> CreateAchievement(UnboundedUInt arg0, string arg1, List<Models.AchievementReward> arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "createAchievement", arg);
			return reply.ToObjects<bool, string, UnboundedUInt>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1, UnboundedUInt ReturnArg2)> CreateAchievementCategory(string arg0, List<Models.AchievementReward> arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "createAchievementCategory", arg);
			return reply.ToObjects<bool, string, UnboundedUInt>(this.Converter);
		}

		public async Task<BackendApiClient.CreateBatchOfUnassignedCodesReturnArg0> CreateBatchOfUnassignedCodes()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "createBatchOfUnassignedCodes", arg);
			return reply.ToObjects<BackendApiClient.CreateBatchOfUnassignedCodesReturnArg0>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1, UnboundedUInt ReturnArg2)> CreateIndividualAchievement(UnboundedUInt arg0, string arg1, Models.Achievementtype1 arg2, UnboundedUInt arg3, List<Models.AchievementReward> arg4)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter), CandidTypedValue.FromObject(arg4, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "createIndividualAchievement", arg);
			return reply.ToObjects<bool, string, UnboundedUInt>(this.Converter);
		}

		public async Task<UnboundedUInt> CreateTournament(string arg0, Time arg1, string arg2, Time arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "createTournament", arg);
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1, UnboundedUInt ReturnArg2)> CreateUserMission(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "createUserMission", arg);
			return reply.ToObjects<bool, string, UnboundedUInt>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> DeclineFriendRequest(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "declineFriendRequest", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> DeleteAccount()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "deleteAccount", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> DeleteAllTournaments()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "deleteAllTournaments", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task DepositCycles()
		{
			CandidArg arg = CandidArg.FromCandid();
			await this.Agent.CallAsync(this.CanisterId, "deposit_cycles", arg);
		}

		public async Task<bool> DisputeMatch(UnboundedUInt arg0, UnboundedUInt arg1, string arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "disputeMatch", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<BackendApiClient.DumpAllPlayerMultipliersReturnArg0> DumpAllPlayerMultipliers()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "dumpAllPlayerMultipliers", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.DumpAllPlayerMultipliersReturnArg0>(this.Converter);
		}

		public async Task<List<Models.AchievementCategory>> GetAchievements()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "getAchievements", arg);
			return reply.ToObjects<List<Models.AchievementCategory>>(this.Converter);
		}

		public async Task<List<Models.Tournament>> GetActiveTournaments()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getActiveTournaments", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Tournament>>(this.Converter);
		}

		public async Task<List<Models.AchievementCategory>> GetAllAchievementsData()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllAchievementsData", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.AchievementCategory>>(this.Converter);
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

		public async Task<List<Models.Avatar>> GetAvailableAvatarDetails()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAvailableAvatarDetails", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Avatar>>(this.Converter);
		}

		public async Task<List<UnboundedUInt>> GetAvailableAvatars()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAvailableAvatars", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<UnboundedUInt>>(this.Converter);
		}

		public async Task<List<Models.Title1>> GetAvailableTitleDetails()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAvailableTitleDetails", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Title1>>(this.Converter);
		}

		public async Task<List<UnboundedUInt>> GetAvailableTitles()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAvailableTitles", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<UnboundedUInt>>(this.Converter);
		}

		public async Task<Models.Avatar> GetAvatarById(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "getAvatarById", arg);
			return reply.ToObjects<Models.Avatar>(this.Converter);
		}

		public async Task<BackendApiClient.GetAvatarsReturnArg0> GetAvatars(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAvatars", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetAvatarsReturnArg0>(this.Converter);
		}

		public async Task<BackendApiClient.GetBeyondReferralsReturnArg0> GetBeyondReferrals(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getBeyondReferrals", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetBeyondReferralsReturnArg0>(this.Converter);
		}

		public async Task<BackendApiClient.GetBlockedUsersReturnArg0> GetBlockedUsers()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getBlockedUsers", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetBlockedUsersReturnArg0>(this.Converter);
		}

		public async Task<Models.GetInformationResponse> GetCanistergeekInformation(Models.GetInformationRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getCanistergeekInformation", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.GetInformationResponse>(this.Converter);
		}

		public async Task<BackendApiClient.GetCharactersReturnArg0> GetCharacters(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getCharacters", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetCharactersReturnArg0>(this.Converter);
		}

		public async Task<BackendApiClient.GetChestsReturnArg0> GetChests(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getChests", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetChestsReturnArg0>(this.Converter);
		}

		public async Task<Models.OverallStats> GetCosmicraftsStats()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getCosmicraftsStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.OverallStats>(this.Converter);
		}

		public async Task<BackendApiClient.GetDirectReferralsReturnArg0> GetDirectReferrals(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getDirectReferrals", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetDirectReferralsReturnArg0>(this.Converter);
		}

		public async Task<List<Models.FriendRequest>> GetFriendRequests()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getFriendRequests", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.FriendRequest>>(this.Converter);
		}

		public async Task<BackendApiClient.GetFriendsListReturnArg0> GetFriendsList()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getFriendsList", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetFriendsListReturnArg0>(this.Converter);
		}

		public async Task<OptionalValue<(Models.Player, Models.PlayerGamesStats, Models.AverageStats)>> GetFullProfile(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getFullProfile", arg);
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
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "getGeneralMissions", arg);
			return reply.ToObjects<List<Models.MissionsUser>>(this.Converter);
		}

		public async Task<BackendApiClient.GetGrandReferrerReturnArg0> GetGrandReferrer(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getGrandReferrer", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetGrandReferrerReturnArg0>(this.Converter);
		}

		public async Task<List<Models.Tournament>> GetInactiveTournaments()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getInactiveTournaments", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.Tournament>>(this.Converter);
		}

		public async Task<BackendApiClient.GetIndirectReferralsReturnArg0> GetIndirectReferrals(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getIndirectReferrals", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetIndirectReferralsReturnArg0>(this.Converter);
		}

		public async Task<Models.TokenInitArgs> GetInitArgs()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getInitArgs", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.TokenInitArgs>(this.Converter);
		}

		public async Task<OptionalValue<BackendApiClient.GetLastLoginReturnArg0Value>> GetLastLogin(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getLastLogin", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<BackendApiClient.GetLastLoginReturnArg0Value>>(this.Converter);
		}

		public async Task<OptionalValue<(Models.MatchData, Dictionary<Models.Player, Models.PlayerGamesStats>)>> GetMatchDetails(MatchID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMatchDetails", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<(Models.MatchData, Dictionary<Models.Player, Models.PlayerGamesStats>)>>(this.Converter);
		}

		public async Task<BackendApiClient.GetMatchHistoryByPrincipalReturnArg0> GetMatchHistoryByPrincipal(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMatchHistoryByPrincipal", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetMatchHistoryByPrincipalReturnArg0>(this.Converter);
		}

		public async Task<BackendApiClient.GetMatchIDsByPrincipalReturnArg0> GetMatchIDsByPrincipal(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMatchIDsByPrincipal", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetMatchIDsByPrincipalReturnArg0>(this.Converter);
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
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "getMatchSearching", arg);
			return reply.ToObjects<Models.MMSearchStatus, UnboundedUInt, string>(this.Converter);
		}

		public async Task<OptionalValue<Models.BasicStats>> GetMatchStats(MatchID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMatchStats", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.BasicStats>>(this.Converter);
		}

		public async Task<string> GetMessage()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMessage", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task<BackendApiClient.GetMintedInfoReturnArg0> GetMintedInfo(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMintedInfo", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetMintedInfoReturnArg0>(this.Converter);
		}

		public async Task<double> GetMultiplier(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMultiplier", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<double>(this.Converter);
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

		public async Task<BackendApiClient.GetNFTsReturnArg0> GetNFTs(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getNFTs", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetNFTsReturnArg0>(this.Converter);
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

		public async Task<BackendApiClient.GetPlayerDeckReturnArg0> GetPlayerDeck(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getPlayerDeck", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetPlayerDeckReturnArg0>(this.Converter);
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

		public async Task<BackendApiClient.GetReferralCodeReturnArg0> GetReferralCode(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getReferralCode", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetReferralCodeReturnArg0>(this.Converter);
		}

		public async Task<BackendApiClient.GetReferrerReturnArg0> GetReferrer(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getReferrer", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetReferrerReturnArg0>(this.Converter);
		}

		public async Task<List<Principal>> GetRegisteredUsers(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getRegisteredUsers", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Principal>>(this.Converter);
		}

		public async Task<OptionalValue<UnboundedUInt>> GetSelectedAvatar()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getSelectedAvatar", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<UnboundedUInt>>(this.Converter);
		}

		public async Task<OptionalValue<UnboundedUInt>> GetSelectedTitle()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getSelectedTitle", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<UnboundedUInt>>(this.Converter);
		}

		public async Task<Models.Title1> GetTitleById(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "getTitleById", arg);
			return reply.ToObjects<Models.Title1>(this.Converter);
		}

		public async Task<UnboundedUInt> GetTotalPlayers()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTotalPlayers", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async Task<BackendApiClient.GetTotalReferralNetworkReturnArg0> GetTotalReferralNetwork(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTotalReferralNetwork", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetTotalReferralNetworkReturnArg0>(this.Converter);
		}

		public async Task<BackendApiClient.GetTournamentBracketReturnArg0> GetTournamentBracket(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTournamentBracket", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetTournamentBracketReturnArg0>(this.Converter);
		}

		public async Task<List<Models.LogEntry>> GetTransactionLogs(Principal arg0, Models.ItemType arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTransactionLogs", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.LogEntry>>(this.Converter);
		}

		public async Task<BackendApiClient.GetTrophiesReturnArg0> GetTrophies(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getTrophies", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetTrophiesReturnArg0>(this.Converter);
		}

		public async Task<BackendApiClient.GetUnitsReturnArg0> GetUnits(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUnits", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.GetUnitsReturnArg0>(this.Converter);
		}

		public async Task<List<Models.AchievementCategory>> GetUserAchievementsStructure(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserAchievementsStructure", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.AchievementCategory>>(this.Converter);
		}

		public async Task<List<Models.AchievementCategory>> GetUserAchievementsStructureByCaller()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserAchievementsStructureByCaller", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.AchievementCategory>>(this.Converter);
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
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "getUserMissions", arg);
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
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "get_transaction", arg);
			return reply.ToObjects<OptionalValue<Models.Transaction2>>(this.Converter);
		}

		public async Task<Models.GetTransactionsResponse> GetTransactions(Models.GetTransactionsRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get_transactions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.GetTransactionsResponse>(this.Converter);
		}

		public async Task<string> Greet(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "greet", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task<BackendApiClient.HandleCombatXPReturnArg0> HandleCombatXP(BackendApiClient.HandleCombatXPArg0 arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "handleCombatXP", arg);
			return reply.ToObjects<BackendApiClient.HandleCombatXPReturnArg0>(this.Converter);
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

		public async Task<BackendApiClient.Icrc1MetadataReturnArg0> Icrc1Metadata()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "icrc1_metadata", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.Icrc1MetadataReturnArg0>(this.Converter);
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
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "icrc1_pay_for_transaction", arg);
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
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "icrc1_transfer", arg);
			return reply.ToObjects<Models.TransferResult>(this.Converter);
		}

		public async Task<Models.ApprovalReceipt> Icrc7Approve(Models.ApprovalArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "icrc7_approve", arg);
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
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "icrc7_get_transactions", arg);
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
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "icrc7_transfer", arg);
			return reply.ToObjects<Models.TransferReceipt>(this.Converter);
		}

		public async Task<bool> IsEven(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "isEven", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool>(this.Converter);
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
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "joinTournament", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> LoadAchievements()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "loadAchievements", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> Login(OptionalValue<string> arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "login", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<Models.TransferResult> Mint(Models.Mint arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "mint", arg);
			return reply.ToObjects<Models.TransferResult>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> MintAchievementRewards(Models.AchievementReward arg0, Playerid1 arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "mintAchievementRewards", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> MintChest(Principal arg0, UnboundedUInt arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "mintChest", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1, BackendApiClient.MintDeckReturnArg2 ReturnArg2)> MintDeck()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "mintDeck", arg);
			return reply.ToObjects<bool, string, BackendApiClient.MintDeckReturnArg2>(this.Converter);
		}

		public async Task<Models.MintReceipt> MintNFT(Models.MintArgs arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "mintNFT", arg);
			return reply.ToObjects<Models.MintReceipt>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> OpenChest(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "openChest", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> SaveFinishedGame(MatchID arg0, BackendApiClient.SaveFinishedGameArg1 arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "saveFinishedGame", arg);
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

		public async Task<BackendApiClient.SelectRandomUnitsReturnArg0> SelectRandomUnits(BackendApiClient.SelectRandomUnitsArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "selectRandomUnits", arg);
			return reply.ToObjects<BackendApiClient.SelectRandomUnitsReturnArg0>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> SendFriendRequest(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "sendFriendRequest", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task SetMessage(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAsync(this.CanisterId, "setMessage", arg);
		}

		public async Task<bool> SetPlayerActive()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "setPlayerActive", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> SetPrivacySetting(Models.PrivacySetting arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "setPrivacySetting", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, OptionalValue<Models.Player> ReturnArg1, string ReturnArg2)> Signup(Username arg0, AvatarID arg1, BackendApiClient.SignupArg2 arg2, string arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "signup", arg);
			return reply.ToObjects<bool, OptionalValue<Models.Player>, string>(this.Converter);
		}

		public async Task<bool> StoreCurrentDeck(BackendApiClient.StoreCurrentDeckArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "storeCurrentDeck", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> SubmitFeedback(UnboundedUInt arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "submitFeedback", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<bool> SubmitMatchResult(UnboundedUInt arg0, UnboundedUInt arg1, string arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "submitMatchResult", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<BackendApiClient.TestReturnArg0> Test(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "test", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.TestReturnArg0>(this.Converter);
		}

		public async Task<BackendApiClient.TopPlayersByMultiplierReturnArg0> TopPlayersByMultiplier(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "topPlayersByMultiplier", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<BackendApiClient.TopPlayersByMultiplierReturnArg0>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UnblockUser(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "unblockUser", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UpdateAddFriendAchievement(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "updateAddFriendAchievement", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UpdateAvatar(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "updateAvatar", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UpdateAvatarChangeAchievement(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "updateAvatarChangeAchievement", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<bool> UpdateBracket(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "updateBracket", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task UpdateBracketAfterMatchUpdate(UnboundedUInt arg0, UnboundedUInt arg1, Principal arg2)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter));
			await this.Agent.CallAsync(this.CanisterId, "updateBracketAfterMatchUpdate", arg);
		}

		public async Task UpdateCanistergeekInformation(Models.UpdateInformationRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAsync(this.CanisterId, "updateCanistergeekInformation", arg);
		}

		public async Task<(bool ReturnArg0, PlayerId ReturnArg1, string ReturnArg2)> UpdateDescription(Description arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "updateDescription", arg);
			return reply.ToObjects<bool, PlayerId, string>(this.Converter);
		}

		public async Task UpdatePlayerGameStats(PlayerId arg0, Models.PlayerStats arg1, UnboundedUInt arg2, UnboundedUInt arg3)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter), CandidTypedValue.FromObject(arg2, this.Converter), CandidTypedValue.FromObject(arg3, this.Converter));
			await this.Agent.CallAsync(this.CanisterId, "updatePlayerGameStats", arg);
		}

		public async Task UpdatePlayerLevel(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAsync(this.CanisterId, "updatePlayerLevel", arg);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UpdateProgressManager(Principal arg0, Models.PlayerStats arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "updateProgressManager", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<BackendApiClient.UpdateSoulNFTPlayedReturnArg0> UpdateSoulNFTPlayed(BackendApiClient.UpdateSoulNFTPlayedArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "updateSoulNFTPlayed", arg);
			return reply.ToObjects<BackendApiClient.UpdateSoulNFTPlayedReturnArg0>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UpdateTitle(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "updateTitle", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UpdateUpgradeNFTAchievement(PlayerId arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "updateUpgradeNFTAchievement", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, PlayerId ReturnArg1, string ReturnArg2)> UpdateUsername(Username arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "updateUsername", arg);
			return reply.ToObjects<bool, PlayerId, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UpgradeNFT(TokenID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAsync(this.CanisterId, "upgradeNFT", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public class CreateBatchOfUnassignedCodesReturnArg0 : List<ReferralCode>
		{
			public CreateBatchOfUnassignedCodesReturnArg0()
			{
			}
		}

		public class DumpAllPlayerMultipliersReturnArg0 : List<BackendApiClient.DumpAllPlayerMultipliersReturnArg0.DumpAllPlayerMultipliersReturnArg0Element>
		{
			public DumpAllPlayerMultipliersReturnArg0()
			{
			}

			public class DumpAllPlayerMultipliersReturnArg0Element
			{
				[CandidTag(0U)]
				public PlayerId F0 { get; set; }

				[CandidTag(1U)]
				public double F1 { get; set; }

				public DumpAllPlayerMultipliersReturnArg0Element(PlayerId f0, double f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public DumpAllPlayerMultipliersReturnArg0Element()
				{
				}
			}
		}

		public class GetAvatarsReturnArg0 : List<BackendApiClient.GetAvatarsReturnArg0.GetAvatarsReturnArg0Element>
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

		public class GetBeyondReferralsReturnArg0 : List<PlayerId>
		{
			public GetBeyondReferralsReturnArg0()
			{
			}
		}

		public class GetBlockedUsersReturnArg0 : List<PlayerId>
		{
			public GetBlockedUsersReturnArg0()
			{
			}
		}

		public class GetCharactersReturnArg0 : List<BackendApiClient.GetCharactersReturnArg0.GetCharactersReturnArg0Element>
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

		public class GetChestsReturnArg0 : List<BackendApiClient.GetChestsReturnArg0.GetChestsReturnArg0Element>
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

		public class GetDirectReferralsReturnArg0 : List<PlayerId>
		{
			public GetDirectReferralsReturnArg0()
			{
			}
		}

		public class GetFriendsListReturnArg0 : OptionalValue<BackendApiClient.GetFriendsListReturnArg0.GetFriendsListReturnArg0Value>
		{
			public GetFriendsListReturnArg0()
			{
			}

			public GetFriendsListReturnArg0(BackendApiClient.GetFriendsListReturnArg0.GetFriendsListReturnArg0Value value) : base(value)
			{
			}

			public class GetFriendsListReturnArg0Value : List<PlayerId>
			{
				public GetFriendsListReturnArg0Value()
				{
				}
			}
		}

		public class GetGrandReferrerReturnArg0 : OptionalValue<PlayerId>
		{
			public GetGrandReferrerReturnArg0()
			{
			}

			public GetGrandReferrerReturnArg0(PlayerId value) : base(value)
			{
			}
		}

		public class GetIndirectReferralsReturnArg0 : List<PlayerId>
		{
			public GetIndirectReferralsReturnArg0()
			{
			}
		}

		public class GetLastLoginReturnArg0Value
		{
			[CandidName("country")]
			public OptionalValue<string> Country { get; set; }

			[CandidName("timestamp")]
			public ulong Timestamp { get; set; }

			public GetLastLoginReturnArg0Value(OptionalValue<string> country, ulong timestamp)
			{
				this.Country = country;
				this.Timestamp = timestamp;
			}

			public GetLastLoginReturnArg0Value()
			{
			}
		}

		public class GetMatchHistoryByPrincipalReturnArg0 : List<BackendApiClient.GetMatchHistoryByPrincipalReturnArg0.GetMatchHistoryByPrincipalReturnArg0Element>
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
			public BackendApiClient.GetMintedInfoReturnArg0.ChestsInfo Chests { get; set; }

			[CandidName("gameNFTs")]
			public BackendApiClient.GetMintedInfoReturnArg0.GameNFTsInfo GameNFTs { get; set; }

			[CandidName("stardust")]
			public UnboundedUInt Stardust { get; set; }

			public GetMintedInfoReturnArg0(BackendApiClient.GetMintedInfoReturnArg0.ChestsInfo chests, BackendApiClient.GetMintedInfoReturnArg0.GameNFTsInfo gameNFTs, UnboundedUInt stardust)
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
				public BackendApiClient.GetMintedInfoReturnArg0.ChestsInfo.TokenIDsInfo TokenIDs { get; set; }

				public ChestsInfo(UnboundedUInt quantity, BackendApiClient.GetMintedInfoReturnArg0.ChestsInfo.TokenIDsInfo tokenIDs)
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
				public BackendApiClient.GetMintedInfoReturnArg0.GameNFTsInfo.TokenIDsInfo TokenIDs { get; set; }

				public GameNFTsInfo(UnboundedUInt quantity, BackendApiClient.GetMintedInfoReturnArg0.GameNFTsInfo.TokenIDsInfo tokenIDs)
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

		public class GetNFTsReturnArg0 : List<BackendApiClient.GetNFTsReturnArg0.GetNFTsReturnArg0Element>
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

		public class GetPlayerDeckReturnArg0 : OptionalValue<BackendApiClient.GetPlayerDeckReturnArg0.GetPlayerDeckReturnArg0Value>
		{
			public GetPlayerDeckReturnArg0()
			{
			}

			public GetPlayerDeckReturnArg0(BackendApiClient.GetPlayerDeckReturnArg0.GetPlayerDeckReturnArg0Value value) : base(value)
			{
			}

			public class GetPlayerDeckReturnArg0Value : List<TokenId>
			{
				public GetPlayerDeckReturnArg0Value()
				{
				}
			}
		}

		public class GetReferralCodeReturnArg0 : OptionalValue<ReferralCode>
		{
			public GetReferralCodeReturnArg0()
			{
			}

			public GetReferralCodeReturnArg0(ReferralCode value) : base(value)
			{
			}
		}

		public class GetReferrerReturnArg0 : OptionalValue<PlayerId>
		{
			public GetReferrerReturnArg0()
			{
			}

			public GetReferrerReturnArg0(PlayerId value) : base(value)
			{
			}
		}

		public class GetTotalReferralNetworkReturnArg0
		{
			[CandidName("beyondReferrals")]
			public BackendApiClient.GetTotalReferralNetworkReturnArg0.BeyondReferralsInfo BeyondReferrals { get; set; }

			[CandidName("directReferrals")]
			public BackendApiClient.GetTotalReferralNetworkReturnArg0.DirectReferralsInfo DirectReferrals { get; set; }

			[CandidName("indirectReferrals")]
			public BackendApiClient.GetTotalReferralNetworkReturnArg0.IndirectReferralsInfo IndirectReferrals { get; set; }

			public GetTotalReferralNetworkReturnArg0(BackendApiClient.GetTotalReferralNetworkReturnArg0.BeyondReferralsInfo beyondReferrals, BackendApiClient.GetTotalReferralNetworkReturnArg0.DirectReferralsInfo directReferrals, BackendApiClient.GetTotalReferralNetworkReturnArg0.IndirectReferralsInfo indirectReferrals)
			{
				this.BeyondReferrals = beyondReferrals;
				this.DirectReferrals = directReferrals;
				this.IndirectReferrals = indirectReferrals;
			}

			public GetTotalReferralNetworkReturnArg0()
			{
			}

			public class BeyondReferralsInfo : List<PlayerId>
			{
				public BeyondReferralsInfo()
				{
				}
			}

			public class DirectReferralsInfo : List<PlayerId>
			{
				public DirectReferralsInfo()
				{
				}
			}

			public class IndirectReferralsInfo : List<PlayerId>
			{
				public IndirectReferralsInfo()
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

		public class GetTrophiesReturnArg0 : List<BackendApiClient.GetTrophiesReturnArg0.GetTrophiesReturnArg0Element>
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

		public class GetUnitsReturnArg0 : List<BackendApiClient.GetUnitsReturnArg0.GetUnitsReturnArg0Element>
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

		public class SignupArg2 : OptionalValue<ReferralCode>
		{
			public SignupArg2()
			{
			}

			public SignupArg2(ReferralCode value) : base(value)
			{
			}
		}

		public class StoreCurrentDeckArg0 : List<TokenId>
		{
			public StoreCurrentDeckArg0()
			{
			}
		}

		public class TestReturnArg0 : OptionalValue<BackendApiClient.TestReturnArg0.TestReturnArg0Value>
		{
			public TestReturnArg0()
			{
			}

			public TestReturnArg0(BackendApiClient.TestReturnArg0.TestReturnArg0Value value) : base(value)
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

		public class TopPlayersByMultiplierReturnArg0 : List<BackendApiClient.TopPlayersByMultiplierReturnArg0.TopPlayersByMultiplierReturnArg0Element>
		{
			public TopPlayersByMultiplierReturnArg0()
			{
			}

			public class TopPlayersByMultiplierReturnArg0Element
			{
				[CandidTag(0U)]
				public PlayerId F0 { get; set; }

				[CandidTag(1U)]
				public double F1 { get; set; }

				public TopPlayersByMultiplierReturnArg0Element(PlayerId f0, double f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public TopPlayersByMultiplierReturnArg0Element()
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