using EdjCase.ICP.Candid.Mapping;

namespace Cosmicrafts.MainCanister.Models
{
	public class Supportedstandard1
	{
		[CandidName("name")]
		public string Name { get; set; }

		[CandidName("url")]
		public string Url { get; set; }

		public Supportedstandard1(string name, string url)
		{
			this.Name = name;
			this.Url = url;
		}

		public Supportedstandard1()
		{
		}
	}
}