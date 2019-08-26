using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Assessment1.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment1.Authorization
{
    ///<summary>
    /// This class contains logic for determining whether PetsAuthorizationRequirement in authorization
    /// policies are satisfied or not 
    /// </summary>
    public class PetsAuthorizationHandler : AuthorizationHandler<PetsAuthorizationRequirement>
    {
        private readonly AuthOptions _authOptions;
        public PetsAuthorizationHandler(IOptions<AuthOptions> options)
        {
            _authOptions = options.Value;
        }

        // Check whether a given PetsAuthorizationRequirement is satisfied or not for a particular context
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PetsAuthorizationRequirement requirement)
        {
            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "permissions" && c.Issuer == _authOptions.Domain))
                return Task.CompletedTask;

            // Split the scopes string into an array
            var permissions = context.User.FindFirst(c => c.Type == "permissions" && c.Issuer == _authOptions.Domain).Value.Split(' ');

            // Succeed if the scope array contains the required scope
            if (permissions.Any(s => s == PetsAuthorizationRequirement.SeeAll))
            {
                context.Succeed(requirement);
            }
            else if (permissions.Any(s => s == PetsAuthorizationRequirement.OnlySold) && context.Resource is SearchPetsQuery query)
            {
                //Left only Sold status even if it isn't passed
                query.Statuses = new[] { PetStatus.Sold };
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

