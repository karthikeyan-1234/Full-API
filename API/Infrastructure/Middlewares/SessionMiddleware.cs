using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Infrastructure.Middlewares
{
#pragma warning disable CS8604 // Possible null reference argument.
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
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
                session?.SetString("user", emailid);
                logger.LogInformation("Custom Session Middleware : Adding user {0} to session",emailid);
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

