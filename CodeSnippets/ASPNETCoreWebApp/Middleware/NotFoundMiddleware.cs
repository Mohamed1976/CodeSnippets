using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


//https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-3.1
namespace ASPNETCoreWebApp.Middleware
{
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<NotFoundMiddleware> _logger;
        public NotFoundMiddleware(RequestDelegate next,
            ILogger<NotFoundMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            //You are developing an ASP.NET Core MVC web application that uses custom security middleware. 
            //The middleware will add a response header to stop pages from loading when reflected cross 
            //site scripting(XSS) attacks are detected. The security middleware component must be constructed 
            //once per application lifetime.
            //https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-XSS-Protection
            //httpContext.Response.Headers.Add("X-XSS-Protection", "1; mode=block");

            await _next(httpContext);
            if (httpContext.Response.StatusCode == 404)
            {
                // record the 404 to fix later
                _logger.LogInformation($"Houston we have a 404: {httpContext.Request.Path}");
            }
        }
    }
}
