using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Assessment2.Models;
using Assessment2.Models.Schema.Index;
using Assessment2.Services.Json;
using AutoMapper;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Options;

namespace Assessment2.Services.Azure
{
    public class AzureSearchIndexer : ISearchIndexer
    {
        private readonly AzureSearchOptions _options;
        private readonly IMapper _mapper;
        public AzureSearchIndexer(IOptions<AzureSearchOptions> options, IMapper mapper)
        {
            _options = options.Value;
            _mapper = mapper;
        }

        public void DoIndex<TDataItem>(Stream stream, Action<string> progressCallback, CancellationToken cancellationToken)
        {          
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var client = new SearchServiceClient(_options.SearchServiceName, new SearchCredentials(_options.SearchServiceAdminApiKey));
            if (!client.Indexes.Exists(_options.IndexName))
            {
                var definition = CreateIndexDefinition(_options.IndexName);
                client.Indexes.Create(definition);
            }

            var indexClient = client.Indexes.GetClient(_options.IndexName);
            using (var jsonDataSource = new JsonPagedDataSource<TDataItem>(stream))
            {
                var indexedDocCount = 0;
                IEnumerable<TDataItem> dataItems;
                do
                {
                    dataItems = jsonDataSource.FetchNextPage();
                    var indexActions = new List<IndexAction<ApartmentDataIndexDocument>>();
                    foreach (var dataItem in dataItems)
                    {
                        var indexModel = _mapper.Map<ApartmentDataIndexDocument>(dataItem);
                        indexActions.Add(new IndexAction<ApartmentDataIndexDocument>(indexModel));
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

        private static Index CreateIndexDefinition(string indexName)
        {
            var result = new Index()
            {
                Name = indexName,
                Fields = FieldBuilder.BuildForType<ApartmentDataIndexDocument>(),
                ScoringProfiles = new[]
                 {
                     new ScoringProfile
                     {
                          Name = "names",
                           TextWeights = new TextWeights
                           {
                               Weights = new Dictionary<string, double>
                               {
                                   { nameof(ApartmentDataIndexDocument.Name), 3.0 },
                                   { nameof(ApartmentDataIndexDocument.FormerName), 3.0 }
                               }
                           }
                     }
                 },
                Suggesters = new[]
                {
                    new Suggester("sg", nameof(ApartmentDataIndexDocument.Name)
                                      , nameof(ApartmentDataIndexDocument.FormerName)
                                      , nameof(ApartmentDataIndexDocument.StreetAddress)
                                      , nameof(ApartmentDataIndexDocument.City) )
                },
                DefaultScoringProfile = "names"
            };
            return result;
        }

    }
}
