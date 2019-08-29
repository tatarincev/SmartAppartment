using System.Diagnostics;
using System.Threading.Tasks;
using Assessment2.Models;
using Assessment2.Models.Schema.Json;
using Assessment2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Assessment2.Controllers
{
    [Produces("application/json")]
    [Route("api/apartment-data")]
    public class IndexSearchDataController : Controller
    {
        private readonly ISearchIndexer _indexer;
        private readonly IIndexedSearchService _searchService;
        public IndexSearchDataController(ISearchIndexer indexer, IIndexedSearchService searchService)
        {
            _indexer = indexer;
            _searchService = searchService;
        }

        /// <summary>
        /// Uploads a JSON file and add automatically run an indexation task that will add to an index all documents from this file
        /// </summary>
        /// <param name="file">json file</param>
        /// <returns></returns>
        [HttpPost]
        [Route("index")]
        [Authorize]
        public ActionResult UploadAndIndexJson(IFormFile file)
        {
            if(file == null)
            {
                return BadRequest("Expected a multipart form request");
            }
            //TODO: Run task in background job and report of progress as SignalR push notifications
            //TODO: Add API for availability to  cancel of an running indexation process
            using (var stream = file.OpenReadStream())
            {
                 _indexer.DoIndex<ApartmentDataJsonDocument>(stream, (x) => Debug.WriteLine(x), new System.Threading.CancellationToken());
            }
            return Ok();
        }

        /// <summary>
        /// Searches an apartment data in an index.
        /// </summary>
        /// <param name="query">Search term</param>
        /// <returns>List of documents  are matched to passed search query</returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<AppartmentDataSearchResult>> Search([FromBody] IndexedSearchQuery query)
        {          
            var result = await _searchService.SearchAsync(query);
            return Ok(result);
        }

        /// <summary>
        /// Suggestions for passed term.
        /// </summary>
        /// <param name="phrase">Search term</param>
        /// <param name="markets">It acts as the scope for the search. When ‘market’ is passed in, results should only be returned which match the markets (scope).</param>
        /// <returns>List of suggested text from matching documents</returns>
        [HttpGet]
        [Route("suggest")]
        public async Task<ActionResult<string>> Suggest([FromQuery]string phrase, [FromQuery] string[] markets)
        {
            var result = await _searchService.SuggestAsync(phrase, markets);
            return Ok(result);
        }

        /// <summary>
        /// Auto completes the entered term by data from index.  
        /// </summary>
        /// <param name="phrase">Search term</param>
        /// <param name="markets">It acts as the scope for the search. When ‘market’ is passed in, results should only be returned which match the markets (scope).</param>
        /// <returns>The result of this operation is a list of suggested terms or phrases depending on the mode.</returns>
        [HttpGet]
        [Route("autocomplete")]
        public async Task<ActionResult<string>> AutoComplete([FromQuery] string phrase, [FromQuery] string[] markets)
        {
            var result = await _searchService.AutoCompleteAsync(phrase, markets);
            return Ok(result);
        }

        /// <summary>
        /// List of all markets
        /// </summary>
        /// <returns>List of all markets that present in an index.</returns>
        [HttpGet]
        [Route("markets")]
        public async Task<ActionResult<string>> FindAllMarkets()
        {
            var result = await _searchService.FindAllMarketsAsync();
            return Ok(result);
        }
    }
}
