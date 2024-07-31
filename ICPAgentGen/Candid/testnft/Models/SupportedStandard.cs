using EdjCase.ICP.Candid.Mapping;

namespace CanisterPK.testnft.Models
{
	public class SupportedStandard
	{
		[CandidName("name")]
		public string Name { get; set; }

		[CandidName("url")]
		public string Url { get; set; }

		public SupportedStandard(string name, string url)
		{
			this.Name = name;
			this.Url = url;
		}

		public SupportedStandard()
		{
		}
	}
}