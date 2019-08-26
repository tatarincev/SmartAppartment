using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Assessment1.Authorization;
using Assessment1.Models;
using Assessment1.Services;
using System.Threading.Tasks;

namespace Assessment1.Controllers
{
    [Produces("application/json")]
    [Route("api/petstore")]
    public class PetstoreDataController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IPetsSearchService _petsSearchService;
        public PetstoreDataController(IAuthorizationService authorizationService, IPetsSearchService petsSearchService)
        {
            _authorizationService = authorizationService;
            _petsSearchService = petsSearchService;
        }

        /// <summary>
        /// Returns the information about the current pets store 
        /// </summary>
        /// <remarks>
        /// Non authorized users can call this API method.
        /// </remarks>
        /// <returns>An information about current store</returns>
        [Route("info")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public ActionResult<SearchPetsResult> GetStoreInfo()
        {          
            return Ok(new StoreInfo { Name = "Scampers Natural Pet Store", Address = "Northfield Business Park, Northfield Rd, Soham, Ely CB7 5UE, UK" });
        }

        /// <summary>
        /// Searches for pets in the current pet store matching a given search query.
        /// </summary>
        /// <remarks>
        /// Only authorized users can call this API method. Also authorization rules will be applied automatically on a passed search query object.
        /// </remarks>
        /// <param name="query">Pets search query</param>
        /// <returns>search result</returns>
        [Route("pets/search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<SearchPetsResult>> SearchPets([FromBody] SearchPetsQuery query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //An AuthorizeAsync overload is invoked to determine whether the current user is allowed to search pets with particular statuses. 
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, query, new PetsAuthorizationRequirement());
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }
            var result = await _petsSearchService.SearchPetsAsync(query);

            return Ok(result);
        }
    }
}
