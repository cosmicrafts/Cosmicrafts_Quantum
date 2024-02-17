using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using CanisterPK.ProjectsICHub;
using System.Collections.Generic;
using EdjCase.ICP.Agent.Responses;

namespace CanisterPK.ProjectsICHub
{
	public class ProjectsICHubApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public ProjectsICHubApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> AddNewsToProject(Models.ProjectNews arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "addNewsToProject", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> CreateProject(Models.ProjectData arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "createProject", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> DeleteVersion(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "deleteVersion", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<List<Models.ProjectFullData>> GetAllProjects()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllProjects", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<List<Models.ProjectFullData>>(this.Converter);
		}

		public async Task<OptionalValue<Models.ProjectData>> GetMyProject()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMyProject", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.ProjectData>>(this.Converter);
		}

		public async Task<OptionalValue<List<Models.ProjectNews>>> GetMyProjectNews()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMyProjectNews", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.ProjectNews>>>(this.Converter);
		}

		public async Task<OptionalValue<List<Models.VersionData>>> GetMyProjectsVersions()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getMyProjectsVersions", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.VersionData>>>(this.Converter);
		}

		public async Task<(bool ReturnArg0, OptionalValue<Models.ProjectData> ReturnArg1)> GetProjectById(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getProjectById", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<bool, OptionalValue<Models.ProjectData>>(this.Converter);
		}

		public async Task<OptionalValue<List<Models.ProjectNews>>> GetProjectNewsById(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getProjectNewsById", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.ProjectNews>>>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> SaveProjectVersions(List<Models.VersionData> arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "saveProjectVersions", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> UpdateProject(UnboundedUInt arg0, Models.ProjectData arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateProject", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}
	}
}