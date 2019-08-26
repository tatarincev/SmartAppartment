using Microsoft.AspNetCore.Authorization;

namespace Assessment1.Authorization
{
    public class PetsAuthorizationRequirement : IAuthorizationRequirement
    {
        public const string OnlySold = "pets:read:sold";
        public const string SeeAll = "pets:read:any";
    }
}
