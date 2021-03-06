﻿#define UseOptions // or NoOptions or UseOptionsAO

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ASPNETCoreWebApp.Data;
using ASPNETCoreWebApp.Helpers;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using ASPNETCoreWebApp.Services;
using System.Net.WebSockets;
using System.Threading;
using System.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ASPNETCoreWebApp.Models;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using System.Reflection;
using ASPNETCoreWebApp.Middleware;
using ASPNETCoreWebApp.SignalRHub;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace ASPNETCoreWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //DbContext/DutchContext registration
            services.AddDbContext<DutchContext>(options =>
            { 
                options.UseSqlServer(Configuration.GetConnectionString("DutchConnectionString"));
            });

            //DbContext/StudentDataContext registration
            services.AddDbContext<StudentDataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("StudentDataContext"));
            });

            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
            });

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //services.AddAutoMapper(typeof(Startup));
            //Registered service, are primarily used in controllers
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<DutchSeeder>();
            services.AddScoped<IDutchRepository, DutchRepository>();
            services.AddSingleton<ICustomersRepository, CustomersRepository>();
            services.AddSingleton<IWebServiceReader, WebServiceReader>();

            //Register the Model provider at the beginning of the list in Startup.ConfigureServices:
            services.AddMvc((options) =>
            {
                options.ModelBinderProviders.Insert(0, new SplitDateTimeModelBinderProvider());
            }).SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

            //The SignalR server must be configured to pass SignalR requests to SignalR.
            services.AddSignalR();

            //Step by step configuring ASP.NET core Identity
            //services.AddIdentity<ApplicationUser, IdentityRole>();
            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.Password.RequireDigit = true;
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            //});

            //Add ASP.NET core Identity 
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
           .AddEntityFrameworkStores<AppIdentityDbContext>()
           .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                //OptionsValidationException: Cookie.Expiration is ignored, use ExpireTimeSpan instead.
                //options.Cookie.Expiration = TimeSpan.FromDays(90);
                options.ExpireTimeSpan = TimeSpan.FromDays(90);
                options.SlidingExpiration = true;
            });

            //Use JSON Web Tokens (JWT) to authenticate the method callers to Web API  
            services.AddAuthentication(options =>
            {
                //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddJwtBearer(cfg => //"JwtBearer", cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidAudience = Configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                    };
                });
            
            Debug.WriteLine($"Tokens:Issuer {Configuration["Tokens:Issuer"]}");
            Debug.WriteLine($"Tokens:Audience {Configuration["Tokens:Audience"]}");
            Debug.WriteLine($"Tokens:Key {Configuration["Tokens:Key"]}");

            services.AddAuthorization(config =>
            {
                config.AddPolicy("UserPolicy", policy =>
                {
                    //policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("IsAdult", "Yes");
                });
            });

            //ProductsController
            //By setting CompatibilityVersion, we can use features to validate response type,
            //and validating that it is an api controller type, validating what it produces
            //[ProducesResponseType(200)]
            //[ProducesResponseType(400)]
            //.SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

            //AddAuthentication(string defaultScheme
            //services.AddAuthentication(IISDefaults.AuthenticationScheme);
            //https://gist.github.com/Gimly/74bd82e0f8990646a8b3751c43efbf81
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(o =>
            //{
            //    o.Authority = Configuration["Jwt:Authority"];
            //    o.Audience = Configuration["Jwt:Audience"];
            //    o.Events = new JwtBearerEvents()
            //    {
            //        OnAuthenticationFailed = c =>
            //        {
            //            c.NoResult();

            //            c.Response.StatusCode = 500;
            //            c.Response.ContentType = "text/plain";
            //            //if (Environment.IsDevelopment())
            //            //{
            //                return c.Response.WriteAsync(c.Exception.ToString());
            //            //}
            //            return c.Response.WriteAsync("An error occured processing your authentication.");
            //        }
            //    };
            //});

            #region [ Localization ]

            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-3.1#using-one-resource-string-for-multiple-classes
            //Resources are named for the full type name of their class minus the assembly name. 
            //For example, a French resource in a project whose main assembly is LocalizationWebsite.Web.dll 
            //for the class LocalizationWebsite.Web.Startup would be named Startup.fr.resx. A resource 
            //for the class LocalizationWebsite.Web.Controllers.HomeController would be named Controllers.
            //HomeController.fr.resx. If your targeted class's namespace isn't the same as the assembly 
            //name you will need the full type name. For example, in the sample project a resource for 
            //the type ExtraNamespace.Tools would be named ExtraNamespace.Tools.fr.resx.
            //In the sample project, the ConfigureServices method sets the ResourcesPath to "Resources", 
            //so the project relative path for the home controller's French resource file is 
            //Resources/Controllers.HomeController.fr.resx. Alternatively, you can use folders to 
            //organize resource files. For the home controller, the path would be 
            //Resources/Controllers/HomeController.fr.resx. If you don't use the ResourcesPath option, 
            //the .resx file would go in the project base directory. The resource file for 
            //HomeController would be named Controllers.HomeController.fr.resx. The choice of 
            //using the dot or path naming convention depends on how you want to organize your resource files.
            //Resources/Controllers.HomeController.fr.resx
            //Resources/Controllers/HomeController.fr.resx
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            #endregion


            //QueryStringRequestCultureProvider
            //Some apps will use a query string to set the culture and UI culture. 
            //For apps that use the cookie or Accept-Language header approach, adding 
            //a query string to the URL is useful for debugging and testing code.
            //By default, the QueryStringRequestCultureProvider is registered as the 
            //first localization provider in the RequestCultureProvider list. 
            //You pass the query string parameters culture and ui - culture. 
            //The following example sets the specific culture(language and region) to Spanish / Mexico:
            //http://localhost:5000/?culture=es-MX&ui-culture=es-MX
            //If you only pass in one of the two(culture or ui-culture), the query 
            //string provider will set both values using the one you passed in. 
            //For example, setting just the culture will set both the Culture and the UICulture:
            //http://localhost:5000/?culture=es-MX

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                        new CultureInfo("en-US"),
                        new CultureInfo("fr-CA"),
                        new CultureInfo("en-UK"),
                        new CultureInfo("es-MX")
                };

                //The culture set on the server is the default culture, and content is returned
                //to all clients in this culture if the application has not been globalized.
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };
            });

            //string enUSCulture = "en-US"; //"fr-CA"
            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    var supportedCultures = new[]
            //    {
            //        new CultureInfo(enUSCulture),
            //        new CultureInfo("fr")
            //    };

            //    options.DefaultRequestCulture = new RequestCulture(culture: enUSCulture, uiCulture: enUSCulture);
            //    options.SupportedCultures = supportedCultures;
            //    options.SupportedUICultures = supportedCultures;

            //    options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(async context =>
            //    {
            //        // My custom request culture logic
            //        return new ProviderCultureResult("en");
            //    }));
            //});

            //Using one resource string for multiple classes
            //The following code shows how to use one resource string for validation attributes with multiple classes:
            //services.AddMvc()
            //    .AddDataAnnotationsLocalization(options => {
            //        options.DataAnnotationLocalizerProvider = (type, factory) =>
            //            factory.Create(typeof(SharedResource));
            //    });
            //In the preceding code, SharedResource is the class corresponding to the resx 
            //where your validation messages are stored.With this approach, DataAnnotations 
            //will only use SharedResource, rather than the resource for each class.

            //Using respone caching
            services.AddResponseCaching();

            services.AddControllersWithViews();
        }

        //Configure order of middleware is important, middleware is loosly coupled 
        //The order in which middleware is called matters. for example authenticate user before rendering page 
        //app.UseDefaultFiles(); //Called first, returns root path to index.html in wwwroot
        //app.UseStaticFiles();  //Called second
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseNotFoundMiddleware();

            //Catch all requests and return Hello World
            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("<html><body><h1>Hello World</h1></body></html>");
            //});

            if (env.IsDevelopment())
            {
                //When building simple pages razor pages can save you time
                //Refers to razor pages directory in Pages/Error.cshtml
                //app.UseExceptionHandler("/Error");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //websocket communication explained
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/websockets?view=aspnetcore-3.1
            //https://github.com/dotnet/AspNetCore.Docs/tree/master/aspnetcore/fundamentals/websockets/samples/2.x/WebSocketsSample
            #region [ WebSocket ]

#if NoOptions
            #region UseWebSockets
            app.UseWebSockets();
            #endregion
#endif
#if UseOptions
            #region UseWebSocketsOptions
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            //app.UseWebSockets();
            app.UseWebSockets(webSocketOptions);
            #endregion
#endif
#if UseOptionsAO
            #region UseWebSocketsOptionsAO
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            webSocketOptions.AllowedOrigins.Add("https://client.com");
            webSocketOptions.AllowedOrigins.Add("https://www.client.com");

            app.UseWebSockets(webSocketOptions);
            #endregion
#endif

            #endregion

            #region [ AcceptWebSocket ]

            //To access Websocket enter url: http://localhost:5001/websocket
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    Trace.WriteLine($"context.Request.Path == \"/ws\"");
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        Trace.WriteLine("Opening WebSocket");
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        await Echo(context, webSocket);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }

            });

            #endregion
            //Add SignalR to the ASP.NET Core dependency injection system and the middleware pipeline.
            //Todo [Obsolete] Should use Map
            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<ChatHub>("/chathub");
            //});

            // app.UseFileServer();

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("fr-CA"),
                new CultureInfo("en-UK"),
                new CultureInfo("es-MX")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            app.UseHttpsRedirection();

            //The UseDefaultFiles configures the DefaultFiles middleware which is a part of StaticFiles 
            //middleware. This will automatically serve html file named default.html, default.htm, 
            //index.html or index.htm
            //https://www.tutorialsteacher.com/core/aspnet-core-static-file
            //https://stackoverflow.com/questions/47351712/app-usedefaultfiles-in-kestrel-not-doing-anything
            //app.UseDefaultFiles();
            app.UseStaticFiles();

            //To use static files uncomment below.
/*          var cachePeriod = env.IsDevelopment() ? "60" : "604800";
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    // Requires the following import:
                    // using Microsoft.AspNetCore.Http;
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}");
                }
            });

            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-3.1
            //https://github.com/dotnet/AspNetCore.Docs/tree/master/aspnetcore/fundamentals/static-files/samples/1x/MyStaticFiles
            //In the preceding code, the MyStaticFiles directory hierarchy is exposed publicly via the StaticFiles 
            //URI segment.A request to http://<server_address>/StaticFiles/images/image.jpg serves the image.jpg file.
            //The following markup references StaticFiles/images/image.jpg:
            //<img src="~/StaticFiles/images/image.jpg" alt="ASP.NET" class="img-responsive" />
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "ContentFiles")),
                RequestPath = "/StaticFiles"
            });
*/            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //https://stackoverflow.com/questions/48185985/aspnetcore-signalr-2-1-and-cors
                //https://github.com/dotnet/docs/issues/14928
                endpoints.MapHub<ChatHub>("/chathub");
            });
        }

        #region [ Echo ]
        
        private async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = 
                await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        #endregion

    }
}
