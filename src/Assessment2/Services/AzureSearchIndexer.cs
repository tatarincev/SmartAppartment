using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Assessment2.Models;
using Assessment2.Models.Index;
using AutoMapper;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Options;

namespace Assessment2.Services
{
    public class AzureSearchIndexer : IIndexer
    {
        private readonly AzureSearchOptions _options;
        private readonly IMapper _mapper;
        public AzureSearchIndexer(IOptions<AzureSearchOptions> options, IMapper mapper)
        {
            _options = options.Value;
            _mapper = mapper;
        }

        public void DoIndex<TDataItem>(string indexName, Stream stream, Action<string> progressCallback, CancellationToken cancellationToken)
        {
            if (indexName == null)
            {
                throw new ArgumentNullException(nameof(indexName));
            }
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var client = CreateSearchServiceClient(_options);
            if (!client.Indexes.Exists(indexName))
            {
                var definition = new Index()
                {
                    Name = indexName,
                    Fields = FieldBuilder.BuildForType<IndexModel>()
                };
                client.Indexes.Create(definition);
            }

            var indexClient = client.Indexes.GetClient(indexName);
            using (var jsonDataSource = new JsonFileSystemPagedDataSource<TDataItem>(stream))
            {
                var indexedDocCount = 0;
                IEnumerable<TDataItem> dataItems;
                do
                {
                    dataItems = jsonDataSource.FetchNextPage();
                    var indexActions = new List<IndexAction<IndexModel>>();
                    foreach (var dataItem in dataItems)
                    {
                        var indexModel = _mapper.Map<IndexModel>(dataItem);
                        indexActions.Add(new IndexAction<IndexModel>(indexModel));
                    }

                    cancellationToken.ThrowIfCancellationRequested();

                    if (indexActions.Any())
                    {
                        var batch = IndexBatch.New(indexActions);
                        try
                        {
                            indexClient.Documents.Index(batch);
                            indexedDocCount += indexActions.Count;
                            progressCallback($"{indexedDocCount} documents have been indexed");
                        }
                        catch (IndexBatchException e)
                        {
                            // When a service is under load, indexing might fail for some documents in the batch. 
                            // Depending on your application, you can compensate by delaying and retrying. 
                            // For this simple demo, we just log the failed document keys and continue.
                            progressCallback($"Failed to index some of the documents: {string.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key))}");
                        }
                    }
                }
                while (dataItems.Any());              
            }
        }

        private static SearchServiceClient CreateSearchServiceClient(AzureSearchOptions options)
        {
            if(options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            var serviceClient = new SearchServiceClient(options.SearchServiceName, new SearchCredentials(options.SearchServiceAdminApiKey));
            return serviceClient;
        }
    }
}
