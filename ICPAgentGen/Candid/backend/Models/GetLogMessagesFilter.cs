using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;

namespace Cosmicrafts.backend.Models
{
	public class GetLogMessagesFilter
	{
		[CandidName("analyzeCount")]
		public uint AnalyzeCount { get; set; }

		[CandidName("messageContains")]
		public OptionalValue<string> MessageContains { get; set; }

		[CandidName("messageRegex")]
		public OptionalValue<string> MessageRegex { get; set; }

		public GetLogMessagesFilter(uint analyzeCount, OptionalValue<string> messageContains, OptionalValue<string> messageRegex)
		{
			this.AnalyzeCount = analyzeCount;
			this.MessageContains = messageContains;
			this.MessageRegex = messageRegex;
		}

		public GetLogMessagesFilter()
		{
		}
	}
}