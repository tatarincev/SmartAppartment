using System.Diagnostics;
using System.Threading.Tasks;
using Assessment2.Models;
using Assessment2.Models.Index;
using Assessment2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
namespace Assessment2.Controllers
{
    [Produces("application/json")]
    [Route("api/smart-apartment/docs")]
    public class IndexSearchDataController : Controller
    {
        private readonly IIndexer _indexer;
        private readonly IIndexedSearchService _searchService;
        public IndexSearchDataController(IIndexer indexer, IIndexedSearchService searchService)
        {
            _indexer = indexer;
            _searchService = searchService;
        }

        [HttpPost]
        [Route("index")]
        public ActionResult UploadAndIndexJson(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                 _indexer.DoIndex<ApartmentData>(stream, (x) => Debug.WriteLine(x), new System.Threading.CancellationToken());
            }
            return Ok();
        }

        
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<IndexDocument[]>> SearchDocuments([FromBody] IndexedSearchQuery query)
        {
          
            var result = await _searchService.SearchDocsAsync(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("markets")]
        public async Task<ActionResult<string>> FindAllMarkets()
        {
            var result = await _searchService.FindAllMarketsAsync();
            return Ok(result);
        }
    }
}
