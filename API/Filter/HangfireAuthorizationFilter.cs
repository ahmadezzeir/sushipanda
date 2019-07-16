using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace API.Filter
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            //var httpContext = context.GetHttpContext();
            //var accessToken = httpContext.Request.Query["access_token"][0];
            //var encryptedToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            //var userRole = encryptedToken.Claims.First(x => x.Type == ClaimTypes.Role).Value;

            //return userRole == "admin";

            // TODO: Add cookie authorize

            return true;
        }
    }
}