using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using CanisterPK.UserGeekICHubCanister;
using EdjCase.ICP.Agent.Responses;
using EdjCase.ICP.Candid.Mapping;
using System.Collections.Generic;
using BatchId = EdjCase.ICP.Candid.Models.UnboundedUInt;
using ChunkId = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Key = System.String;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;

namespace CanisterPK.UserGeekICHubCanister
{
	public class UserGeekICHubCanisterApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public UserGeekICHubCanisterApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<UserGeekICHubCanisterApiClient.GetReturnArg0> Get(UserGeekICHubCanisterApiClient.GetArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UserGeekICHubCanisterApiClient.GetReturnArg0>(this.Converter);
		}

		public async Task<UserGeekICHubCanisterApiClient.GetChunkReturnArg0> GetChunk(UserGeekICHubCanisterApiClient.GetChunkArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "get_chunk", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UserGeekICHubCanisterApiClient.GetChunkReturnArg0>(this.Converter);
		}

		public async Task<UserGeekICHubCanisterApiClient.ListReturnArg0> List(UserGeekICHubCanisterApiClient.ListArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "list", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UserGeekICHubCanisterApiClient.ListReturnArg0>(this.Converter);
		}

		public async Task<UserGeekICHubCanisterApiClient.CertifiedTreeReturnArg0> CertifiedTree(UserGeekICHubCanisterApiClient.CertifiedTreeArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "certified_tree", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<UserGeekICHubCanisterApiClient.CertifiedTreeReturnArg0>(this.Converter);
		}

