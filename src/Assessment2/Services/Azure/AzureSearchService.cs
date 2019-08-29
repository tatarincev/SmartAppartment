using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assessment2.Models;
using Assessment2.Models.Schema.Index;
using AutoMapper;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Options;

namespace Assessment2.Services.Azure
{
    public class AzureSearchService : IIndexedSearchService
    {
        private readonly AzureSearchOptions _options;
        private readonly IMapper _mapper;

        public AzureSearchService(IOptions<AzureSearchOptions> options, IMapper mapper)
        {
            _options = options.Value;
            _mapper = mapper;
        }

        public async Task<AppartmentDataSearchResult> SearchAsync(IndexedSearchQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            var result = new AppartmentDataSearchResult();

            var parameters = new SearchParameters
            {
                Select = new[] {
                        nameof(ApartmentDataIndexDocument.Id),
                        nameof(ApartmentDataIndexDocument.Name),
                        nameof(ApartmentDataIndexDocument.FormerName),
                        nameof(ApartmentDataIndexDocument.City),
                        nameof(ApartmentDataIndexDocument.State),
                        nameof(ApartmentDataIndexDocument.StreetAddress),
                        nameof(ApartmentDataIndexDocument.DataType)
                },
                Top = query.Top,
                IncludeTotalResultCount = true
            };
            var andFilters = new List<string>();
            if (!query.Markets.IsNullOrEmpty())
            {
                andFilters.Add($"search.in({nameof(ApartmentDataIndexDocument.Market)}, '{string.Join("|",query.Markets)}', '|')");
            }
            if (query.DataType != null)
            {
                andFilters.Add($"{nameof(ApartmentDataIndexDocument.DataType)} eq '{query.DataType.ToString()}'");
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
                var searchResult = await client.Documents.SearchAsync<ApartmentDataIndexDocument>(string.IsNullOrEmpty(query.Phrase) ? "*" : query.Phrase, parameters);
                result.TotalCount = searchResult.Count.GetValueOrDefault();

                result.Results = searchResult.Results.Select(x =>
                {
                    var entry = _mapper.Map<ApartmentDataSearchEntry>(x.Document);
                    entry.Score = x.Score;
                    return entry;
                }).ToList();
            }
            return result;
        }

        public async Task<string[]> FindAllMarketsAsync()
        {
            var parameters = new SearchParameters
            {
                Select = new[] { nameof(ApartmentDataIndexDocument.Id), nameof(ApartmentDataIndexDocument.Market), },
                Facets = new [] { $"{nameof(ApartmentDataIndexDocument.Market)}, count:100"}
            };
            using (var client = CreateClient(_options))
            {
                var searchResult = await client.Documents.SearchAsync<ApartmentDataIndexDocument>("*", parameters);
                return searchResult.Facets[nameof(ApartmentDataIndexDocument.Market)].Select(x => x.Value.ToString()).ToArray();
            }
        }

        public async Task<string[]> SuggestAsync(string phrase, params string[] markets)
        {
            var result = Array.Empty<string>();
     
            if (!string.IsNullOrWhiteSpace(phrase))
            {
                var parameters = new SuggestParameters
                {
                    UseFuzzyMatching = true,
                    Top = 5
                };
                if (!markets.IsNullOrEmpty())
                {
                    parameters.Filter = $"search.in({nameof(ApartmentDataIndexDocument.Market)}, '{string.Join("|", markets)}', '|')";
                }
                using (var client = CreateClient(_options))
                {
                    var suggestResult = await client.Documents.SuggestAsync<ApartmentDataIndexDocument>(phrase, "sg", parameters);
                    result = suggestResult.Results.Select(x => x.Document.ToString()).ToArray();
                }
            }
            return result;
        }

        public async Task<string[]> AutoCompleteAsync(string phrase, params string[] markets)
        {
            var result = Array.Empty<string>();

            if (!string.IsNullOrWhiteSpace(phrase))
            {
                var parameters = new AutocompleteParameters
                {
                    AutocompleteMode = AutocompleteMode.TwoTerms,
                    UseFuzzyMatching = false,
                    Top = 5
                };
                if (!markets.IsNullOrEmpty())
                {
                    parameters.Filter = $"search.in({nameof(ApartmentDataIndexDocument.Market)}, '{string.Join("|", markets)}', '|')";
                }
                using (var client = CreateClient(_options))
                {
                    var autocompleteResult = await client.Documents.AutocompleteAsync(phrase, "sg", parameters);
                    result = autocompleteResult.Results.Select(x => x.Text).ToArray();
                }
            }
            return result;
        }
              

        private static SearchIndexClient CreateClient(AzureSearchOptions options)
        {
            return  new SearchIndexClient(options.SearchServiceName, options.IndexName, new SearchCredentials(options.SearchServiceQueryApiKey));
        }

     
    }
}
