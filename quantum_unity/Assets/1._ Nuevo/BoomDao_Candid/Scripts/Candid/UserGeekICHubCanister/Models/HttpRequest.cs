using EdjCase.ICP.Candid.Mapping;
using CanisterPK.UserGeekICHubCanister.Models;
using System.Collections.Generic;
using HeaderField = System.ValueTuple<System.String, System.String>;

namespace CanisterPK.UserGeekICHubCanister.Models
{
	public class HttpRequest
	{
		[CandidName("method")]
		public string Method { get; set; }

		[CandidName("url")]
		public string Url { get; set; }

		[CandidName("headers")]
		public HttpRequest.HeadersInfo Headers { get; set; }

		[CandidName("body")]
		public List<byte> Body { get; set; }

		public HttpRequest(string method, string url, HttpRequest.HeadersInfo headers, List<byte> body)
		{
			this.Method = method;
			this.Url = url;
			this.Headers = headers;
			this.Body = body;
		}

		public HttpRequest()
		{
		}

		public class HeadersInfo : List<HeaderField>
		{
			public HeadersInfo()
			{
			}
		}
	}
}