		public async Task<UserGeekICHubCanisterApiClient.CreateBatchReturnArg0> CreateBatch(UserGeekICHubCanisterApiClient.CreateBatchArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "create_batch", arg);
			return reply.ToObjects<UserGeekICHubCanisterApiClient.CreateBatchReturnArg0>(this.Converter);
		}

		public async Task<UserGeekICHubCanisterApiClient.CreateChunkReturnArg0> CreateChunk(UserGeekICHubCanisterApiClient.CreateChunkArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "create_chunk", arg);
			return reply.ToObjects<UserGeekICHubCanisterApiClient.CreateChunkReturnArg0>(this.Converter);
		}

		public async Task CommitBatch(UserGeekICHubCanisterApiClient.CommitBatchArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "commit_batch", arg);
		}

		public async Task CreateAsset(Models.CreateAssetArguments arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "create_asset", arg);
		}

		public async Task SetAssetContent(Models.SetAssetContentArguments arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "set_asset_content", arg);
		}

		public async Task UnsetAssetContent(Models.UnsetAssetContentArguments arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "unset_asset_content", arg);
		}

		public async Task DeleteAsset(Models.DeleteAssetArguments arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "delete_asset", arg);
		}

		public async Task Clear(Models.ClearArguments arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "clear", arg);
		}

		public async Task Store(UserGeekICHubCanisterApiClient.StoreArg0 arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "store", arg);
		}

		public async Task<Models.HttpResponse> HttpRequest(Models.HttpRequest request)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(request, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "http_request", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Models.HttpResponse>(this.Converter);
		}

		public async Task<OptionalValue<Models.StreamingCallbackHttpResponse>> HttpRequestStreamingCallback(Models.StreamingCallbackToken token)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(token, this.Converter));
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "http_request_streaming_callback", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<OptionalValue<Models.StreamingCallbackHttpResponse>>(this.Converter);
		}

		public async Task Authorize(Principal arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			await this.Agent.CallAndWaitAsync(this.CanisterId, "authorize", arg);
		}

		public class GetArg0
		{
			[CandidName("key")]
			public Key Key { get; set; }

			[CandidName("accept_encodings")]
			public List<string> AcceptEncodings { get; set; }

			public GetArg0(Key key, List<string> acceptEncodings)
			{
				this.Key = key;
				this.AcceptEncodings = acceptEncodings;
			}

			public GetArg0()
			{
			}
		}

		public class GetReturnArg0
		{
			[CandidName("content")]
			public List<byte> Content { get; set; }

			[CandidName("content_type")]
			public string ContentType { get; set; }

			[CandidName("content_encoding")]
			public string ContentEncoding { get; set; }

			[CandidName("sha256")]
			public OptionalValue<List<byte>> Sha256 { get; set; }

			[CandidName("total_length")]
			public UnboundedUInt TotalLength { get; set; }

			public GetReturnArg0(List<byte> content, string contentType, string contentEncoding, OptionalValue<List<byte>> sha256, UnboundedUInt totalLength)
			{
				this.Content = content;
				this.ContentType = contentType;
				this.ContentEncoding = contentEncoding;
				this.Sha256 = sha256;
				this.TotalLength = totalLength;
			}

			public GetReturnArg0()
			{
			}
		}

		public class GetChunkArg0
		{
			[CandidName("key")]
			public Key Key { get; set; }

			[CandidName("content_encoding")]
			public string ContentEncoding { get; set; }

			[CandidName("index")]
			public UnboundedUInt Index { get; set; }

			[CandidName("sha256")]
			public OptionalValue<List<byte>> Sha256 { get; set; }

			public GetChunkArg0(Key key, string contentEncoding, UnboundedUInt index, OptionalValue<List<byte>> sha256)
			{
				this.Key = key;
				this.ContentEncoding = contentEncoding;
				this.Index = index;
				this.Sha256 = sha256;
			}

			public GetChunkArg0()
			{
			}
		}

		public class GetChunkReturnArg0
		{
			[CandidName("content")]
			public List<byte> Content { get; set; }

			public GetChunkReturnArg0(List<byte> content)
			{
				this.Content = content;
			}

			public GetChunkReturnArg0()
			{
			}
		}

		public class ListArg0
		{
			public ListArg0()
			{
			}
		}

		public class ListReturnArg0 : List<UserGeekICHubCanisterApiClient.ListReturnArg0.ListReturnArg0Element>
		{
			public ListReturnArg0()
			{
			}

			public class ListReturnArg0Element
			{
				[CandidName("key")]
				public Key Key { get; set; }

				[CandidName("content_type")]
				public string ContentType { get; set; }

				[CandidName("encodings")]
				public UserGeekICHubCanisterApiClient.ListReturnArg0.ListReturnArg0Element.EncodingsInfo Encodings { get; set; }

				public ListReturnArg0Element(Key key, string contentType, UserGeekICHubCanisterApiClient.ListReturnArg0.ListReturnArg0Element.EncodingsInfo encodings)
				{
					this.Key = key;
					this.ContentType = contentType;
					this.Encodings = encodings;
				}

				public ListReturnArg0Element()
				{
				}

				public class EncodingsInfo : List<UserGeekICHubCanisterApiClient.ListReturnArg0.ListReturnArg0Element.EncodingsInfo.EncodingsInfoElement>
				{
					public EncodingsInfo()
					{
					}

					public class EncodingsInfoElement
					{
						[CandidName("content_encoding")]
						public string ContentEncoding { get; set; }

						[CandidName("sha256")]
						public OptionalValue<List<byte>> Sha256 { get; set; }

						[CandidName("length")]
						public UnboundedUInt Length { get; set; }

						[CandidName("modified")]
						public Time Modified { get; set; }

						public EncodingsInfoElement(string contentEncoding, OptionalValue<List<byte>> sha256, UnboundedUInt length, Time modified)
						{
							this.ContentEncoding = contentEncoding;
							this.Sha256 = sha256;
							this.Length = length;
							this.Modified = modified;
						}

						public EncodingsInfoElement()
						{
						}
					}
				}
			}
		}

		public class CertifiedTreeArg0
		{
			public CertifiedTreeArg0()
			{
			}
		}

		public class CertifiedTreeReturnArg0
		{
			[CandidName("certificate")]
			public List<byte> Certificate { get; set; }

			[CandidName("tree")]
			public List<byte> Tree { get; set; }

			public CertifiedTreeReturnArg0(List<byte> certificate, List<byte> tree)
			{
				this.Certificate = certificate;
				this.Tree = tree;
			}

			public CertifiedTreeReturnArg0()
			{
			}
		}

		public class CreateBatchArg0
		{
			public CreateBatchArg0()
			{
			}
		}

		public class CreateBatchReturnArg0
		{
			[CandidName("batch_id")]
			public BatchId BatchId { get; set; }

			public CreateBatchReturnArg0(BatchId batchId)
			{
				this.BatchId = batchId;
			}

			public CreateBatchReturnArg0()
			{
			}
		}

		public class CreateChunkArg0
		{
			[CandidName("batch_id")]
			public BatchId BatchId { get; set; }

			[CandidName("content")]
			public List<byte> Content { get; set; }

			public CreateChunkArg0(BatchId batchId, List<byte> content)
			{
				this.BatchId = batchId;
				this.Content = content;
			}

			public CreateChunkArg0()
			{
			}
		}

		public class CreateChunkReturnArg0
		{
			[CandidName("chunk_id")]
			public ChunkId ChunkId { get; set; }

			public CreateChunkReturnArg0(ChunkId chunkId)
			{
				this.ChunkId = chunkId;
			}

			public CreateChunkReturnArg0()
			{
			}
		}

		public class CommitBatchArg0
		{
			[CandidName("batch_id")]
			public BatchId BatchId { get; set; }

			[CandidName("operations")]
			public List<Models.BatchOperationKind> Operations { get; set; }

			public CommitBatchArg0(BatchId batchId, List<Models.BatchOperationKind> operations)
			{
				this.BatchId = batchId;
				this.Operations = operations;
			}

			public CommitBatchArg0()
			{
			}
		}

		public class StoreArg0
		{
			[CandidName("key")]
			public Key Key { get; set; }

			[CandidName("content_type")]
			public string ContentType { get; set; }

			[CandidName("content_encoding")]
			public string ContentEncoding { get; set; }

			[CandidName("content")]
			public List<byte> Content { get; set; }

			[CandidName("sha256")]
			public OptionalValue<List<byte>> Sha256 { get; set; }

			public StoreArg0(Key key, string contentType, string contentEncoding, List<byte> content, OptionalValue<List<byte>> sha256)
			{
				this.Key = key;
				this.ContentType = contentType;
				this.ContentEncoding = contentEncoding;
				this.Content = content;
				this.Sha256 = sha256;
			}

			public StoreArg0()
			{
			}
		}
	}
}