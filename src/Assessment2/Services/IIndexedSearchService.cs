using System.Threading.Tasks;
using Assessment2.Models;
using Assessment2.Models.Index;

namespace Assessment2.Services
{
    public interface IIndexedSearchService
    {
        Task<IndexDocument[]> SearchDocsAsync(IndexedSearchQuery query);
        Task<string[]> FindAllMarketsAsync();
    }
}
