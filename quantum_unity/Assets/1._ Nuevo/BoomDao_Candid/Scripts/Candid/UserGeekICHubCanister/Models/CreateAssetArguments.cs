using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using Key = System.String;
using HeaderField = System.ValueTuple<System.String, System.String>;

namespace CanisterPK.UserGeekICHubCanister.Models
{
	public class CreateAssetArguments
	{
		[CandidName("key")]
		public Key Key { get; set; }

		[CandidName("content_type")]
		public string ContentType { get; set; }

		[CandidName("max_age")]
		public OptionalValue<ulong> MaxAge { get; set; }

		[CandidName("headers")]
		public OptionalValue<List<HeaderField>> Headers { get; set; }

		public CreateAssetArguments(Key key, string contentType, OptionalValue<ulong> maxAge, OptionalValue<List<HeaderField>> headers)
		{
			this.Key = key;
			this.ContentType = contentType;
			this.MaxAge = maxAge;
			this.Headers = headers;
		}

		public CreateAssetArguments()
		{
		}
	}
}