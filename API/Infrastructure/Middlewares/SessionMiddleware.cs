using System.Security.Claims;

namespace API.Infrastructure.Middlewares
{
#pragma warning disable CS8604 // Possible null reference argument.
    public class CustomSessionMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<CustomSessionMiddleware> logger;

        public CustomSessionMiddleware(RequestDelegate next, ILogger<CustomSessionMiddleware> logger)
        {
            _next = next;
            this.logger = logger;
        }

        public Task Invoke(HttpContext httpContext)
        {
            try
            {
                var user = httpContext.User;
                var session = httpContext.Session;
                string? emailid = user.FindFirst(ClaimTypes.Email)?.Value;
                string? tenantId = user.FindFirst("TenantID")?.Value;
                session?.SetString("user", emailid);
                session?.SetString("tenantid", tenantId);
                logger.LogInformation("Custom Session Middleware : Adding user {0} to session with tenant id {1}",emailid,tenantId);
                return _next(httpContext);

            }
            catch (Exception ex)
            {
                logger.LogError("Custom Session Middleware : No user has been added to session");
                logger.LogError(ex.ToString());
                return _next(httpContext);
            }

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomSessionMiddlewareExtensions
    {
        public static IApplicationBuilder UseSessionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomSessionMiddleware>();
        }
    }
}
#pragma warning restore CS8604 // Possible null reference argument.

