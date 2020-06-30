using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Middleware
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogMiddleware> _logger;
        private long counter = 0;

        public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            //ILoggerFactory logFactory
            //_logger = logFactory.CreateLogger("MyMiddleware");
        }

        public async Task Invoke(HttpContext httpContext)
        {
            // Test with https://localhost:5001/Privacy/?option=Hello
            var option = httpContext.Request.Query["option"];
            if (!string.IsNullOrWhiteSpace(option))
            {
                httpContext.Items["option"] = WebUtility.HtmlEncode(option);
            }

            _logger.LogInformation("**Handling request: " + httpContext.Request.Path + ", counter: " + ++counter);
            //_logger.LogInformation($"LogMiddleware executing..., counter: {++counter}");
            await _next(httpContext); // calling next middleware
            //_logger.LogInformation("LogMiddleware Finished...");
            _logger.LogInformation("**Finished handling request.**");
        }
    }
}
