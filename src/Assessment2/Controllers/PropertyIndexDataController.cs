using System.Diagnostics;
using Assessment2.Models;
using Assessment2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assessment2.Controllers
{
    [Produces("application/json")]
    [Route("api/smart-apartment/")]
    public class PropertyIndexDataController : Controller
    {
        private readonly IIndexer _indexer;
        public PropertyIndexDataController(IIndexer indexer)
        {
            _indexer = indexer;
        }
        
        [HttpPost]
        [Route("index")]
        public ActionResult IndexCompanies(IFormFile file)
        {
            using(var stream = file.OpenReadStream())
            {
                _indexer.DoIndex<ApartmentData>("smart-search-index", file.OpenReadStream(), (x) => Debug.WriteLine(x), new System.Threading.CancellationToken());
            }
            return Ok();
        }    
    }
}
