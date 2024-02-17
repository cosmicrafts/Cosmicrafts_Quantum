using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using System.Collections.Generic;
using CanisterPK.ImagesICHub;
using EdjCase.ICP.Agent.Responses;

namespace CanisterPK.ImagesICHub
{
	public class ImagesICHubApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public ImagesICHubApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<Dictionary<string, Models.ImageData>> GetAllImages()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllImages", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Dictionary<string, Models.ImageData>>(this.Converter);
		}

		public async Task<Models.HttpResponse> HttpRequest(Models.HttpRequest arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "http_request", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.HttpResponse>(this.Converter);
		}

		public async Task<(bool ReturnArg0, string ReturnArg1)> SaveImage(List<byte> arg0, string arg1)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter), CandidTypedValue.FromObject(arg1, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "saveImage", arg);
			return reply.ToObjects<bool, string>(this.Converter);
		}
	}
}