using System.Threading.Tasks;
using Assessment2.Models;

namespace Assessment2.Services
{
    /// <summary>
    /// Provides an abstraction for searching apartment data in an indexed store
    /// </summary>
    public interface IIndexedSearchService
    {
        Task<AppartmentDataSearchResult> SearchAsync(IndexedSearchQuery query);
        Task<string[]> FindAllMarketsAsync();
        Task<string[]> SuggestAsync(string phrase, params string[] markets);
        Task<string[]> AutoCompleteAsync(string phrase, params string[] markets);
    }
}
