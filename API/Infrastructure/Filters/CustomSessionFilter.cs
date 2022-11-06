using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

#pragma warning disable CS8604 // Possible null reference argument.

namespace API.Infrastructure.Filters
{
    public class CustomSessionFilter : Attribute, IAuthorizationFilter
    {
        private readonly ILogger<CustomSessionFilter> logger;

        public CustomSessionFilter(ILogger<CustomSessionFilter> logger)
        {
            //logger = loggerFactory.CreateLogger<CustomSessionFilter>();
            this.logger = logger;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            HttpContext httpContext = context.HttpContext;
            var user = httpContext.User;
            var session = httpContext.Session;
            string? emailid = user.FindFirst(ClaimTypes.Email)?.Value;
            string? tenantId = user.FindFirst("TenantID")?.Value;
            //session?.SetString("user", emailid);
            //session?.SetString("tenantid", tenantId);
            logger.LogInformation("Custom Session Filter : Adding user {0} to session with tenant id {1}", emailid, tenantId);
        }
    }
}

#pragma warning restore CS8604 // Possible null reference argument.
