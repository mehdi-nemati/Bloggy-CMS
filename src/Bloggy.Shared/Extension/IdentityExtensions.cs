using System.Security.Claims;
using System.Security.Principal;

namespace Bloggy.Shared.Extension
{
    public static class IdentityExtensions
    {
        public static int GetUserIdInt(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            return int.Parse(claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
