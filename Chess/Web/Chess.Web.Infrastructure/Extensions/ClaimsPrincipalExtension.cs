using System.Security.Claims;

namespace Chess.Web.Infrastructure.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
