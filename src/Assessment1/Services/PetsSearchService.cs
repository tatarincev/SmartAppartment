using AutoMapper;
using Assessment1.Models;
using Assessment1.Nswag;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment1.Services
{
    public class PetsSearchService : IPetsSearchService
    {
        private readonly PetstoreClient _petsApiClient;
        private readonly IMapper _mapper;
        public PetsSearchService(IMapper mapper, PetstoreClient petsApiClient) 
        {
            _petsApiClient = petsApiClient;
            _mapper = mapper;
        }

        public async Task<SearchPetsResult> SearchPetsAsync(SearchPetsQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            //TechDept: Since the public petstore API doesn't support pagination on the server side, we must load all the pets and do this on the client side in memory.            
            var pets = await _petsApiClient.FindPetsByStatusAsync(query.Statuses.Select(x=> (Anonymous)x));
            var result = new SearchPetsResult
            {
                TotalCount = pets.Count,
                //Use automapper for map  dto's types from remote  service into our domains types
                Pets = pets.Skip(query.Skip).Take(query.Take)
                           .Select(x => _mapper.Map<Models.Pet>(x))
            };
            return result;
        }
    }
}
