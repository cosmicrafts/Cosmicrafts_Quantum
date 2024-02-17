using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace CanisterPK.ReportsICHubCanister.Models
{
	public class ReportData
	{
		[CandidName("category")]
		public UnboundedUInt Category { get; set; }

		[CandidName("dateReported")]
		public string DateReported { get; set; }

		[CandidName("reasonReport")]
		public string ReasonReport { get; set; }

		[CandidName("reportType")]
		public string ReportType { get; set; }

		[CandidName("reported")]
		public string Reported { get; set; }

		[CandidName("userReports")]
		public Principal UserReports { get; set; }

		public ReportData(UnboundedUInt category, string dateReported, string reasonReport, string reportType, string reported, Principal userReports)
		{
			this.Category = category;
			this.DateReported = dateReported;
			this.ReasonReport = reasonReport;
			this.ReportType = reportType;
			this.Reported = reported;
			this.UserReports = userReports;
		}

		public ReportData()
		{
		}
	}
}