using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Infrastructure.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            try
            {
                var user = httpContext.User;
                var session = httpContext.Session;
                string? emailid = user.FindFirst(ClaimTypes.Email)?.Value;
                session?.SetString("user", emailid);
                return _next(httpContext);

            }
            catch (Exception)
            {

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
