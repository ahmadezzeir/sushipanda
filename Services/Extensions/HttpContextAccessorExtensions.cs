using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Services.Extensions
{
    public static class HttpContextAccessorExtension
    {
        public static Guid? CurrentUser(this IHttpContextAccessor httpContextAccessor)
        {
            Guid.TryParse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var result);

            return result == Guid.Empty ? (Guid?)null : result;
        }
    }
}