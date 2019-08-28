using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assessment2.Models;
using Assessment2.Models.Index;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Options;

namespace Assessment2.Services
{
    public class AzureSearchService : IIndexedSearchService
    {
        private readonly AzureSearchOptions _options;

        public AzureSearchService(IOptions<AzureSearchOptions> options)
        {
            _options = options.Value;
          
        }

        public async Task<IndexDocument[]> SearchDocsAsync(IndexedSearchQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
                       
            var parameters = new SearchParameters
            {
                Select = new[] { nameof(IndexDocument.Id), nameof(IndexDocument.Name), nameof(IndexDocument.FormerName), nameof(IndexDocument.City), nameof(IndexDocument.DocType) },
                Top = query.Top
            };
            var andFilters = new List<string>();
            if (!query.Markets.IsNullOrEmpty())
            {
                andFilters.Add($"search.in({nameof(IndexDocument.Market)}, '{string.Join("|",query.Markets)}', '|')");
            }
            if (!string.IsNullOrEmpty(query.DocType))
            {
                andFilters.Add($"{nameof(IndexDocument.DocType)} eq '{query.DocType}'");
            }
            if (andFilters.Any())
            {
                parameters.Filter = string.Join(" and ", andFilters);
            }
            if (!string.IsNullOrEmpty(query.OrderBy))
            {
                parameters.OrderBy = new[] { query.OrderBy };
            }

            using (var client = CreateClient(_options))
            {
                var searchResult = await client.Documents.SearchAsync<IndexDocument>(string.IsNullOrEmpty(query.Phrase) ? "*" : query.Phrase, parameters);
                return searchResult.Results.Select(x => x.Document).ToArray();
            }
        }

        public async Task<string[]> FindAllMarketsAsync()
        {
            var parameters = new SearchParameters
            {
                Select = new[] { nameof(IndexDocument.Id), nameof(IndexDocument.Market), },
                Facets = new [] { $"{nameof(IndexDocument.Market)}, count:100"}
            };
            using (var client = CreateClient(_options))
            {
                var searchResult = await client.Documents.SearchAsync<IndexDocument>("*", parameters);
                return searchResult.Facets[nameof(IndexDocument.Market)].Select(x => x.Value.ToString()).ToArray();
            }
        }

        private static SearchIndexClient CreateClient(AzureSearchOptions options)
        {
            return  new SearchIndexClient(options.SearchServiceName, options.IndexName, new SearchCredentials(options.SearchServiceQueryApiKey));
        }
    }
}
