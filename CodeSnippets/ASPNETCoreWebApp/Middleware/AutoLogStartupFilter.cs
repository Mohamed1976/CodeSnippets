using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/*
To configure middleware at the beginning or end of an app's Configure middleware pipeline without an explicit 
call to Use{Middleware}. IStartupFilter is used by ASP.NET Core to add defaults to the beginning of the pipeline 
without having to make the app author explicitly register the default middleware. IStartupFilter allows a 
different component call Use{Middleware} on behalf of the app author. To create a pipeline of Configure 
methods. IStartupFilter.Configure can set a middleware to run before or after middleware added by libraries.
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-3.1
*/

//You are developing a.NET Core library that will be used by multiple applications.
//The library contains ASP.NET Core middleware named EnsureSecurityMiddleware.
//EnsureSecurityMiddleware must always run prior to other middleware.
//You need to configure the middleware. How should you complete the code?

//https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.istartupfilter.configure?view=aspnetcore-3.1
//https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-2.2
//https://andrewlock.net/exploring-istartupfilter-in-asp-net-core/
namespace ASPNETCoreWebApp.Middleware
{
    //IStartupFilter - it is a way to add additional middleware (or other configuration) 
    //at the beginning or end of the configured pipeline.
    public class AutoLogStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return (builder) =>
            {
                builder.UseMiddleware<LogMiddleware>();
                next(builder);
            };
        }
    }
}
