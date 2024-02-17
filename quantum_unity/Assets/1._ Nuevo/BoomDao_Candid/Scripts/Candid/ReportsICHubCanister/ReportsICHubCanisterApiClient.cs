using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using CanisterPK.ReportsICHubCanister;
using EdjCase.ICP.Agent.Responses;
using System.Collections.Generic;
using EdjCase.ICP.Candid.Mapping;
using UserID = EdjCase.ICP.Candid.Models.Principal;

namespace CanisterPK.ReportsICHubCanister
{
	public class ReportsICHubCanisterApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public ReportsICHubCanisterApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<bool> AddReport(Models.ReportData arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "addReport", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<ReportsICHubCanisterApiClient.GetAllReportsReturnArg0> GetAllReports()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllReports", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<ReportsICHubCanisterApiClient.GetAllReportsReturnArg0>(this.Converter);
		}

		public async Task<ReportsICHubCanisterApiClient.GetAllReportsByCategoryReturnArg0> GetAllReportsByCategory(UnboundedUInt arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getAllReportsByCategory", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<ReportsICHubCanisterApiClient.GetAllReportsByCategoryReturnArg0>(this.Converter);
		}

		public async Task<OptionalValue<List<Models.ReportData>>> GetUserReports(UserID arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "getUserReports", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<List<Models.ReportData>>>(this.Converter);
		}

		public class GetAllReportsReturnArg0 : List<ReportsICHubCanisterApiClient.GetAllReportsReturnArg0.GetAllReportsReturnArg0Element>
		{
			public GetAllReportsReturnArg0()
			{
			}

			public class GetAllReportsReturnArg0Element
			{
				[CandidTag(0U)]
				public UserID F0 { get; set; }

				[CandidTag(1U)]
				public List<Models.ReportData> F1 { get; set; }

				public GetAllReportsReturnArg0Element(UserID f0, List<Models.ReportData> f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetAllReportsReturnArg0Element()
				{
				}
			}
		}

		public class GetAllReportsByCategoryReturnArg0 : List<ReportsICHubCanisterApiClient.GetAllReportsByCategoryReturnArg0.GetAllReportsByCategoryReturnArg0Element>
		{
			public GetAllReportsByCategoryReturnArg0()
			{
			}

			public class GetAllReportsByCategoryReturnArg0Element
			{
				[CandidTag(0U)]
				public UserID F0 { get; set; }

				[CandidTag(1U)]
				public List<Models.ReportData> F1 { get; set; }

				public GetAllReportsByCategoryReturnArg0Element(UserID f0, List<Models.ReportData> f1)
				{
					this.F0 = f0;
					this.F1 = f1;
				}

				public GetAllReportsByCategoryReturnArg0Element()
				{
				}
			}
		}
	}
}